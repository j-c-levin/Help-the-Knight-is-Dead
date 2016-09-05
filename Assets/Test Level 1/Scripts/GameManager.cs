#define local_testing

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
	public GameObject PlayerPrefab;

	private List<Rigidbody2D> m_players;
	private List<float[]> m_playerValues;

	private enum PlayerValues
	{
		DEVICE_ID = 0,
		HORIZONTAL_MOVEMENT = 1,
		VERTICAL_MOVEMENT = 2
	}

	// Use this for initialization
	void Start ()
	{
		m_players = new List<Rigidbody2D> ();
		m_playerValues = new List<float[]> ();

		#if local_testing
		OnPlayerConnected (200);
		#endif
	}

	public void OnPlayerConnected (int deviceID)
	{
		GameObject newPlayer = (GameObject)GameObject.Instantiate (PlayerPrefab, Vector3.zero, Quaternion.identity);
		m_players.Add (newPlayer.GetComponent<Rigidbody2D> ());
		m_playerValues.Add (new float[] { deviceID, 0.0f, 0.0f }); 
	}
	
	// Update is called once per frame
	void Update ()
	{
		#if local_testing

		#endif
		UpdatePlayerMovement ();

	}

	void UpdatePlayerMovement ()
	{
		#if local_testing
		m_playerValues [0] [(int)PlayerValues.HORIZONTAL_MOVEMENT] = Input.GetAxisRaw ("Horizontal");
		m_playerValues [0] [(int)PlayerValues.VERTICAL_MOVEMENT] = Input.GetAxisRaw ("Vertical");
		#endif

		for (int i = 0; i < m_players.Count; i++) {
			Vector2 playerMovement = new Vector2 (m_playerValues [i] [(int)PlayerValues.HORIZONTAL_MOVEMENT], m_playerValues [i] [(int)PlayerValues.VERTICAL_MOVEMENT]);
			m_players [i].AddForce (playerMovement, ForceMode2D.Impulse);
		}
	}

	void LocalTesting() {

	}

	public void SetPlayerMovement (int sender, float horizontalMovement, float verticalMovement)
	{
		for (int i = 0; i < m_players.Count; i++) {
			if (m_playerValues [i] [(int)PlayerValues.DEVICE_ID] == sender) {
				m_playerValues [i] [(int)PlayerValues.HORIZONTAL_MOVEMENT] = horizontalMovement;
				m_playerValues [i] [(int)PlayerValues.VERTICAL_MOVEMENT] = verticalMovement;
				return;
			}
		}
	}

	public void SetPlayerAction (int sender, bool pressed)
	{
		for (int i = 0; i < m_players.Count; i++) {
				//Handle action here
				return;
			}
		}
	}
}
