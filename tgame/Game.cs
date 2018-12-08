using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace tgame {
	public class Game {
		public int Width { get; }
		public int Height { get; }
		public Character Player { get; }

		List<List<Char>> display = new List<List<Char>>() { };
		List<Lazer> lazers = new List<Lazer>() { };

		public Game(List<List<string>> level) {
			// prepare level
			foreach (var l in level) {
				List<Char> line = new List<Char>() { };

				foreach(var c in l) {
					line.Add(StringToChar(c[0]));
				}

				display.Add(line);
			}

			// set variables
			Width = level[0].ToArray().Length;
			Height = level.ToArray().Length;

			Player = new Character(Width / 2 - 1, Height / 2 - 1, true);

			// initialize console
			Console.SetWindowSize(Width + 1, Height + 1);
			Console.SetBufferSize(Width + 1, Height + 1);
			Console.CursorVisible = false;

			// initialize display
			for (int i = 0; i < Height; i++) {
				display.Add(new List<Char>() { });
				for (int j = 0; j < Width; j++) {
					if (j == (Width / 4) * 3)
						display[i].Add(Char.Wall);
					else
						display[i].Add(Char.Nothing);
				}
			}

			display[Player.Y][Player.X] = Char.Player;

			// initialize threads
			Dictionary<string, Thread> threads = new Dictionary<string, Thread>() {
				{ "update", new Thread(UpdateThread) },
				{ "input", new Thread(InputThread) },
				{ "render", new Thread(RenderThread) }
			};

			foreach (var thread in threads) {
				thread.Value.Start();
			}

			void RenderThread() {
				while (true) {
					Render();

					Thread.Sleep(100);
				}
			}

			void UpdateThread() {
				while (true) {
					for (int i = 0; i < lazers.ToArray().Length; i++) {
						Lazer l = lazers[i];

						if (l.Age == 0) {
							if (display[l.Y][l.X] == Char.Wall) {
								lazers.Remove(l);
								return;
							}

							l.Age++;
							display[l.Y][l.X] = l.Rep;
							continue;
						}

						l.Age++;

						switch (l.Facing) {
							case Direction.Up:
								updateEntityLocation(l.X, l.Y, l.X, l.Y - 1, Direction.Up, l);
								break;
							case Direction.Right:
								updateEntityLocation(l.X, l.Y, l.X + 1, l.Y, Direction.Right, l);
								break;
							case Direction.Down:
								updateEntityLocation(l.X, l.Y, l.X, l.Y + 1, Direction.Down, l);
								break;
							case Direction.Left:
								updateEntityLocation(l.X, l.Y, l.X - 1, l.Y, Direction.Left, l);
								break;
						}
					}

					Thread.Sleep(100);
				}
			}

			void InputThread() {
				while (true) {
					// get key
					switch (Console.ReadKey().KeyChar) {
						case 'w':
							updateEntityLocation(Player.X, Player.Y, Player.X, Player.Y - 1, Direction.Up, Player);
							break;
						case 'a': 
							updateEntityLocation(Player.X, Player.Y, Player.X - 1, Player.Y, Direction.Left, Player);
							break;
						case 's': 
							updateEntityLocation(Player.X, Player.Y, Player.X, Player.Y + 1, Direction.Down, Player);
							break;
						case 'd': 
							updateEntityLocation(Player.X, Player.Y, Player.X + 1, Player.Y, Direction.Right, Player);
							break;
						case ' ':
							lazers.Add(Player.Shoot());
							break;
					}
				}
			}
		}

		void updateEntityLocation(int prevx, int prevy, int curx, int cury, Direction facing, Entity entity) {
			entity.Facing = facing;

			if (Height - 1 <= cury || 0 >= cury) {
				CallCollisionAction(); return;
			} else if (Width - 1 <= curx || 0 >= curx) {
				CallCollisionAction(); return;
			} else if (display[cury][curx] == Char.Wall) {
				CallCollisionAction(true); return;
			}

			display[prevy][prevx] = Char.Nothing;
			display[cury][curx] = entity.Rep;

			entity.X = curx;
			entity.Y = cury;

			void CallCollisionAction(bool onWall = false) {
				switch (entity.CollisionAction) {
					case CollisionAction.Die:
						if (entity.GetType() == typeof(Lazer)) {
							if (onWall)
								display[cury][curx] = Char.Wall;

							display[prevy][prevx] = Char.Nothing;
							lazers.Remove((Lazer)entity);
						}
						break;
					case CollisionAction.Stop:
						break;
				}
			}
		}

		public void Render() {
			string buffer = "";

			for (int i = 0; i < Height; i++) {
				for (int j = 0; j < Width; j++)
					if (j == 0 || j == Width - 1 || i == 0 || i == Height - 1)
						buffer += "#";
					else
						buffer += CharToString(display[i][j]);
 
				if (i != Height - 1)
					buffer += '\n';
			}

			Console.Clear();
			Console.Write(buffer);
		}

		public static char CharToString(Char c) {
			switch (c) {
				case Char.Nothing:
					return ' ';
				case Char.Wall:
					return '#';
				case Char.Player:
					return '@';
				case Char.Enemy:
					return '!';
				case Char.Lazer:
					return '+';
				default:
					return ' ';
			}
		}

		public static Char StringToChar(char s) {
			switch (s) {
				case ' ':
					return Char.Nothing;
				case '#':
					return Char.Wall;
				case '@':
					return Char.Player;
				case '!':
					return Char.Enemy;
				case '+':
					return Char.Lazer;
				default:
					return Char.Nothing;
			}
		}
	}

	public enum Char {
		Nothing,
		Wall,
		Player,
		Enemy,
		Lazer,
	}

	public enum Direction {
		Up,
		Right,
		Down,
		Left
	}

	public enum CollisionAction {
		Die,
		Stop
	}
}
