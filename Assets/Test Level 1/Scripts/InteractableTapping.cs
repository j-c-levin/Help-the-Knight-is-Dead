using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using HKD_1;

public class InteractableTapping : InteractableObject, IInteractable
{
	public void Interact (PlayerController player, bool activeInput)
	{
		if (DetermineInput (activeInput, player.deviceID) == TapState.BUTTON_DOWN) {
			CompleteInteraction ();
		}
	}

	private void CompleteInteraction ()
	{
		completionPercentage += 5;
	}
}
