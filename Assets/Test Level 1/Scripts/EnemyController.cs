using UnityEngine;
using System.Collections;
using HKD_1;

namespace HKD_1
{
	public class EnemyController : InteractableObject, IInteractable
	{
		public ResourceType consumedResource;

		private SpawnPoint m_spawnPoint;

		private int currentNodeID = 0;

		private bool m_continue = true;

		private float m_damageTickTimer = 1.5f;
		private float m_movementSpeed = 0.02f;

		private IInteractable curentObstacle {
			get {
				return m_spawnPoint.obstacles [currentNodeID].GetComponent<IInteractable> ();
			}
		}

		private GameObject currentNode {
			get {
				return m_spawnPoint.nodes [currentNodeID];
			}
		}

		void Update ()
		{
			transform.position = Vector2.MoveTowards (transform.position, currentNode.transform.position, m_movementSpeed);

			float me = transform.position.x;
			float dest = currentNode.transform.position.x;

			if (Mathf.Approximately (me, dest) && m_continue) {
				m_continue = false;

				if (!ShouldMoveToNextTarget ()) {
					StartCoroutine (DamageObject ());
				}
			}
		}

		public void SetSpawnPoint (SpawnPoint sp)
		{
			m_spawnPoint = sp;
		}

		private IEnumerator DamageObject ()
		{
			curentObstacle.Damage (40);

			yield return new WaitForSeconds (m_damageTickTimer);

			if (!ShouldMoveToNextTarget ()) {
				StartCoroutine (DamageObject ());
			}
		}

		private bool ShouldMoveToNextTarget ()
		{
			//Deal 10 damage to the king and die
			if (currentNodeID == m_spawnPoint.nodes.Length - 1) {
				curentObstacle.Damage (10);
				Destroy (gameObject);
				return true;
			}

			//False if it's still active, true if it isn't
			if (curentObstacle.IsBlocking ()) {
				return false;
			} else {
				currentNodeID += 1;
				m_continue = true;
				return true;
			}
		}

		public void Interact (PlayerController player, bool activeInput)
		{
			if (DetermineInput (activeInput) == TapState.BUTTON_DOWN) {
				CompleteInteraction (player);
			}
		}

		private void CompleteInteraction (PlayerController player)
		{
			if (player.UseResource (consumedResource)) {
				player.RemoveInteractable (this);
				Destroy (gameObject);
			}
		}
	}
}