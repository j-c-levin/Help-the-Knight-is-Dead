using UnityEngine;
using System.Collections;
using HKD_1;

namespace HKD_1
{
	public enum ResourceType
	{
		WOOD,
		SWORD
	}

	public class InteractableResourceStation : InteractableObject, IInteractable
	{
		public ResourceType stationResource;

		public void Interact (PlayerController player)
		{
			if (player.tapState == TapState.BUTTON_DOWN) {
				CompleteInteraction (player);
			}
		}

		private void CompleteInteraction (PlayerController player)
		{
			StartCoroutine (flash ());
			player.AddResource (stationResource);
		}
	}
}
