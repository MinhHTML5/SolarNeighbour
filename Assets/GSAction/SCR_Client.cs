using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public enum GameState {
	INIT = 0,
	CHOOSE_PLANET,
	ACTION
}

public enum Command {
	PING = 0,
	CREATE_PLANET,
}


public class SCR_Client : MonoBehaviour {
	public static SCR_Client instance;
	
	private void Start() {
        instance = this;
    }

    private void Update() {
        
    }
	
	public void OnDataReceive(byte[] data) {
		int readIndex = 0;
		int commandID = 0;
		
		while (readIndex < data.Length) {
			commandID = BitConverter.ToInt32(data, readIndex);
			if (commandID == (int)Command.CREATE_PLANET) {
				int planetNumber = BitConverter.ToInt32(data, readIndex + 1 * 4);
				int[] planetID 			= new int[planetNumber];
				int[] planetSpeed 		= new int[planetNumber];
				int[] planetDistance 	= new int[planetNumber];
				int[] planetAngle 		= new int[planetNumber];
				
				for (int i=0; i<planetNumber; i++) {
					planetID[i] 		= BitConverter.ToInt32(data, readIndex + (i * 4 + 2) * 4);
					planetDistance[i] 	= BitConverter.ToInt32(data, readIndex + (i * 4 + 3) * 4);
					planetAngle[i] 		= BitConverter.ToInt32(data, readIndex + (i * 4 + 4) * 4);
					planetSpeed[i] 		= BitConverter.ToInt32(data, readIndex + (i * 4 + 5) * 4);
				}
				
				readIndex += (planetNumber * 4 + 2) * 4;
				SCR_Action.instance.CreatePlanet (planetID, planetDistance, planetAngle, planetSpeed);
			}
		}
	}
}
