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
		private bool m_destroyAfterHittingKing = false;

		private float m_damageTickTimer = 1.5f;
		private float m_movementSpeed = 0.01f;

		private int m_damageToObstacles = 20;

		private GameManager m_gameManager;

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

			if (m_continue && Mathf.Approximately (me, dest)) {
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

		public void SetGameManager (GameManager gm)
		{
			m_gameManager = gm;
		}

		private IEnumerator DamageObject ()
		{
			yield return new WaitForSeconds (m_damageTickTimer);

			curentObstacle.Damage (m_damageToObstacles);

			if (m_destroyAfterHittingKing) {
				Destroy (gameObject);
			}

			if (!ShouldMoveToNextTarget ()) {
				StartCoroutine (DamageObject ());
			}
		}

		private bool ShouldMoveToNextTarget ()
		{
			//Deal 10 damage to the king and die
			if (currentNodeID == m_spawnPoint.nodes.Length - 1) {
				m_damageToObstacles = 6;
				m_destroyAfterHittingKing = true;
				return false;
			}

			//False if it's still active and needs attacking, true if it isn't and can proceed
			if (curentObstacle.IsBlocking ()) {
				return false;
			} else {
				currentNodeID += 1;
				m_continue = true;
				return true;
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
			if (player.UseResource (consumedResource)) {
				m_gameManager.EnemyDefeated (this);
				Destroy (gameObject);
			}
		}
	}
}