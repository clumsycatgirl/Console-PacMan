using pac_man.Generics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pac_man.ECS.Ghosts {
	internal class BlueGhost : Ghost {
		public static readonly DrawData GhostDrawData = new('G', Color.Blue);
		public BlueGhost(string nameIdentifier, Point? position = null, DrawData? drawData = null) : base(nameIdentifier, position, drawData ?? GhostDrawData) {
			positionUpdateMaxCounter = positionUpdateMaxCounter = 6;
		}
		protected override Point GetDestinationPoint(List<Block> walls) =>
			Point.PathUtils.ConstructGraph(walls.Select(w => w.Position).ToList()).OrderByDescending((p) => p.Y).ThenByDescending((p) => p.X).First();
	}
}
