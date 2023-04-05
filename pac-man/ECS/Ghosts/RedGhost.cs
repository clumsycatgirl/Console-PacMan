using pac_man.Generics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace pac_man.ECS.Ghosts {
	internal class RedGhost : Ghost {
		public static readonly DrawData GhostDrawData = new('G', Color.Red);
		public RedGhost(string nameIdentifier, Point? position = null, DrawData? drawData = null) : base(nameIdentifier, position, drawData ?? GhostDrawData) {
			positionUpdateMaxCounter = positionUpdateMaxCounter = 3;
		}
		protected override Point GetDestinationPoint(List<Block> walls) =>
			Point.PathUtils.ConstructGraph(walls.Select(w => w.Position).ToList()).OrderByDescending((p) => p.Y).ThenBy((p) => p.X).First();
	}
}
