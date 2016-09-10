using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using HKD_1;

public class GameManager : MonoBehaviour
{
	public GameObject playerSpawn;

	public int numberOfExpectedPlayers;

	public bool localTesting;

	public GameObject PlayerPrefab;

	private Dictionary<int, PlayerController> m_players;

	private EnemySpawner m_enemySpawner;

	float m_min = 0f;
	float m_max = 1f;

	private enum PlayerValues
	{
		DEVICE_ID = 0,
		HORIZONTAL_MOVEMENT = 1,
		VERTICAL_MOVEMENT = 2
	}
	 
	// Use this for initialization
	void Start ()
	{
		m_players = new Dictionary<int, PlayerController> ();

		if (localTesting) {
			Debug.LogWarning ("Using Local Testing");
			OnPlayerConnected (200);
		} else {
			GetComponent<EnemySpawner> ().enabled = false;
		}
	}

	public void OnPlayerConnected (int deviceID)
	{
		GameObject newPlayer = (GameObject)GameObject.Instantiate (PlayerPrefab, playerSpawn.transform.position, Quaternion.identity);
		newPlayer.GetComponent<SpriteRenderer> ().color = new Color (Random.Range (m_min, m_max), Random.Range (m_min, m_max), Random.Range (m_min, m_max));
		m_players.Add (deviceID, newPlayer.GetComponent<PlayerController> ());
		newPlayer.GetComponent<PlayerController> ().deviceID = deviceID;

		if (m_players.Count >= numberOfExpectedPlayers && !GetComponent<EnemySpawner> ().enabled) {
			GetComponent<EnemySpawner> ().enabled = true;
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (localTesting) {
			LocalTesting ();
		}
	}

	void LocalTesting ()
	{
		SetPlayerMovement (200, Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));

		if (Input.GetAxisRaw ("Jump") > 0) {
			SetPlayerAction (200, true);
		} else {
			SetPlayerAction (200, false);
		}

		if (Input.GetKeyDown ("b")) {
			PlayerController player = null;
			m_players.TryGetValue (200, out player);

			player.SetDamage ();
		}
	}

	public void SetPlayerMovement (int sender, float horizontalMovement, float verticalMovement)
	{
		PlayerController player = null;
		m_players.TryGetValue (sender, out player);

		if (player == null) {
			Debug.LogError ("id: " + sender + " not recognised");
			return;
		}

		player.SetMovement (horizontalMovement, verticalMovement);
	}

	public void SetPlayerAction (int sender, bool active)
	{
		PlayerController player = null;
		m_players.TryGetValue (sender, out player);

		if (player == null) {
			Debug.LogError ("id: " + sender + " not recognised");
			return;
		}

		player.SetAction (active);
	}
}
