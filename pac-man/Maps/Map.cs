using pac_man.ECS;
using pac_man.Generics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pac_man.Maps {
	internal class Map {
		public List<Block> Walls { get; set; }
		public List<Dot> Dots { get; set; }
		public Point PlayerSpawnPosition { get; set; }
		public List<Point> GhostSpawnPositions { get; set; }
		public List<PowerUp> PowerUps { get; set; }
		public int TotalScore { get; set; }

		public Map() {
			Walls = new List<Block>();
			Dots = new List<Dot>();
			GhostSpawnPositions = new List<Point>();
			PowerUps = new List<PowerUp>();
			TotalScore = 0;
		}

		public void UpdateTotalScore() {
			TotalScore = Dot.Value * Dots.Count;
		}

		public override string ToString() => $"{PlayerSpawnPosition}, Walls-Count: {Walls.Count}, Entities-Count: {Dots.Count}, Ghost-Spawn-Count: {GhostSpawnPositions.Count}, Power-Up-Count: {PowerUps.Count}";
	}
}
