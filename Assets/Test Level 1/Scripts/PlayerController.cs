using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace HKD_1
{
	public class PlayerController : MonoBehaviour
	{
		private int m_deviceID;
		private int m_resourceStorageLimit = 1;

		private float m_horizontalMovement;
		private float m_verticalMovement;

		private bool m_action;

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

			UpdateAction ();
		}

		private void UpdateMovement ()
		{
			Vector2 playerMovement = new Vector2 (m_horizontalMovement, m_verticalMovement);
			m_rigidbody.AddForce (playerMovement, ForceMode2D.Impulse);
		}

		private void UpdateAction ()
		{
			for (int i = 0; i < m_interactableList.Count; i++) {
				m_interactableList [i].Interact (this, m_action);
			} 
		}

		public void SetMovement (float horizontal, float vertical)
		{
			m_horizontalMovement = horizontal;
			m_verticalMovement = vertical;
		}

		public void SetAction (bool active)
		{
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
			if (m_resourceStorage.Count < m_resourceStorageLimit) {
				m_resourceStorage.Add (resource);
			} else {
				m_resourceStorage.Remove (0);
				m_resourceStorage.Add (resource);
			}
		}

		public bool UseResource (ResourceType resource)
		{
			return m_resourceStorage.Remove (resource);
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