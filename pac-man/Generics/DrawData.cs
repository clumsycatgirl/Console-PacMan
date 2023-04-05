using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pac_man.Generics {
	internal readonly record struct DrawData {
		public readonly char Sprite { get; init; }
		public readonly Color Color { get; init; }

		public DrawData(char sprite, Color color = Color.White) => (Sprite, Color) = (sprite, color);

		public override string ToString() => $"Sprite:{Sprite}, Color:{Color}";
	}
}
