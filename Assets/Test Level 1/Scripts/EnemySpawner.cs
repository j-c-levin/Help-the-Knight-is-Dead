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

		private float m_currentSpawnTime;
		private float m_downtimeDuration = 5f;
		private float m_downtimeRemaining;
		private float m_timeBetweenSets = 5f;
		private float m_currentTimeBetweenSets;
		private float m_timeBetweenUnits = 2.5f;

		private int m_currentSetNumber = 0;
		private int m_wavesCompleted = 0;

		List<EnemyController> m_activeEnemies;

		private List<Wave> m_waves;

		private WaveState m_waveState = WaveState.WAITING;

		private Wave m_currentWave;

		private Set m_currentSet;

		private struct Wave
		{
			public Set[] sets;
		}

		private struct Set
		{
			public int[] units;
		}

		private enum WaveState
		{
			NONE,
			WAITING,
			READY,
			SPAWNING,
			SET_IN_PROGRESS,
			DOWNTIME,
			END
		}

		// Use this for initialization
		void Start ()
		{
			m_spawners = GameObject.FindGameObjectsWithTag ("Enemy Spawner");

			SetUpWaves ();
		}

		void Update ()
		{
			WaveStateMachine ();
		}

		public void RemoveInteractable (IInteractable interactableObject)
		{
			m_activeEnemies.Remove ((EnemyController)interactableObject);
		}

		private void SetUpWaves ()
		{
			m_activeEnemies = new List<EnemyController> ();

			Set set1;
			Set set2;
			Set set3;
			Set set4;
			m_waves = new List<Wave> ();

			set1 = NewSet (new int[] { 2, 1 });
			set2 = NewSet (new int[] { 1, 3 });
			set3 = NewSet (new int[] { 2, 2 });
			Wave wave1 = NewWave (new Set[] { set1, set2, set3 });
			m_waves.Add (wave1);

			set1 = NewSet (new int[] { 3, 3 });
			set2 = NewSet (new int[] { 2, 4 });
			set3 = NewSet (new int[] { 2, 3 });
			Wave wave2 = NewWave (new Set[] { set1, set2, set3 });
			m_waves.Add (wave2);

			set1 = NewSet (new int[] { 1, 3 });
			set2 = NewSet (new int[] { 2, 3 });
			set3 = NewSet (new int[] { 4, 4 });
			set4 = NewSet (new int[] { 5, 4 });
			Wave wave3 = NewWave (new Set[] { set1, set2, set3, set4 });
			m_waves.Add (wave3);

		}

		private void WaveStateMachine ()
		{
			switch (m_waveState) {
			case WaveState.WAITING:
				Debug.Log ("waiting");
				LiftNextWave ();
				break;
			case WaveState.READY:
				Debug.Log ("ready");
				LiftNextSet ();
				break;
			case WaveState.SPAWNING:
				SpawnSet ();
				break;
			case WaveState.SET_IN_PROGRESS:
				SetInProgress ();
				break;
			case WaveState.DOWNTIME:
				Downtime ();
				break;
			case WaveState.END:
				Debug.Log ("End. No waves remain, players win");
				m_waveState = WaveState.NONE;
				break;
			case WaveState.NONE:
				//intenionally empty
				break;
			default:
				Debug.LogError ("Unknown case: " + m_waveState.ToString ());
				break;
			}
		}

		private void LiftNextWave ()
		{
			if (m_waves.Count == 0) {
				m_waveState = WaveState.END;
				return;
			}

			//Lift a wave off the array
			m_currentWave = m_waves [0];
			m_waves.RemoveAt (0);

			m_waveState = WaveState.READY;
		}


		private void LiftNextSet ()
		{
			if (m_currentSetNumber == m_currentWave.sets.Length) {
				Debug.Log ("downtime");
				m_waveState = WaveState.DOWNTIME;
				m_currentSetNumber = 0;
				return;
			}

			//Lift a set off the wave
			m_currentSet = m_currentWave.sets [m_currentSetNumber];
			m_currentSetNumber += 1;
			Debug.Log ("spawning");
			m_waveState = WaveState.SPAWNING;
			m_currentTimeBetweenSets = m_timeBetweenSets;
		}

		private void SpawnSet ()
		{
			m_currentTimeBetweenSets -= Time.deltaTime;

			if (m_currentTimeBetweenSets > 0) {
				return;
			}

			ShuffleSpawnerIndex ();

			int spawnGroupNumber = m_currentSet.units.Length;

			if (spawnGroupNumber > m_spawners.Length) {
				Debug.LogWarning ("More unit groups than spawners, capping.");
				spawnGroupNumber = m_spawners.Length;
			}

			//Take a set of units and set them to spawn
			for (int i = 0; i < spawnGroupNumber; i++) {
				StartCoroutine (SpawnUnitCount (m_currentSet.units [i],
					m_spawners [i].GetComponent<SpawnPoint> ()));
			}

			Debug.Log ("set in progress");
			m_waveState = WaveState.SET_IN_PROGRESS;
			m_currentTimeBetweenSets = m_timeBetweenSets;
			m_downtimeRemaining = m_downtimeDuration;
		}

		private void ShuffleSpawnerIndex ()
		{
			for (int i = 0; i < m_spawners.Length; i++) {
				int randomPosition = Random.Range (0, m_spawners.Length);
				GameObject movedA = m_spawners [randomPosition];
				GameObject movedB = m_spawners [i];
				m_spawners [i] = movedA;
				m_spawners [randomPosition] = movedB;
			}
		}

		private void SetInProgress ()
		{
			if (m_wavesCompleted != m_currentSet.units.Length) {
				return;
			}
				
			m_waveState = WaveState.READY;
			m_wavesCompleted = 0;
		}

		private IEnumerator SpawnUnitCount (int count, SpawnPoint spawner)
		{
			for (int i = 0; i < count; i++) {
				//spawn unit
				SpawnUnit (spawner);
				yield return new WaitForSeconds (m_timeBetweenUnits);
			}

			m_wavesCompleted += 1;
		}

		private void SpawnUnit (SpawnPoint spawnPoint)
		{
			EnemyController enemy = ((GameObject)GameObject.Instantiate (
				                        Enemy, 
				                        spawnPoint.transform.position, 
				                        spawnPoint.transform.rotation))
				.GetComponent<EnemyController> ();

			enemy.SetSpawnPoint (spawnPoint);
			enemy.SetGameManager (GetComponent<GameManager> ());
			m_activeEnemies.Add (enemy);
		}

		private void Downtime ()
		{
			if (m_activeEnemies.Count > 0) {
				return;
			}

			//wait a period of time then go to the next set, and then the next wave
			m_downtimeRemaining -= Time.deltaTime;

			if (m_downtimeRemaining <= 0) {
				m_waveState = WaveState.WAITING;
			}
		}

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
	}
}