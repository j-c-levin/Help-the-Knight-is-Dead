using UnityEngine;
using System.Collections;
using HKD_1;

namespace HKD_1
{
	public class EnemySpawner : MonoBehaviour
	{
		public GameObject Enemy;

		private GameObject[] m_spawners;

		private float m_minSpawnTime = 1;
		private float m_maxSpawnTime = 4;
		private float m_currentSpawnTime;

		// Use this for initialization
		void Start ()
		{
			m_spawners = GameObject.FindGameObjectsWithTag ("Enemy Spawner");
			RandomiseSpawnTime ();
		}

		void Update ()
		{
			m_currentSpawnTime -= Time.deltaTime;

			if (m_currentSpawnTime <= 0) {
				SpawnEnemy ();
				RandomiseSpawnTime ();
			}
		}

		private void RandomiseSpawnTime ()
		{
			m_currentSpawnTime = Random.Range (m_minSpawnTime, m_maxSpawnTime);
		}

		private void SpawnEnemy ()
		{
			int spawnPointNumber = Random.Range (0, m_spawners.Length);

			GameObject currentSpawnPoint = m_spawners [spawnPointNumber];

			GameObject enemy = (GameObject)GameObject.Instantiate (Enemy, currentSpawnPoint.transform.position, Quaternion.identity);

			SpawnPoint sp = currentSpawnPoint.GetComponent<SpawnPoint> ();

			enemy.GetComponent<EnemyController> ().SetSpawnPoint (sp);
		}
	}
}