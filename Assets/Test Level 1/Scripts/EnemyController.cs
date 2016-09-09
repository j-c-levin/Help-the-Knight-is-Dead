using UnityEngine;
using System.Collections;
using HKD_1;

namespace HKD_1
{
	public class EnemyController : MonoBehaviour
	{
		private SpawnPoint m_spawnPoint;

		private int currentNode = 0;

		void Update ()
		{
			transform.position = Vector2.MoveTowards (transform.position, m_spawnPoint.nodes [currentNode].transform.position, 0.1f);

			float me = transform.position.x;
			float dest = m_spawnPoint.nodes [currentNode].transform.position.x;
			if (Mathf.Approximately (me, dest)) {
				if (currentNode < m_spawnPoint.nodes.Length - 1) {
					currentNode += 1;
				} else {
					Destroy (gameObject);
				}
			}
		}

		public void SetSpawnPoint (SpawnPoint sp)
		{
			m_spawnPoint = sp;
		}
	}
}