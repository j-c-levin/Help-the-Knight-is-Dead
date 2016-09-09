using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using HKD_1;

public class GameManager : MonoBehaviour
{
	public GameObject playerSpawn;

	public bool localTesting;

	public GameObject PlayerPrefab;

	private Dictionary<int, PlayerController> m_players;

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
		}
	}

	public void OnPlayerConnected (int deviceID)
	{
		GameObject newPlayer = (GameObject)GameObject.Instantiate (PlayerPrefab, playerSpawn.transform.position, Quaternion.identity);
		m_players.Add (deviceID, newPlayer.GetComponent<PlayerController> ());
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
