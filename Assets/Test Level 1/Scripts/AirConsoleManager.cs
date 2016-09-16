using UnityEngine;
using System.Collections;
using NDream.AirConsole;
using Newtonsoft.Json.Linq;

public class AirConsoleManager : MonoBehaviour
{
	private GameManager m_gameManager;

	void Awake ()
	{
		if (AirConsole.instance != null) {
			SetupAirConsoleEvents ();
		}
	}

	void Start ()
	{
		m_gameManager = GetComponent<GameManager> ();
	}

	void SetupAirConsoleEvents ()
	{
		AirConsole.instance.onReady += OnReady;
		AirConsole.instance.onMessage += OnMessage;
		AirConsole.instance.onConnect += OnConnect;
		AirConsole.instance.onDisconnect += OnDisconnect;
		AirConsole.instance.onDeviceStateChange += OnDeviceStateChange;
		AirConsole.instance.onCustomDeviceStateChange += OnCustomDeviceStateChange;
		AirConsole.instance.onDeviceProfileChange += OnDeviceProfileChange;
		AirConsole.instance.onAdShow += OnAdShow;
		AirConsole.instance.onAdComplete += OnAdComplete;
		AirConsole.instance.onGameEnd += OnGameEnd;
		Debug.Log ("Connecting...");
	}

	void OnReady (string code)
	{

	}

	void OnMessage (int from, JToken data)
	{ 
//		Debug.Log ("time: " + (AirConsole.instance.GetServerTime () - (long)data ["time"]));

		if ((object)data ["joystick-left"] != null) {
			ReceivedMovement (from, data);
			return;
		}

		if ((object)data ["action"] != null) {
			ReceivedAction (from, data);
			return;
		}

		Debug.LogError ("unknown input: " + data.ToString ());
	}

	void ReceivedMovement (int from, JToken data)
	{
		if (!(bool)data ["joystick-left"] ["pressed"]) { 
			m_gameManager.SetPlayerMovement (from, 0, 0);
			return;
		}

		//Update player movement
		m_gameManager.SetPlayerMovement (from, (float)data ["joystick-left"] ["message"] ["x"], -(float)data ["joystick-left"] ["message"] ["y"]);
	}

	void ReceivedAction (int from, JToken data)
	{
		m_gameManager.SetPlayerAction (from, (bool)data ["action"] ["pressed"]);
	}

	void OnConnect (int device_id)
	{
		Debug.Log ("New device connected: " + device_id);
		m_gameManager.OnPlayerConnected (device_id);
	}

	void OnDisconnect (int device_id)
	{
		Debug.LogError ("Device disconnected and this is not handled");
	}

	void OnDeviceStateChange (int device_id, JToken data)
	{

	}

	void OnCustomDeviceStateChange (int device_id, JToken custom_data)
	{

	}

	void OnDeviceProfileChange (int device_id)
	{

	}

	void OnAdShow ()
	{

	}

	void OnAdComplete (bool adWasShown)
	{

	}

	void OnGameEnd ()
	{
		Debug.Log ("OnGameEnd is called");
	}

	void OnDestroy ()
	{
		// unregister events
		if (AirConsole.instance != null) {
			AirConsole.instance.onReady -= OnReady;
			AirConsole.instance.onMessage -= OnMessage;
			AirConsole.instance.onConnect -= OnConnect;
			AirConsole.instance.onDisconnect -= OnDisconnect;
			AirConsole.instance.onDeviceStateChange -= OnDeviceStateChange;
			AirConsole.instance.onCustomDeviceStateChange -= OnCustomDeviceStateChange;
			AirConsole.instance.onAdShow -= OnAdShow;
			AirConsole.instance.onAdComplete -= OnAdComplete;
			AirConsole.instance.onGameEnd -= OnGameEnd;
		}
	}
}
