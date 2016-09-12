using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace HKD_1
{
	public class PlayerController : MonoBehaviour
	{
		public int deviceID;

		public TapState tapState;

		private int m_resourceStorageLimit = 1;

		private float m_horizontalMovement;
		private float m_verticalMovement;
		private float m_movementFactor = 5;

		private bool m_action = false;
		private bool m_lastInputWasTrue = false;

		private Rigidbody2D m_rigidbody;

		private List<IInteractable> m_interactableList;
		private List<ResourceType> m_resourceStorage;

		void Start ()
		{
			m_rigidbody = GetComponent<Rigidbody2D> ();
			m_interactableList = new List<IInteractable> ();
			m_resourceStorage = new List<ResourceType> ();
		}

		void Update ()
		{
			UpdateMovement ();

			if (m_action) {
				UpdateAction ();
			}
		}

		private void UpdateMovement ()
		{
			Vector2 playerMovement = new Vector2 (m_horizontalMovement * m_movementFactor,
				                         m_verticalMovement * m_movementFactor);
			//m_rigidbody.AddForce (playerMovement, ForceMode2D.Impulse);
			m_rigidbody.velocity = playerMovement;
		}

		private void UpdateAction ()
		{
			for (int i = 0; i < m_interactableList.Count; i++) {
				m_interactableList [i].Interact (this);
			}
		}

		public void SetMovement (float horizontal, float vertical)
		{
			m_horizontalMovement = horizontal;
			m_verticalMovement = vertical;
		}

		public void SetAction (bool active)
		{
			//Assign the last input action
			m_lastInputWasTrue = m_action;

			//Set the player's tap state based on current and previous input
			tapState = DetermineInput (active);

			//Assign the current input for use in update()
			m_action = active;
		}

		//Not needed outside of testing
		public void SetDamage ()
		{
			for (int i = 0; i < m_interactableList.Count; i++) {
				m_interactableList [i].Damage (40);
			} 
		}

		public void AddResource (ResourceType resource)
		{
			//If the player's inventory is full, remove the oldest resource and add the new one
			if (m_resourceStorage.Count >= m_resourceStorageLimit) {
				m_resourceStorage.RemoveAt (0);
			}

			m_resourceStorage.Add (resource);
		}

		public bool UseResource (ResourceType resource)
		{
			return m_resourceStorage.Remove (resource);
		}

		public TapState DetermineInput (bool activeInput)
		{
			TapState response = TapState.NONE;

			//Button not pressed -> button not pressed
			if (!m_lastInputWasTrue && !activeInput) {
				response = TapState.BUTTON_UP;
			}

			//Button not pressed -> button pressed
			if (!m_lastInputWasTrue && activeInput) {
				response = TapState.BUTTON_DOWN;
			}

			//Button pressed -> button not pressed
			if (m_lastInputWasTrue && !activeInput) {
				response = TapState.BUTTON_UP;
			}

			//Button pressed -> button pressed
			if (m_lastInputWasTrue && activeInput) {
				response = TapState.BUTTON_PRESSED;
			}

			return response;
		}

		public void RemoveInteractable (IInteractable interactableObject)
		{
			m_interactableList.Remove (interactableObject);
		}

		void OnTriggerEnter2D (Collider2D collider)
		{
			IInteractable interactableObject = collider.GetComponent<IInteractable> ();

			if (interactableObject == null) {
				return;
			}

			m_interactableList.Add (interactableObject);
		}

		void OnTriggerExit2D (Collider2D collider)
		{
			IInteractable interactableObject = collider.GetComponent<IInteractable> ();

			if (interactableObject == null) {
				return;
			}

			m_interactableList.Remove (interactableObject);
		}
	}
}