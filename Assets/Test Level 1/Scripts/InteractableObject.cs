using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using HKD_1;

namespace HKD_1
{
	public class InteractableObject : MonoBehaviour
	{
		protected Text completionText;

		private Color m_myColour;

		protected int completionPercentage {
			get {
				return m_completionPercentage;
			}
			set {
				m_completionPercentage = value;

				if (m_completionPercentage >= 100) {
					m_completionPercentage = 100;
				}

				if (m_completionPercentage > 0) {
					m_isBlocking = true;
				}

				if (m_completionPercentage <= 0) {
					m_completionPercentage = 0;
					m_isBlocking = false;
				}
					
				if (completionText != null) {
					completionText.text = m_completionPercentage + "%";
				}
			}
		}

		protected bool m_isInteractable = true;

		private int m_completionPercentage = 0;

		private bool m_isBlocking = true;

		void Start ()
		{
			completionText = GetComponentInChildren<Text> ();
			completionPercentage = 100;

			if (GetComponent<SpriteRenderer> () != null) {
				m_myColour = GetComponent<SpriteRenderer> ().color;
			} else {
				m_myColour = gameObject.GetComponent<Renderer> ().material.color;
			}
		}

		public virtual bool IsBlocking ()
		{
			return m_isBlocking;
		}

		public virtual void Damage (int damage)
		{
			completionPercentage -= damage;
		}

		protected IEnumerator flash ()
		{
			if (GetComponent<SpriteRenderer> () != null) {
				GetComponent<SpriteRenderer> ().color = Color.green;
				yield return new WaitForSeconds (0.1f);
				GetComponent<SpriteRenderer> ().color = m_myColour;
			} else {
				gameObject.GetComponent<Renderer> ().material.color = Color.green;
				yield return new WaitForSeconds (0.1f);
				gameObject.GetComponent<Renderer> ().material.color = m_myColour;
			}
		}
	}
}