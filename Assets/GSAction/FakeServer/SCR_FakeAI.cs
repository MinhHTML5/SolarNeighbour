using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SCR_FakeAI : MonoBehaviour {
	private int 		playerID;
	private byte[]		packet;
	private GameState 	gameState;
	
	private int 		planetID;
	private float 		timeCounter;
	
	
	
    public void Init(int id) {
		gameState = GameState.INIT;
		packet = new byte[0];
        playerID = id;
		planetID = -1;
		timeCounter = playerID * 0.5f + UnityEngine.Random.Range (0, 0.5f);
    }
	
	private void AppendCommand (byte[] data) {
		int originalLength = packet.Length;
		System.Array.Resize<byte>(ref packet, packet.Length + data.Length);
		System.Array.Copy(data, 0, packet, originalLength, data.Length);
	}
	
	public void OnDataReceive (byte[] data) {
		int readIndex = 0;
		int commandID = 0;
		
		while (readIndex < data.Length) {
			commandID = BitConverter.ToInt32(data, readIndex);
			if (commandID == (int)Command.SERVER_PROVIDE_ID) {
				// Cheat
				readIndex += 2 * 4;
			}
			else if (commandID == (int)Command.SERVER_CREATE_PLANET) {
				// Cheat
				int planetNumber = BitConverter.ToInt32(data, readIndex + 1 * 4);
				readIndex += (planetNumber * 5 + 2) * 4;
				gameState = GameState.CHOOSE_PLANET;
				timeCounter = playerID * 1.5f + UnityEngine.Random.Range (0, 2.0f);
			}
			else if (commandID == (int)Command.SERVER_PROVIDE_PLANET) {
				// Cheat
				int playerIndex = BitConverter.ToInt32(data, readIndex + 1 * 4);
				int planetIndex = BitConverter.ToInt32(data, readIndex + 2 * 4);
				
				if (playerID == playerIndex) {
					planetID = planetIndex;
				}
				
				readIndex += 3 * 4;
			}
			else if (commandID == (int)Command.SERVER_UPDATE_PLANET) {
				// Cheat
				int planetNumber = SCR_Action.instance.planets.Length;
				readIndex += (planetNumber + 1) * 4;
			}
			else {
				// Just to avoid loop
				// Shouldn't go here
				readIndex += 4;
			}
		}
	}

    private void Update() {
        float dt = Time.deltaTime;
		
		if (gameState == GameState.INIT) {
			// Fake ready after a while
			timeCounter -= dt;
			if (timeCounter <= 0) {
				AppendCommand (System.BitConverter.GetBytes((int)Command.CLIENT_READY));
				timeCounter += 5.0f;
			}
		}
		else if (gameState == GameState.CHOOSE_PLANET) {
			// Fake pick planet after a while
			timeCounter -= dt;
			if (timeCounter <= 0 && planetID == -1) {
				bool picked = false;
				while (!picked) {
					int choose = UnityEngine.Random.Range (0, SCR_Action.instance.planets.Length);
					if (SCR_Action.instance.planets[choose].GetComponent<SCR_Planet>().playerID == -1) {
						picked = true;
						AppendCommand (System.BitConverter.GetBytes((int)Command.CLIENT_PICK_PLANET));
						AppendCommand (System.BitConverter.GetBytes(choose));
						
						// In case cannot pick
						timeCounter += 1.0f;
					}
				}
			}
		}
		
		
		
		
		
		
		if (packet.Length > 0) {
			SCR_FakeServer.instance.OnDataReceive (packet, playerID);
			packet = new byte[0];
		}
    }
}
