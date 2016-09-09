using UnityEngine;
using System.Collections;
using HKD_1;

public class InteractableResourceDrop : InteractableObject, IInteractable
{
	public ResourceType consumedResource;

	public void Interact (PlayerController player, bool activeInput)
	{
		if (DetermineInput (activeInput) == TapState.BUTTON_DOWN) {
			CompleteInteraction (player);
		}
	}

	private void CompleteInteraction (PlayerController player)
	{
		if (player.UseResource (consumedResource)) {
			completionPercentage += 30;
		}
	}
}
