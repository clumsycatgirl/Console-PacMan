global using Color = System.ConsoleColor;
using pac_man.ECS;
using pac_man.Generics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace pac_man {
	internal static class GraphicsManager {
		public static int Width => Console.BufferWidth;
		public static int Height => Console.BufferHeight;

		public static void Draw(IDrawable drawable) {
			Console.SetCursorPosition(drawable.Position.X, drawable.Position.Y);
			Console.ForegroundColor = drawable.DrawData.Color;
			Console.Write(drawable.DrawData.Sprite);
			Console.ResetColor();
		}

		public static void ClearPosition(Point position) =>
			Draw(new Player("dummy", position: position, drawData: new DrawData(' ', Color.Black)));
	}
}
