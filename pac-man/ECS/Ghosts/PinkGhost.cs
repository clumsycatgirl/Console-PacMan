using pac_man.Generics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pac_man.ECS.Ghosts {
	internal class PinkGhost : Ghost {
		public static readonly DrawData GhostDrawData = new('G', Color.Magenta);
		public PinkGhost(string nameIdentifier, Point? position = null, DrawData? drawData = null) : base(nameIdentifier, position, drawData ?? GhostDrawData) {
			positionUpdateMaxCounter = positionUpdateMaxCounter = 5;
		}
		protected override Point GetDestinationPoint(List<Block> walls) =>
			Point.PathUtils.ConstructGraph(walls.Select(w => w.Position).ToList()).OrderBy((p) => p.Y).ThenBy((p) => p.X).First();
	}
}
