using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using HKD_1;

namespace HKD_1
{
	public class InteractableObject : MonoBehaviour
	{
		protected Text completionText;

		protected int completionPercentage {
			get {
				return m_completionPercentage;
			}
			set {
				m_completionPercentage = value;

				if (m_completionPercentage >= 100) {
					m_completionPercentage = 100;
					m_isBlocking = true;
				}

				if (m_completionPercentage <= 0) {
					m_completionPercentage = 0;
					m_isBlocking = false;
				}
					
				completionText.text = m_completionPercentage + "%";
			}
		}

		protected bool m_isInteractable = true;

		private int m_completionPercentage = 0;

		private bool m_lastInputWasTrue = false;

		private bool m_isBlocking = true;

		void Start ()
		{
			completionText = GetComponentInChildren<Text> ();
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

			//Button pressed -> buton pressed
			if (m_lastInputWasTrue && activeInput) {
				response = TapState.BUTTON_PRESSED;
			}

			m_lastInputWasTrue = activeInput;
			return response;
		}

		public bool IsBlocking ()
		{
			return m_isBlocking;
		}

		public void Damage (int damage)
		{
			completionPercentage -= damage;
		}
	}
}