using pac_man.ECS;
using pac_man.ECS.Ghosts;
using pac_man.Generics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pac_man {
	internal static class GhostManager {
		public static List<Ghost> Ghosts { get; } = new();
		public static List<Point>? SpawnPositions { get; set; } = null;
		public static int MaxGhostCount => SpawnPositions?.Count ?? 0;
		private static readonly Random _rng = new Random(Guid.NewGuid().GetHashCode());

		public static void Update(List<Block> walls, Player player) {
			List<Ghost> ghosts = Ghosts.ToList();
			foreach (Ghost ghost in ghosts) {
				GraphicsManager.ClearPosition(ghost.Position);
				ghost.AI(walls, player);
			}

			if (SpawnPositions is null)
				throw new NullReferenceException("not set spawn position\nset the SpawnPosition [List<Point>] attribute");

			//if (!Ghosts.OfType<RedGhost>().Any())
			if (Ghosts.Count != MaxGhostCount) {
				Point spawnPosition;
				do {
					spawnPosition = SpawnPositions.ElementAt(_rng.Next(0, SpawnPositions.Count));
				} while (Ghosts.FirstOrDefault((g) => g.Position.Equals(spawnPosition)) is not null && Ghosts.Count(g => SpawnPositions.Contains(g.Position)) != SpawnPositions.Count);

				if (!Ghosts.OfType<RedGhost>().Any()) {
					Ghosts.Add(new RedGhost("Blinky", position: spawnPosition));
				} else if (!Ghosts.OfType<PinkGhost>().Any()) {
					Ghosts.Add(new PinkGhost("Pinky", position: spawnPosition));
				} else if (!Ghosts.OfType<BlueGhost>().Any()) {
					Ghosts.Add(new BlueGhost("Inky", position: spawnPosition));
				} else if (!Ghosts.OfType<OrangeGhost>().Any()) {
					Ghosts.Add(new OrangeGhost("Clyde", position: spawnPosition));
				}
			}
		}

		public static void Draw() {
			foreach (Ghost ghost in Ghosts) {
				ghost.Draw();
			}
		}
	}
}
