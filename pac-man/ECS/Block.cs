using pac_man.Generics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pac_man.ECS {
	internal class Block : Entity {
		public static readonly DrawData WallDrawData = new('+', Color.DarkBlue);
		public Block(string nameIdentifier, Point? position = null, DrawData? drawData = null) : base(nameIdentifier, position, drawData ?? WallDrawData) { }
		public override void Move(Vector movement) => throw new InvalidOperationException("blocks can't move");
	}
}
