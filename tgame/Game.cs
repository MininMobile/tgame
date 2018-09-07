using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tgame {
	public class Game {
		List<List<Char>> display = new List<List<Char>>() { };

		public int Width { get; }
		public int Height { get; }

		public Game(int width = 128, int height = 32) {
			Width = width;
			Height = height;
			Console.SetWindowSize(Width, Height);
			Console.SetBufferSize(Width, Height);

			// initialize display
			for (int i = 0; i < Height; i++) {
				display.Add(new List<Char>() { });
				for (int j = 0; j < Width; j++) {
					display[i].Add(Char.Empty);
				}
			}
		}
	}

	enum Char {
		Empty,
		Wall,
		Player,
		Lazer,
	}
}
