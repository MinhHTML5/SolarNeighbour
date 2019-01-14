using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public class SCR_FakeServer : MonoBehaviour {
	public static bool 	useFakeServer;
	
	public int			SUN_MASS				 	= 1000000000;
	public int			NUMBER_OF_PLANET_TEMPLATE 	= 17;
	public int			NUMBER_OF_PLANET_CREATE		= 8;
	public float		ORBIT_DISTANCE_MIN			= 50;
	public float		ORBIT_DISTANCE_MAX			= 150;
	public float		GRAVITY_CONSTANT 			= 0.000001f;
	
	private GameState 	gameState;
	private byte[]		packet;
	
    private void Start() {
		if (SCR_Loading.firstTimeRun) {
			return;
		}
		
		// Whenever this script is enabled, switch to fake mode
		useFakeServer = true;
		
		// First state
        gameState = GameState.INIT;
		packet = new byte[0];
		
		// Randomly create a list of planet
		List<int> planetsToPickFrom = new List<int>();
		for (int i=0; i<NUMBER_OF_PLANET_TEMPLATE; i++) {
			// Currently, we have 17 planets template
			planetsToPickFrom.Add (i);
		}
		
		// New planet will be saved here
		int[] planetID 			= new int[NUMBER_OF_PLANET_CREATE];
		int[] planetSpeed 		= new int[NUMBER_OF_PLANET_CREATE];
		int[] planetDistance 	= new int[NUMBER_OF_PLANET_CREATE];
		int[] planetAngle 		= new int[NUMBER_OF_PLANET_CREATE];
		
		// Now create them
		float currentDistance = 0;
		for (int i=0; i<NUMBER_OF_PLANET_CREATE; i++) {
			currentDistance += Random.Range (ORBIT_DISTANCE_MIN, ORBIT_DISTANCE_MAX);
			
			int chosen = Random.Range(0, planetsToPickFrom.Count);
			
			planetID[i] 		= planetsToPickFrom[chosen];
			planetDistance[i]	= Mathf.RoundToInt(currentDistance);
			planetAngle[i]		= Random.Range(0, 360);
			 
			float orbitalSpeed 	= Mathf.Sqrt(GRAVITY_CONSTANT * SUN_MASS / currentDistance);
			float angularSpeed 	= Mathf.Atan(orbitalSpeed / currentDistance);
			planetSpeed[i]		= Mathf.RoundToInt(angularSpeed * SCR_Helper.RAD_TO_DEG);
			
			planetsToPickFrom.RemoveAt (chosen);
		}
		
		// Send a command to client
		AppendCommand (System.BitConverter.GetBytes((int)Command.CREATE_PLANET));
		AppendCommand (System.BitConverter.GetBytes((int)NUMBER_OF_PLANET_CREATE));
		for (int i=0; i<NUMBER_OF_PLANET_CREATE; i++) {
			AppendCommand (System.BitConverter.GetBytes(planetID[i]));
			AppendCommand (System.BitConverter.GetBytes(planetDistance[i]));
			AppendCommand (System.BitConverter.GetBytes(planetAngle[i]));
			AppendCommand (System.BitConverter.GetBytes(planetSpeed[i]));
		}
    }
	
	private void AppendCommand(byte[] data) {
		int originalLength = packet.Length;
		System.Array.Resize<byte>(ref packet, packet.Length + data.Length);
		System.Array.Copy(data, 0, packet, originalLength, data.Length);
	}

    private void Update() {
		if (SCR_Loading.firstTimeRun) {
			return;
		}
        if (packet.Length > 0) {
			SCR_Client.instance.OnDataReceive (packet);
			packet = new byte[0];
		}
    }
}
