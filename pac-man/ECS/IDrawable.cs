using pac_man.Generics;

namespace pac_man.ECS {
	internal interface IDrawable {
		public Point Position { get; }
		public DrawData DrawData { get; }
		public void Draw();
	}
}