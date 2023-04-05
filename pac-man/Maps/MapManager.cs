using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Diagnostics;
using pac_man.ECS;
using pac_man.Generics;

namespace pac_man.Maps {
	internal static class MapManager {
		private class ItemData {
			public enum ItemType {
				Wall,
				Dot,
				GhostSpawn,
				PlayerSpawn,
				PowerUp
			}
			public string Type { get; set; }
			public int X { get; set; }
			public int Y { get; set; }
			public override string ToString() => $"Name:{Type}, X:{X}, Y:{Y}";
		}
		public static Dictionary<string, Map> Maps { get; set; } = new();

		public static void LoadMap(string file) {
			string data = File.ReadAllText(file);
			List<ItemData> items = JsonSerializer.Deserialize<List<ItemData>>(data) ?? new();
			Map map = new();
			int wallCounter = 0, dotCounter = 0, ghostSpawnCounter = 0, powerupCounter = 0;
			foreach (ItemData item in items) {
				switch (item.Type) {
					case nameof(ItemData.ItemType.Wall):
						map.Walls.Add(new Block($"wall_{wallCounter}", new Point(item.X, item.Y)));
						wallCounter++;
						break;
					case nameof(ItemData.ItemType.Dot):
						map.Dots.Add(new Dot($"dot_{dotCounter}", new Point(item.X, item.Y)));
						dotCounter++;
						break;
					case nameof(ItemData.ItemType.GhostSpawn):
						map.GhostSpawnPositions.Add(new Point(item.X, item.Y));
						ghostSpawnCounter++;
						break;
					case nameof(ItemData.ItemType.PlayerSpawn):
						map.PlayerSpawnPosition = new Point(item.X, item.Y);
						break;
					case nameof(ItemData.ItemType.PowerUp):
						map.PowerUps.Add(new PowerUp($"powerup_{powerupCounter}", new Point(item.X, item.Y)));
						powerupCounter++;
						break;
				}
			}
			map.UpdateTotalScore();
			Maps.Add(file.Split(".json")[0], map);
		}
	}
}
