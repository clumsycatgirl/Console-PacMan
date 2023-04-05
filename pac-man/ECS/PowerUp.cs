using pac_man.Generics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pac_man.ECS {
	internal class PowerUp : Entity {
		public static DrawData PowerUpDrawData = new('^', Color.Gray);
		public PowerUp(string nameIdentifier, Point? position = null, DrawData? drawData = null) : base(nameIdentifier, position, drawData ?? PowerUpDrawData) { }
	}
}
