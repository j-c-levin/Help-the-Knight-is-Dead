using UnityEngine;
using System.Collections;
using System.Collections.Generic;
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

		List<Wave> waves;

		private struct Wave
		{
			public Set[] sets;
		}

		private struct Set
		{
			public int[] units;
		}

		// Use this for initialization
		void Start ()
		{
			m_spawners = GameObject.FindGameObjectsWithTag ("Enemy Spawner");
			RandomiseSpawnTime ();

			//SetUpWaves ();
		}

		/*private void SetUpWaves ()
		{
			Set set1;
			Set set2;
			Set set3;
			Set set4;
			Set set5;

			set1 = NewSet (2, 3);
			set2 = NewSet (1, 2);
			set3 = NewSet (2, 2);
			Wave wave1 = NewWave (set1, set2, set3);
			waves.Add (wave1);

			set1 = NewSet (3, 3);
			set2 = NewSet (2, 4);
			set3 = NewSet (2, 3);
			Wave wave2 = NewWave (set1, set2, set3);

			set1 = NewSet (1, 3);
			set2 = NewSet (2, 3);
			set3 = NewSet (4, 4);
			set4 = NewSet (5, 4);
			Wave wave3 = NewWave (set1, set2, set3);

			waves.Add (wave1);
			waves.Add (wave2);
			waves.Add (wave3);
		}*/

		private Wave NewWave (Set[] sets)
		{
			Wave response = new Wave ();
			response.sets = sets;

			return response;
		}

		private Set NewSet (int[] unitComposition)
		{
			Set response = new Set ();
			response.units = unitComposition;

			return response;
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

			GameObject enemy = (GameObject)GameObject.Instantiate (Enemy, currentSpawnPoint.transform.position, currentSpawnPoint.transform.rotation);

			SpawnPoint sp = currentSpawnPoint.GetComponent<SpawnPoint> ();

			enemy.GetComponent<EnemyController> ().SetSpawnPoint (sp);
			enemy.GetComponent<EnemyController> ().SetGameManager (GetComponent<GameManager> ());
		}
	}
}