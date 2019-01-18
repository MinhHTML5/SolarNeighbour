using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SCR_FakeAI : MonoBehaviour {
	private int 		playerID;
	private byte[]		packet;
	private GameState 	gameState;
	
	private float 		readyCounter;
	
	
	
    
    public void Init(int id) {
		gameState = GameState.INIT;
		packet = new byte[0];
        playerID = id;
		readyCounter = id * 0.5f + UnityEngine.Random.Range (0, 0.5f);
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
			}
			else if (commandID == (int)Command.SERVER_PROVIDE_PLANET) {
				// Cheat
				readIndex += 3 * 4;
			}
			else if (commandID == (int)Command.SERVER_UPDATE_PLANET) {
				// Cheat
				int planetNumber	= SCR_Action.instance.planets.Length;
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
			readyCounter -= dt;
			if (readyCounter <= 0) {
				AppendCommand (System.BitConverter.GetBytes((int)Command.CLIENT_READY));
				readyCounter += 5.0f;
			}
		}
		
		
		
		
		
		if (packet.Length > 0) {
			SCR_FakeServer.instance.OnDataReceive (packet, playerID);
			packet = new byte[0];
		}
    }
}
