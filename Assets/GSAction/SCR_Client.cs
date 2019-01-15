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
	UPDATE_PLANET,
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
			else if (commandID == (int)Command.UPDATE_PLANET) {
				int planetNumber	= SCR_Action.instance.planets.Length;
				float[] planetAngle	= new float[planetNumber];
				for (int i=0; i<planetNumber; i++) {
					planetAngle[i]	= BitConverter.ToSingle(data, readIndex + (i + 1) * 4);
				}
				
				readIndex += (planetNumber + 1) * 4;
				SCR_Action.instance.UpdatePlanet (planetAngle);
			}
		}
	}
}
