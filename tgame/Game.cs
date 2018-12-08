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
		List<Lazer> bullets = new List<Lazer>() { };

		public Game(int width = 64, int height = 16) {
			Width = width;
			Height = height;
			Player = new Character(width / 2 - 1, height / 2 - 1);
			Console.SetWindowSize(Width + 1, Height + 1);
			Console.SetBufferSize(Width + 1, Height + 1);
			Console.CursorVisible = false;

			// initialize display
			for (int i = 0; i < Height; i++) {
				display.Add(new List<Char>() { });
				for (int j = 0; j < Width; j++) {
					if (j == 0 || j == Width - 1 || i == 0 || i == Height - 1)
						display[i].Add(Char.Wall);
					else
						display[i].Add(Char.Nothing);
				}
			}

			display[Player.Y][Player.X] = Char.Player;

			// initialize threads
			Dictionary<string, Thread> threads = new Dictionary<string, Thread>() {
				{ "render", new Thread(RenderThread) },
				{ "input", new Thread(InputThread) }
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

			void InputThread() {
				while (true) {
					// get key
					switch (Console.ReadKey().KeyChar) {
						case 'w':
							updatePlayerLocation(Player.X, Player.Y, Player.X, Player.Y - 1, Direction.Up);
							break;
						case 'a':
							updatePlayerLocation(Player.X, Player.Y, Player.X - 1, Player.Y, Direction.Down);
							break;
						case 's':
							updatePlayerLocation(Player.X, Player.Y, Player.X, Player.Y + 1, Direction.Left);
							break;
						case 'd':
							updatePlayerLocation(Player.X, Player.Y, Player.X + 1, Player.Y, Direction.Right);
							break;
						case ' ':
							bullets.Add(Player.Shoot());
							break;
					}
				}
			}
		}

		void updatePlayerLocation(int prevx, int prevy, int curx, int cury, Direction facing) {
			display[prevy][prevx] = Char.Nothing;
			display[cury][curx] = Char.Player;

			Player.X = curx;
			Player.Y = cury;
			Player.Facing = facing;
		}

		public void Render() {
			string buffer = "";

			for (int i = 0; i < Height; i++) {
				for (int j = 0; j < Width; j++)
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
				case Char.Lazer:
					return '+';
				default:
					return ' ';
			}
		}
	}

	public enum Char {
		Nothing,
		Wall,
		Player,
		Lazer,
	}

	public enum Direction {
		Up,
		Right,
		Down,
		Left
	}
}
