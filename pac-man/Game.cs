using pac_man.ECS;
using pac_man.Generics;
using pac_man.Maps;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NCurses;

namespace pac_man {
	internal class Game : IDisposable {
		private readonly Player player;
		private Map activeMap;

		public Game() {
			Console.CursorVisible = false;

			MapManager.LoadMap("map_1.json");
			activeMap = MapManager.Maps["map_1"];

			GhostManager.SpawnPositions = activeMap.GhostSpawnPositions.ToList();

			player = new Player("pac-man", position: activeMap.PlayerSpawnPosition);
		}

		public void Run() {
			InputHandler.Begin();

			foreach (Block wall in activeMap.Walls)
				wall.Draw();

			while (player.Living && player.Score < activeMap.TotalScore) {
				foreach (Dot dot in activeMap.Dots)
					dot.Draw();

				foreach (PowerUp powerup in activeMap.PowerUps)
					powerup.Draw();

				GhostManager.Draw();

				player.Draw();

				Refresh();

				Vector? movement = null;
				try {
					movement = Vector.FromString(InputHandler.Input);
				} catch { }
				GraphicsManager.ClearPosition(player.Position);
				player.Move(movement ?? new Vector(player.FacingDirection), activeMap.Walls, activeMap.Dots, activeMap.PowerUps);

				GhostManager.Update(activeMap.Walls, player);
			}
		}

		private void Refresh() {
			Thread.Sleep(1000 / 30 * 8);
		}

		public void Dispose() {
			InputHandler.End();
		}
	}
}
