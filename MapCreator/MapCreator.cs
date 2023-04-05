using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace MapCreator {
	internal enum CellType {
		Wall,
		Dot,
		GhostSpawn,
		PlayerSpawn,
		PowerUp
	}
	internal record class Data(string Type, int X, int Y) {
		//public string Type { get; set; }
		//public int X { get; set; }
		//public int Y { get; set; }

	}
	internal static class MapCreator {
		private static ConsoleKeyInfo input;
		private static (int X, int Y) position = (0, 0);
		private static List<(int X, int Y)> walls = new();
		private static List<(int X, int Y)> dots = new();
		private static List<(int X, int Y)> ghostSpawn = new();
		private static List<(int X, int Y)> powerUps = new();
		private static (int X, int Y) playerSpawn = (0, 0);
		private static bool running => input.Modifiers != ConsoleModifiers.Shift && input.Key != ConsoleKey.Escape;
		private static bool emptyPosition => !walls.Contains(position) && !dots.Contains(position) && !ghostSpawn.Contains(position) && playerSpawn != position && !powerUps.Contains(position);

		static void Main(string[] args) {
			Console.CursorVisible = false;

			Thread input_thread = new(() => {
				while (running)
					input = Console.ReadKey(true);
			});

			input_thread.Start();
			while (running) {
				Console.SetCursorPosition(position.X, position.Y);

				if (input.KeyChar is Char key && key != '\0') {
					if (!running)
						break;
					if (new List<char>() { 'a', 'd', 'w', 's' }.Contains(key)) {
						(int X, int Y) = key switch {
							'a' => (-1, 0),
							'd' => (+1, 0),
							'w' => (0, -1),
							's' => (0, +1),
							_ => (0, 0),
						};
						if (walls.Contains(position))
							DrawWall();
						else if (dots.Contains(position))
							DrawDot();
						else if (ghostSpawn.Contains(position)) {
							DrawGhostSpawn();
						} else if (position == playerSpawn) {
							DrawPlayerSpawn();
						} else if (powerUps.Contains(position)) {
							DrawPowerUp();
						} else {
							Console.SetCursorPosition(position.X, position.Y);
							Console.Write(" ");
						}

						position = (Math.Max(Math.Min(Console.BufferWidth - 1, position.X + X), 0),
									Math.Max(Math.Min(Console.BufferHeight - 1, position.Y + Y), 0));
						DrawUser();
					} else if (key == 'l') {
						if (emptyPosition) {
							walls.Add(position);
							DrawWall();
						} else if (walls.Contains(position))
							walls.Remove(position);
					} else if (key == 'o') {
						if (emptyPosition) {
							dots.Add(position);
							DrawDot();
						} else if (dots.Contains(position))
							dots.Remove(position);
					} else if (key == 'p') {
						if (emptyPosition) {
							ghostSpawn.Add(position);
						} else if (ghostSpawn.Contains(position)) {
							ghostSpawn.Remove(position);
						}
					} else if (key == 'j' && emptyPosition) {
						Console.SetCursorPosition(playerSpawn.X, playerSpawn.Y);
						Console.Write(' ');
						playerSpawn = position;
						DrawPlayerSpawn();
					} else if (key == 'k') {
						if (emptyPosition) {
							powerUps.Add(position);
							DrawPowerUp();
						} else if (powerUps.Contains(position)) {
							powerUps.Remove(position);
						}
					}

					input = new ConsoleKeyInfo('\0', 0, false, false, false);
				}
			}
			Console.SetCursorPosition(Console.BufferWidth - 1, Console.BufferHeight - 1);
			Console.Write("\n");
			Console.Write("input map name: ");
			string mapName = Console.ReadLine() ?? "new_map";
			List<Data> data = new();

			foreach ((int X, int Y) position in walls) {
				data.Add(new Data(
					nameof(CellType.Wall),
					position.X,
					position.Y
				));
			}
			foreach ((int X, int Y) position in dots) {
				data.Add(new Data(
					nameof(CellType.Dot),
					position.X,
					position.Y
				));
			}
			foreach ((int X, int Y) position in ghostSpawn) {
				data.Add(new Data(
					nameof(CellType.GhostSpawn),
					position.X,
					position.Y
				));
			}
			foreach ((int X, int Y) position in powerUps) {
				data.Add(new Data(
					nameof(CellType.PowerUp),
					position.X,
					position.Y
				));
			}
			data.Add(new Data(
				nameof(CellType.PlayerSpawn),
				playerSpawn.X,
				playerSpawn.Y
			));

			string json = JsonSerializer.Serialize(data);
			File.WriteAllText($"{mapName}.json", json);
		}

		private static void Draw(char sprite, ConsoleColor color) {
			Console.SetCursorPosition(position.X, position.Y);
			Console.ForegroundColor = color;
			Console.Write(sprite);
			Console.ResetColor();
		}

		private static void DrawUser() => Draw('_', ConsoleColor.Red);
		private static void DrawWall() => Draw('+', ConsoleColor.Blue);
		private static void DrawDot() => Draw('*', ConsoleColor.Yellow);
		private static void DrawGhostSpawn() => Draw('G', ConsoleColor.Green);
		private static void DrawPlayerSpawn() => Draw('S', ConsoleColor.Magenta);
		private static void DrawPowerUp() => Draw('P', ConsoleColor.Gray);
	}
}