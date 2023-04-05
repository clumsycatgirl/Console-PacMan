using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pac_man.Generics {
	internal readonly record struct Vector(Vector.Direction Dir, int Amount = 1) {
		public enum Direction {
			Up,
			Down,
			Left,
			Right,
		}
		public static Vector FromString(string input, int movementAmount = 1) =>
				new Vector(input switch {
					"a" => Direction.Left,
					"d" => Direction.Right,
					"w" => Direction.Up,
					"s" => Direction.Down,
					_ => throw new InvalidEnumArgumentException("")
				}, movementAmount);
	}

}
