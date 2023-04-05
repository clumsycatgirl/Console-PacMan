using pac_man.ECS.Ghosts;
using pac_man.Generics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pac_man.ECS {
	internal class Player : Entity {
		public static readonly DrawData PlayerDrawData = new('O', ConsoleColor.Magenta);
		public int Score { get; set; }
		public bool PowerUp { get; set; } = false;
		private int powerUpCounterMax;
		private int powerUpCounter;
		public bool Living { get; set; } = true;
		public Player(string nameIdentifier, Point? position = null, DrawData? drawData = null) : base(nameIdentifier, position, drawData ?? PlayerDrawData) {
			Score = 0;
			powerUpCounter = 0;
			powerUpCounterMax = 45;
		}

		public override void Move(Vector movement) => Move(movement, null, null, null);
		public void Move(Vector movement, List<Block>? walls = null, List<Dot>? dots = null, List<PowerUp>? powerUps = null) {
			Point new_position = GetNewPosition(movement);
			if (walls is not null)
				foreach (Block wall in walls) {
					if (new_position.Equals(wall.Position)) {
						CheckPowerUp();
						return;
					}
				}
			Position = new_position;
			if (dots is not null) {
				List<Dot> dotsToRemove = new();
				foreach (Dot dot in dots) {
					if (Collide(dot)) {
						dotsToRemove.Add(dot);
						Score += Dot.Value;
					}
				}
				foreach (Dot dot in dotsToRemove)
					dots.Remove(dot);
			}
			List<Ghost> ghostsToRemove = new();
			foreach (Ghost ghost in GhostManager.Ghosts) {
				if (Collide(ghost)) {
					if (PowerUp)
						ghostsToRemove.Add(ghost);
					else
						Living = !Living;
				}
			}
			foreach (Ghost ghost in ghostsToRemove)
				GhostManager.Ghosts.Remove(ghost);

			if (powerUps is not null)
				foreach (PowerUp powerUp in powerUps) {
					if (Collide(powerUp)) {
						ActivatePowerUp();
						powerUps.Remove(powerUp);
						break;
					}
				}

			CheckPowerUp();
		}

		private void ActivatePowerUp() {
			PowerUp = true;
			powerUpCounter = 0;
			DrawData = new(DrawData.Sprite, Color.Green);
		}

		private void CheckPowerUp() {
			if (PowerUp) {
				Debug.WriteLine(powerUpCounter);
				powerUpCounter++;
				if (powerUpCounter == powerUpCounterMax) {
					PowerUp = false;
					powerUpCounter = 0;
					DrawData = new(DrawData.Sprite, Color.Magenta);
				}
			}
		}
	}
}
