using UnityEngine;
using System.Collections;
using HKD_1;

public class InteractableHolding : InteractableObject, IInteractable
{
	private float m_interactionPercentage = 0;
	private float m_interactionFactor = 30;

	public void Interact (PlayerController player)
	{
		if (player.tapState == TapState.BUTTON_PRESSED && completionPercentage < 100) {
			CompleteInteraction ();
		}
	}

	private void CompleteInteraction ()
	{
		m_interactionPercentage += Time.deltaTime * m_interactionFactor;
		if (m_interactionPercentage >= 1) {
			completionPercentage += 1;
		}

		m_interactionPercentage %= 1;

		if (completionPercentage % 10 == 0) {
			StartCoroutine (flash ());
		}
	}
}
