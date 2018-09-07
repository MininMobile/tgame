using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tgame {
	public class Game {
		public int Width { get; }
		public int Height { get; }

		List<List<Char>> display = new List<List<Char>>() { };

		public Game(int width = 128, int height = 32) {
			Width = width;
			Height = height;
			Console.SetWindowSize(Width + 1, Height);
			Console.SetBufferSize(Width + 1, Height);
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
					return '=';
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
}
