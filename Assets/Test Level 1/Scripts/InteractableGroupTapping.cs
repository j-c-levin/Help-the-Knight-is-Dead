using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using HKD_1;

namespace HKD_1
{
	public class InteractableGroupTapping : InteractableObject, IInteractable
	{
		private enum ActionState
		{
			WAITING,
			ONE_INTERACTION
		}

		private ActionState m_actionState = ActionState.WAITING;

		private static float m_groupActionWindow = 0.5f;
		private float m_currentGroupActionTime = m_groupActionWindow;

		private PlayerController m_interactedPlayer;

		void Update ()
		{
			if (m_actionState == ActionState.ONE_INTERACTION) {
				m_currentGroupActionTime -= Time.deltaTime;

				if (m_currentGroupActionTime <= 0) {
					Debug.Log ("group action timeout, resetting");
					//Actions not performed together, reset
					ResetActionTimer ();
				}
			}
		}

		public void Interact (PlayerController player)
		{
			if (player.tapState == TapState.BUTTON_DOWN) {
				CompleteInteraction (player);
			}
		}

		private void CompleteInteraction (PlayerController player)
		{
			if (m_actionState == ActionState.WAITING) {
				//This is the first player to interact
				//Begin the timeout for the second player
				m_actionState = ActionState.ONE_INTERACTION;
				m_interactedPlayer = player;
			} else {
				//Check that this is the second person to interact
				if (player == m_interactedPlayer) {
					return;
				}

				//This is the second player to interact
				//Complete the interaction and reset
				completionPercentage += 10;

				ResetActionTimer ();

				StartCoroutine (flash ());
			}
		}

		private void ResetActionTimer ()
		{
			m_actionState = ActionState.WAITING;
			m_currentGroupActionTime = m_groupActionWindow;
			m_interactedPlayer = null;
		}
	}
}