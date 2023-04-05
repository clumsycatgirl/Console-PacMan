using pac_man.Generics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pac_man.ECS {
	internal abstract class Entity : IDrawable {
		public string NameIdentifier { get; }
		public Point Position { get; set; }
		public DrawData DrawData { get; set; }
		public Vector.Direction FacingDirection { get; set; }

		public Entity(string nameIdentifier, Point? position = null, DrawData? drawData = null) {
			NameIdentifier = nameIdentifier;
			Position = position ?? new Point(0, 0);
			DrawData = drawData ?? new DrawData('#', ConsoleColor.White);
		}

		public virtual void Draw() => GraphicsManager.Draw(this);
		public virtual void Move(Vector movement) => Position = GetNewPosition(movement);

		protected Point GetNewPosition(Vector movement) {
			FacingDirection = movement.Dir;
			switch (FacingDirection) {
				case Vector.Direction.Up:
					return new(Position.X, Math.Max(Position.Y - movement.Amount, 0));
				case Vector.Direction.Down:
					return new(Position.X, Math.Min(Position.Y + movement.Amount, GraphicsManager.Height - 1));
				case Vector.Direction.Left:
					return new(Math.Max(Position.X - movement.Amount, 0), Position.Y);
				case Vector.Direction.Right:
					return new(Math.Min(Position.X + movement.Amount, GraphicsManager.Width - 1), Position.Y);
			}
			return Position;
		}

		public virtual bool Collide(Entity other) => Position.Equals(other.Position);

		public override string ToString() => $"NameIdentifier:{NameIdentifier}, Position:{Position}, DrawData:{DrawData}";
	}
}
