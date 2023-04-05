using pac_man.Generics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pac_man.ECS {
	internal class Dot : Entity {
		public static readonly DrawData DotDrawData = new DrawData('*', Color.Yellow);
		public static readonly int Value = 10;
		public Dot(string nameIdentifier, Point? position = null, DrawData? drawData = null) : base(nameIdentifier, position, drawData ?? DotDrawData) { }
		public override void Move(Vector movement) => throw new InvalidOperationException("dots can't move");
	}
}
