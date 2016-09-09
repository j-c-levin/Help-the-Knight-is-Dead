using UnityEngine;
using System.Collections;
using HKD_1;

namespace HKD_1
{
	public class InteractableKing : InteractableObject, IInteractable
	{
		public void Interact (PlayerController player, bool active)
		{
			//Intentionally empty
		}

		public override void Damage (int damage)
		{
			completionPercentage -= damage;

			if (completionPercentage <= 0) {
				Debug.Log ("GAME OVER!!!!!");
				Time.timeScale = 0f;
			}
		}

		public override bool IsBlocking ()
		{
			return true;
		}
	}
}