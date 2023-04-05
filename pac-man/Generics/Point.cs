using pac_man.ECS;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static pac_man.Generics.Vector;

namespace pac_man.Generics {
	internal struct Point {
		public int X { get; set; }
		public int Y { get; set; }

		public Point(int x, int y) => (X, Y) = (x, y);

		public override string ToString() => $"X:{X}, Y:{Y}";
		public override bool Equals(object? obj) => obj is Point point && X == point.X && Y == point.Y;

		public override int GetHashCode() => base.GetHashCode();

		public static class PathUtils {
			public static List<Point> FindShortestPath(Point startPosition, Point endPosition, List<Point> walls) {
				return FindPath(ConstructGraph(walls), startPosition, endPosition);
			}

			private static readonly Point[] DIRECTIONS = new Point[] {
				new Point(0, 1),
				new Point(1, 0),
				new Point(0, -1),
				new Point(-1, 0),
			};

			public static List<Point> FindPath(List<Point> validPositions, Point startPosition, Point endPosition) {
				HashSet<Point> openSet = new HashSet<Point>();
				openSet.Add(startPosition);

				Dictionary<Point, Point> cameFrom = new Dictionary<Point, Point>();

				Dictionary<Point, int> gScore = new Dictionary<Point, int>();
				gScore[startPosition] = 0;

				Dictionary<Point, int> fScore = new Dictionary<Point, int>();
				fScore[startPosition] = GetHeuristic(startPosition, endPosition);

				while (openSet.Count > 0) {
					Point? current = null;
					int lowestFScore = int.MaxValue;
					foreach (Point cell in openSet) {
						if (fScore.ContainsKey(cell) && fScore[cell] < lowestFScore) {
							current = cell;
							lowestFScore = fScore[cell];
						}
					}

					if (current.Equals(endPosition)) {
						return ReconstructPath(cameFrom, current.Value);
					}

					openSet.Remove(current.Value);

					foreach (Point direction in DIRECTIONS) {
						Point neighbor = new Point(current.Value.X + direction.X, current.Value.Y + direction.Y);

						if (!validPositions.Contains(neighbor)) {
							continue;
						}

						int tentativeGScore = gScore[current.Value] + 1;

						if (gScore.ContainsKey(neighbor) && tentativeGScore >= gScore[neighbor]) {
							continue;
						}

						if (!openSet.Contains(neighbor)) {
							openSet.Add(neighbor);
						}

						cameFrom[neighbor] = current.Value;

						gScore[neighbor] = tentativeGScore;
						fScore[neighbor] = tentativeGScore + GetHeuristic(neighbor, endPosition);
					}
				}

				return null;
			}

			public static List<Point> ConstructGraph(List<Point> walls) {
				int minX = int.MaxValue;
				int minY = int.MaxValue;
				foreach (Point wall in walls) {
					if (wall.X < minX)
						minX = wall.X;
					if (wall.Y < minY)
						minY = wall.Y;
				}

				int width = walls.Max(wall => wall.X) - minX + 1;
				int height = walls.Max(wall => wall.Y) - minY + 1;
				bool[,] graph = new bool[width, height];
				foreach (Point wall in walls) {
					graph[wall.X - minX, wall.Y - minY] = true;
				}

				List<Point> positions = new List<Point>();
				bool[,] visited = new bool[width, height];
				for (int x = 0; x < width; x++) {
					for (int y = 0; y < height; y++) {
						if (!graph[x, y] && !visited[x, y]) {
							List<Point> connectedPositions = FloodFill(graph, visited, x, y);
							positions.AddRange(connectedPositions);
						}
					}
				}

				List<Point> shiftedPositions = new List<Point>();
				foreach (Point position in positions) {
					shiftedPositions.Add(new Point { X = position.X + minX, Y = position.Y + minY });
				}

				return shiftedPositions;
			}

			private static List<Point> FloodFill(bool[,] graph, bool[,] visited, int x, int y) {
				List<Point> positions = new List<Point>();
				if (x < 0 || x >= graph.GetLength(0) || y < 0 || y >= graph.GetLength(1) || graph[x, y] || visited[x, y]) {
					return positions;
				}
				visited[x, y] = true;
				positions.Add(new Point { X = x, Y = y });
				positions.AddRange(FloodFill(graph, visited, x - 1, y));
				positions.AddRange(FloodFill(graph, visited, x + 1, y));
				positions.AddRange(FloodFill(graph, visited, x, y - 1));
				positions.AddRange(FloodFill(graph, visited, x, y + 1));
				return positions;
			}

			public static List<Point> GetNeighbours(Point cell) {
				List<Point> neighbors = new List<Point>();

				if (cell.X > 0) {
					neighbors.Add(new Point(cell.X - 1, cell.Y));
				}
				if (cell.X < GraphicsManager.Width - 1) {
					neighbors.Add(new Point(cell.X + 1, cell.Y));
				}
				if (cell.Y > 0) {
					neighbors.Add(new Point(cell.X, cell.Y - 1));
				}
				if (cell.Y < GraphicsManager.Height - 1) {
					neighbors.Add(new Point(cell.X, cell.Y + 1));
				}

				return neighbors;
			}


			private static int GetHeuristic(Point currentPosition, Point endPosition) {
				int dx = currentPosition.X - endPosition.X;
				int dy = currentPosition.Y - endPosition.Y;
				return (int)Math.Sqrt(dx * dx + dy * dy);
			}

			private static List<Point> ReconstructPath(Dictionary<Point, Point> cameFrom, Point current) {
				List<Point> path = new List<Point>();
				path.Add(current);

				while (cameFrom.ContainsKey(current)) {
					current = cameFrom[current];
					path.Add(current);
				}

				path.Reverse();
				return path;
			}
		}
	}
}
