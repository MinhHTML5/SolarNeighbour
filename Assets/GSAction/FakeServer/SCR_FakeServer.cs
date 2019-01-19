using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public class SCR_FakeServer : MonoBehaviour {
	public static bool 				useFakeServer;
	public static SCR_FakeServer 	instance;
	
	public GameObject		PFB_FakeAI;
	
	private GameState 		gameState;
	private GameObject[]	fakeAIObject;
	private SCR_FakeAI[]	fakeAI;
	private FakePlanet[]	planet;
	
	private byte[]			broadcastPacket;
	private byte[]			privatePacket_1;
	private byte[]			privatePacket_2;
	private byte[]			privatePacket_3;
	private byte[]			privatePacket_4;
	private float			pickTimeOut;
	private bool[]			playerReady					= new bool [4];
	
	
	
	private void Awake() {
		// Whenever this script is enabled, switch to fake mode
		useFakeServer = true;
	}
	
    private void Start() {
		// Block run if GS_Action is start first
		if (SCR_Loading.firstTimeRun) {
			return;
		}
		
		// Import setting
		pickTimeOut = SCR_Config.PICK_PLANET_TIME + 1;
		
		// Init everything
		instance = this;
        gameState = GameState.INIT;
		for (int i=0; i<4; i++) {
			playerReady[i] = false;
		}
		broadcastPacket = new byte[0];
		privatePacket_1 = new byte[0];
		privatePacket_2 = new byte[0];
		privatePacket_3 = new byte[0];
		privatePacket_4 = new byte[0];
		
		
		
		// Create 3 fake AI, who will send message like a real human opponent
		// Their intelligence is doubtly good.
		fakeAI = new SCR_FakeAI[3];
		fakeAIObject = new GameObject[3];
		for (int i=0; i<3; i++) {
			fakeAIObject[i] = Instantiate (PFB_FakeAI);
			fakeAIObject[i].transform.SetParent (transform);
			fakeAI[i] = fakeAIObject[i].GetComponent<SCR_FakeAI>();
			fakeAI[i].Init (i + 1);
		}
    }
	
	private void AppendBroadcastCommand (byte[] data) {
		// Add message to the broadcast buffer
		// This buffer will be broadcasted at the end of a loop
		int originalLength = broadcastPacket.Length;
		System.Array.Resize<byte>(ref broadcastPacket, broadcastPacket.Length + data.Length);
		System.Array.Copy(data, 0, broadcastPacket, originalLength, data.Length);
	}
	
	private void AppendPrivateMessage (byte[] data, int playerID) {
		// PM a player. Send at the end of a loop also
		// This chunk of code is a little stupid, should have used a 2 dimension array, but fuck it
		if (playerID == 0) {
			int originalLength = privatePacket_1.Length;
			System.Array.Resize<byte>(ref privatePacket_1, privatePacket_1.Length + data.Length);
			System.Array.Copy(data, 0, privatePacket_1, originalLength, data.Length);
		}
		else if (playerID == 1) {
			int originalLength = privatePacket_2.Length;
			System.Array.Resize<byte>(ref privatePacket_2, privatePacket_2.Length + data.Length);
			System.Array.Copy(data, 0, privatePacket_2, originalLength, data.Length);
		}
		else if (playerID == 2) {
			int originalLength = privatePacket_3.Length;
			System.Array.Resize<byte>(ref privatePacket_3, privatePacket_3.Length + data.Length);
			System.Array.Copy(data, 0, privatePacket_3, originalLength, data.Length);
		}
		else if (playerID == 3) {
			int originalLength = privatePacket_4.Length;
			System.Array.Resize<byte>(ref privatePacket_4, privatePacket_4.Length + data.Length);
			System.Array.Copy(data, 0, privatePacket_4, originalLength, data.Length);
		}
	}






    private void FixedUpdate() {
		// Block run if GS_Action is start first
		if (SCR_Loading.firstTimeRun) {
			return;
		}



		
		float dt = Time.fixedDeltaTime;
		if (gameState == GameState.INIT) {
			// Wait for ready command from client
		}
		else if (gameState == GameState.CHOOSE_PLANET) {
			pickTimeOut -= dt;
			if (pickTimeOut <= 0) {
				// Find out who haven't picked
				List<int> unpick = new List<int>();
				unpick.Add(0); unpick.Add(1); unpick.Add(2); unpick.Add(3);
				for (int i=0; i<planet.Length; i++) {
					for (int j=0; j<unpick.Count; j++) {
						if (planet[i].playerID == unpick[j]) {
							unpick.RemoveAt(j);
							break;
						}
					}
				}
				
				for (int i=0; i<unpick.Count; i++) {
					// Pick randomly for those dudes
					bool ok = false;
					while (!ok) {
						int choose = Random.Range (0, planet.Length);
						if (planet[choose].playerID == -1) {
							planet[choose].playerID = unpick[i];
							AppendBroadcastCommand (System.BitConverter.GetBytes((int)Command.SERVER_PROVIDE_PLANET));
							AppendBroadcastCommand (System.BitConverter.GetBytes(unpick[i]));
							AppendBroadcastCommand (System.BitConverter.GetBytes(choose));
							unpick.RemoveAt(i);
							ok = true;
						}
					}
				}
				
				gameState = GameState.ACTION;
				AppendBroadcastCommand (System.BitConverter.GetBytes((int)Command.SERVER_START_GAME));
			}
		}
		else if (gameState == GameState.ACTION) {
			// Update planets movement
			for (int i=0; i<planet.Length; i++) {
				planet[i].FixedUpdate (dt);
			}
			// Propagate planet info
			AppendBroadcastCommand (System.BitConverter.GetBytes((int)Command.SERVER_UPDATE_PLANET));
			for (int i=0; i<planet.Length; i++) {
				AppendBroadcastCommand (System.BitConverter.GetBytes(planet[i].angle));
			}
		}
		
		// Send broadcast packet
        if (broadcastPacket.Length > 0) {
			SCR_Client.instance.OnDataReceive (broadcastPacket);
			fakeAI[0].OnDataReceive (broadcastPacket);
			fakeAI[1].OnDataReceive (broadcastPacket);
			fakeAI[2].OnDataReceive (broadcastPacket);
			broadcastPacket = new byte[0];
		}
		
		// Send PM packet
		if (privatePacket_1.Length > 0) {
			SCR_Client.instance.OnDataReceive (privatePacket_1);
			privatePacket_1 = new byte[0];
		}
		if (privatePacket_2.Length > 0) {
			fakeAI[0].OnDataReceive (privatePacket_2);
			privatePacket_2 = new byte[0];
		}
		if (privatePacket_3.Length > 0) {
			fakeAI[1].OnDataReceive (privatePacket_3);
			privatePacket_3 = new byte[0];
		}
		if (privatePacket_4.Length > 0) {
			fakeAI[2].OnDataReceive (privatePacket_4);
			privatePacket_4 = new byte[0];
		}
    }
	
	
	
	
	
	
	
	
	
	
	
	
	public void OnDataReceive(byte[] data, int playerID) {
		// playerID in a real server will get from socket index in socket array
		
		
		int readIndex = 0;
		int commandID = 0;
		
		while (readIndex < data.Length) {
			commandID = System.BitConverter.ToInt32(data, readIndex);
			if (commandID == (int)Command.CLIENT_READY) {
				if (gameState == GameState.INIT) {
					// Player send ready command, mark them as ready
					playerReady[playerID] = true;
					
					// Send them their playerID so they know who they are
					AppendPrivateMessage (System.BitConverter.GetBytes((int)Command.SERVER_PROVIDE_ID), playerID);
					AppendPrivateMessage (System.BitConverter.GetBytes(playerID), playerID);
					
					// If all 4 player are ready, we create the planets and start the game
					if (playerReady[0] && playerReady[1] && playerReady[2] && playerReady[3]) {
						CreatePlanet();
					}
				}
				readIndex += 4;
			}
			else if (commandID == (int)Command.CLIENT_PICK_PLANET) {
				if (gameState == GameState.CHOOSE_PLANET) {
					// Player pick a planet
					int planetID = commandID = System.BitConverter.ToInt32(data, readIndex + 1 * 4);
					
					// Check if the planet is picked
					if (planet[planetID].playerID == -1) {
						// If not, let the player pick it, broadcast to others
						planet[planetID].playerID = playerID;
						AppendBroadcastCommand (System.BitConverter.GetBytes((int)Command.SERVER_PROVIDE_PLANET));
						AppendBroadcastCommand (System.BitConverter.GetBytes(playerID));
						AppendBroadcastCommand (System.BitConverter.GetBytes(planetID));
					}
					else {
						// If planet is picked, just ignore
					}
				}
				readIndex += 2 * 4;
			}
			else {
				// Just to avoid loop
				// Shouldn't go here
				readIndex ++;
			}
		}
	}
	
	
	
	
	
	
	
	
	private void CreatePlanet () {
		// Randomly create a list of planet
		List<int> planetsToPickFrom = new List<int>();
		for (int i=0; i<SCR_Config.NUMBER_OF_PLANET_TEMPLATE; i++) {
			// Currently, we have 17 planets template
			planetsToPickFrom.Add (i);
		}
		
		// Some size
		List<int> planetSizesToChoose = new List<int>();
		planetSizesToChoose.Add(1);
		planetSizesToChoose.Add(1);
		planetSizesToChoose.Add(2);
		planetSizesToChoose.Add(2);
		planetSizesToChoose.Add(2);
		planetSizesToChoose.Add(3);
		planetSizesToChoose.Add(3);
		planetSizesToChoose.Add(3);
		
		// New planet will be saved here
		planet = new FakePlanet[SCR_Config.NUMBER_OF_PLANET_CREATE];
		
		// Now create them
		float currentDistance = SCR_Config.ORBIT_DISTANCE_MAX;
		for (int i=0; i<SCR_Config.NUMBER_OF_PLANET_CREATE; i++) {
			currentDistance += Random.Range (SCR_Config.ORBIT_DISTANCE_MIN, SCR_Config.ORBIT_DISTANCE_MAX);
			
			int chosen 	= Random.Range(0, planetsToPickFrom.Count);
			int size 	= Random.Range(0, planetSizesToChoose.Count);
			
			int 	planetID 		= planetsToPickFrom[chosen];
			int		planetSize		= planetSizesToChoose[size];
			float 	planetDistance	= currentDistance;
			float	planetAngle		= Random.Range(0, 360);
			float 	orbitalSpeed 	= Mathf.Sqrt(SCR_Config.GRAVITY_CONSTANT * SCR_Config.SUN_MASS / currentDistance);
			float 	angularSpeed 	= Mathf.Atan(orbitalSpeed / currentDistance) * SCR_Helper.RAD_TO_DEG;
			
			planet[i] = new FakePlanet(planetID, planetSize, planetDistance, planetAngle, angularSpeed);
			
			planetsToPickFrom.RemoveAt (chosen);
			planetSizesToChoose.RemoveAt (size);
		}
		
		// Send a command to client
		AppendBroadcastCommand (System.BitConverter.GetBytes((int)Command.SERVER_CREATE_PLANET));
		AppendBroadcastCommand (System.BitConverter.GetBytes(SCR_Config.NUMBER_OF_PLANET_CREATE));
		for (int i=0; i<SCR_Config.NUMBER_OF_PLANET_CREATE; i++) {
			AppendBroadcastCommand (System.BitConverter.GetBytes(planet[i].id));
			AppendBroadcastCommand (System.BitConverter.GetBytes(planet[i].size));
			AppendBroadcastCommand (System.BitConverter.GetBytes(planet[i].distance));
			AppendBroadcastCommand (System.BitConverter.GetBytes(planet[i].angle));
			AppendBroadcastCommand (System.BitConverter.GetBytes(planet[i].speed));
		}
		
		// Switch state
		gameState = GameState.CHOOSE_PLANET;
	}
}
