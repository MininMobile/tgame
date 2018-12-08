using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace tgame {
	class Program {
		static void Main(string[] args) {
			string[] rawLevel = File.ReadAllLines("levels\\level1.dat");

			List<List<string>> level = new List<List<string>>() { };

			foreach (string line in rawLevel) {
				level.Add(line.Split('|').ToList());
			}

			Game game = new Game(level);
		}
	}
}
