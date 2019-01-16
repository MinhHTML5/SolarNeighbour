using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public enum GameState {
	IDLE = 0,
	INIT,
	CHOOSE_PLANET,
	ACTION
}

public enum Command {
	PING = 0,
	CLIENT_READY,
	CLIENT_PICK_PLANET,
	SERVER_SWITCH_STATE,
	SERVER_CREATE_PLANET,
	SERVER_UPDATE_PLANET,
}


public class SCR_Client : MonoBehaviour {
	public static SCR_Client instance;
	
	private byte[]			packet;
	
	private void Start() {
		if (SCR_Loading.firstTimeRun) {
			return;
		}
        instance = this;
		packet = new byte[0];
		
		// Send ready command
		AppendCommand (System.BitConverter.GetBytes((int)Command.CLIENT_READY));
    }
	
	private void AppendCommand (byte[] data) {
		int originalLength = packet.Length;
		System.Array.Resize<byte>(ref packet, packet.Length + data.Length);
		System.Array.Copy(data, 0, packet, originalLength, data.Length);
	}

    private void Update() {
		if (SCR_Loading.firstTimeRun) {
			return;
		}
        // Send packet
        if (packet.Length > 0) {
			if (SCR_FakeServer.useFakeServer) {
				SCR_FakeServer.instance.OnDataReceive (packet);
			}
			else {
				// Real TCP goes here
			}
			packet = new byte[0];
		}
    }
	
	
	
	public void OnDataReceive(byte[] data) {
		int readIndex = 0;
		int commandID = 0;
		
		while (readIndex < data.Length) {
			commandID = BitConverter.ToInt32(data, readIndex);
			if (commandID == (int)Command.SERVER_CREATE_PLANET) {
				int planetNumber = BitConverter.ToInt32(data, readIndex + 1 * 4);
				int[] planetID 			= new int[planetNumber];
				int[] planetSize 		= new int[planetNumber];
				float[] planetDistance 	= new float[planetNumber];
				float[] planetAngle 	= new float[planetNumber];
				float[] planetSpeed 	= new float[planetNumber];
				
				for (int i=0; i<planetNumber; i++) {
					planetID[i] 		= BitConverter.ToInt32(data, readIndex + (i * 5 + 2) * 4);
					planetSize[i] 		= BitConverter.ToInt32(data, readIndex + (i * 5 + 3) * 4);
					planetDistance[i] 	= BitConverter.ToSingle(data, readIndex + (i * 5 + 4) * 4);
					planetAngle[i] 		= BitConverter.ToSingle(data, readIndex + (i * 5 + 5) * 4);
					planetSpeed[i] 		= BitConverter.ToSingle(data, readIndex + (i * 5 + 6) * 4);
				}
				
				readIndex += (planetNumber * 5 + 2) * 4;
				SCR_Action.instance.CreatePlanet (planetID, planetSize, planetDistance, planetAngle, planetSpeed);
			}
			else if (commandID == (int)Command.SERVER_UPDATE_PLANET) {
				int planetNumber	= SCR_Action.instance.planets.Length;
				float[] planetAngle	= new float[planetNumber];
				for (int i=0; i<planetNumber; i++) {
					planetAngle[i]	= BitConverter.ToSingle(data, readIndex + (i + 1) * 4);
				}
				
				readIndex += (planetNumber + 1) * 4;
				SCR_Action.instance.UpdatePlanet (planetAngle);
			}
			else {
				// Just to avoid loop
				// Shouldn't go here
				readIndex ++;
			}
		}
	}
}
