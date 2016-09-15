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

	public Color[] playerColours;

	private Dictionary<int, PlayerController> m_players;

	private EnemySpawner m_enemySpawner;
	 
	// Use this for initialization
	void Start ()
	{
		m_players = new Dictionary<int, PlayerController> ();

		m_enemySpawner = GetComponent<EnemySpawner> ();

		if (localTesting) {
			Debug.LogWarning ("Using Local Testing");
			OnPlayerConnected (200);
		} else {
			m_enemySpawner.enabled = false;
		}
	}

	public void OnPlayerConnected (int deviceID)
	{
		//Spawn a new player
		GameObject newPlayer = (GameObject)GameObject.Instantiate (PlayerPrefab, playerSpawn.transform.position, Quaternion.identity);

		//Give a color from the array
		newPlayer.GetComponent<SpriteRenderer> ().color = playerColours [m_players.Count];
			
		//Add it to the players list
		m_players.Add (deviceID, newPlayer.GetComponent<PlayerController> ());

		//Assign the player it's device ID
		newPlayer.GetComponent<PlayerController> ().deviceID = deviceID;

		//For testing, only start the game when enough players have been connected
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

	//For keyboard movement and actions
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

	public void RemoveInteractable (IInteractable interactableObject)
	{
		foreach (PlayerController player in m_players.Values) {
			player.RemoveInteractable (interactableObject);
		}

		m_enemySpawner.RemoveInteractable (interactableObject);
	}
}
