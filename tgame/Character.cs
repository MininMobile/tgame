using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tgame {
	public class Character : Entity {
		public Character(int startx, int starty) {
			X = startx;
			Y = starty;
			Facing = Direction.Right;
		}

		public Entity Shoot() {
			return new Entity();
		}
	}
}
