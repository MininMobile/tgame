using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tgame {
	// base
	public class Entity {
		public int X { get; set; }
		public int Y { get; set; }
		public Char Rep { get; set; }
		public Direction Facing { get; set; }

		public CollisionAction CollisionAction = CollisionAction.Die;
	}

	// entities
	public class Character : Entity {
		public Character(int startx, int starty, bool main = false) {
			X = startx;
			Y = starty;
			Facing = Direction.Right;

			Rep = main ? Char.Player : Char.Enemy;
			CollisionAction = CollisionAction.Stop;
		}

		public Lazer Shoot() {
			return new Lazer(
				Facing == Direction.Left || Facing == Direction.Right ? Facing == Direction.Left ? X - 1 : X + 1 : X,
				Facing == Direction.Up || Facing == Direction.Down ? Facing == Direction.Up ? Y - 1 : Y + 1 : Y,
				Facing
			);
		}
	}

	public class Lazer : Entity {
		public int Age { get; set; }

		public Lazer(int startx, int starty, Direction direction) {
			X = startx;
			Y = starty;
			Facing = direction;

			Rep = Char.Lazer;

			Age = 0;
		}
	}
}
