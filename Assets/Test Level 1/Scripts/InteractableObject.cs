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

		private Dictionary<int,bool> m_lastInputWasTrue;

		private bool m_isBlocking = true;

		void Start ()
		{
			completionText = GetComponentInChildren<Text> ();
			m_lastInputWasTrue = new Dictionary<int, bool> ();
			completionPercentage = 100;
		}

		public TapState DetermineInput (bool activeInput, int deviceID)
		{
			TapState response = TapState.NONE;

			if (!m_lastInputWasTrue.ContainsKey (deviceID)) {
				m_lastInputWasTrue [deviceID] = false;
			}

			//Button not pressed -> button not pressed
			if (!m_lastInputWasTrue [deviceID] && !activeInput) {
				response = TapState.BUTTON_UP;
			}

			//Button not pressed -> button pressed
			if (!m_lastInputWasTrue [deviceID] && activeInput) {
				response = TapState.BUTTON_DOWN;
			}

			//Button pressed -> button not pressed
			if (m_lastInputWasTrue [deviceID] && !activeInput) {
				response = TapState.BUTTON_UP;
			}

			//Button pressed -> buton pressed
			if (m_lastInputWasTrue [deviceID] && activeInput) {
				response = TapState.BUTTON_PRESSED;
			}

			m_lastInputWasTrue [deviceID] = activeInput;

			return response;
		}

		public virtual bool IsBlocking ()
		{
			return m_isBlocking;
		}

		public virtual void Damage (int damage)
		{
			completionPercentage -= damage;
		}
	}
}