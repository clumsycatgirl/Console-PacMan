using pac_man.Generics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pac_man.ECS.Ghosts {
	internal abstract class Ghost : Entity {
		protected static Random _rng = new(Guid.NewGuid().GetHashCode());
		protected int positionUpdateMaxCounter;
		protected int positionUpdateCounter;
		protected List<Point>? currentPath = null;
		public Ghost(string nameIdentifier, Point? position = null, DrawData? drawData = null) : base(nameIdentifier, position, drawData) { }
		public virtual void AI(List<Block> walls, Player player) {
			if (player.PowerUp)
				ScatterMode(walls, player);
			else
				ChaseMode(walls, player);
		}
		public virtual void ChaseMode(List<Block> walls, Player player) {
			if (walls is null) {
				return;
			}
			Point newPosition = new();
			if (positionUpdateCounter == 0 || currentPath is null) {
				currentPath = Point.PathUtils.FindShortestPath(Position, player.Position, walls.Select(b => b.Position).ToList());
				if (currentPath is not null) {
					currentPath.RemoveAt(0);
					newPosition = currentPath.ElementAt(0);
					currentPath.RemoveAt(0);
				} else {
					newPosition = Position;
				}
				positionUpdateCounter = positionUpdateMaxCounter;
			} else {
				try {
					newPosition = currentPath.ElementAt(0);
					currentPath.RemoveAt(0);
					positionUpdateCounter--;
				} catch {
					currentPath = Point.PathUtils.FindShortestPath(Position, player.Position, walls.Select(b => b.Position).ToList());
					newPosition = currentPath.ElementAt(1);
					currentPath.RemoveRange(0, 2);
					positionUpdateCounter = positionUpdateMaxCounter;
				}
			}
			if (GhostManager.Ghosts.FirstOrDefault((g) => !g.Equals(this) && g.Position.Equals(newPosition)) is Ghost collidingGhost && collidingGhost is not null) {
				newPosition = Position;
			}

			if (newPosition.Equals(player.Position)) {
				player.Living = (player.PowerUp) ? true : false;
				if (player.PowerUp) {
					GhostManager.Ghosts.Remove(this);
				}
			}
			Position = newPosition;
		}
		public virtual void ScatterMode(List<Block> walls, Player player) {
			if (walls is null) {
				return;
			}
			Point newPosition = new();
			if (positionUpdateCounter == 0 || currentPath is null) {
				currentPath = Point.PathUtils.FindShortestPath(Position, GetDestinationPoint(walls), walls.Select(b => b.Position).ToList());
				if (currentPath is not null) {
					try {
						currentPath.RemoveAt(0);
						newPosition = currentPath.ElementAt(0);
						currentPath.RemoveAt(0);
					} catch {
						newPosition = Position;
					}
				} else {
					newPosition = Position;
				}
				positionUpdateCounter = positionUpdateMaxCounter;
			} else {
				try {
					newPosition = currentPath.ElementAt(0);
					currentPath.RemoveAt(0);
					positionUpdateCounter--;
				} catch {
					currentPath = Point.PathUtils.FindShortestPath(Position, player.Position, walls.Select(b => b.Position).ToList());
					newPosition = currentPath.ElementAt(1);
					currentPath.RemoveRange(0, 2);
					positionUpdateCounter = positionUpdateMaxCounter;
				}
			}
			if (GhostManager.Ghosts.FirstOrDefault((g) => !g.Equals(this) && g.Position.Equals(newPosition)) is Ghost collidingGhost && collidingGhost is not null) {
				newPosition = Position;
			}

			if (newPosition.Equals(player.Position)) {
				player.Living = (player.PowerUp) ? true : false;
				if (player.PowerUp) {
					GhostManager.Ghosts.Remove(this);
				}
			}
			Position = newPosition;
		}

		protected abstract Point GetDestinationPoint(List<Block> walls);
	}
}

