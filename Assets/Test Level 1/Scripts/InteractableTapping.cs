using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using HKD_1;

public class InteractableTapping : InteractableObject, IInteractable
{
	public void Interact (PlayerController player)
	{
		if (player.tapState == TapState.BUTTON_DOWN) {
			CompleteInteraction ();
		}
	}

	private void CompleteInteraction ()
	{
		completionPercentage += 5;

		StartCoroutine (flash ());
	}
}
