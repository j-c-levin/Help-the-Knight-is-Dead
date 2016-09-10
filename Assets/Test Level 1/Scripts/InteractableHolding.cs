using UnityEngine;
using System.Collections;
using HKD_1;

public class InteractableHolding : InteractableObject, IInteractable
{
	public void Interact (PlayerController player, bool activeInput)
	{
		if (DetermineInput (activeInput, player.deviceID) == TapState.BUTTON_PRESSED) {
			CompleteInteraction ();
		}
	}

	private void CompleteInteraction ()
	{
		//Will absolutely need to change how this is handled because right now
		//It's biased towards computers that can pump out higher fps
		completionPercentage += 1;
	}
}
