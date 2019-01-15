﻿using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public class SCR_FakeServer : MonoBehaviour {
	public static bool 		useFakeServer;
	
	public int				SUN_MASS				 	= 1000000000;
	public int				NUMBER_OF_PLANET_TEMPLATE 	= 17;
	public int				NUMBER_OF_PLANET_CREATE		= 8;
	public float			ORBIT_DISTANCE_MIN			= 50;
	public float			ORBIT_DISTANCE_MAX			= 75;
	public float			GRAVITY_CONSTANT 			= 0.000001f;
	
	private GameState 		gameState;
	private FakePlanet[]	planet;
	
	private byte[]			packet;
	
	
	
	
	
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
		
		// Some size
		List<int> planetSizesToChoose = new List<int>();
		planetSizesToChoose.Add(1);
		planetSizesToChoose.Add(1);
		planetSizesToChoose.Add(1);
		planetSizesToChoose.Add(2);
		planetSizesToChoose.Add(2);
		planetSizesToChoose.Add(2);
		planetSizesToChoose.Add(3);
		planetSizesToChoose.Add(3);
		
		// New planet will be saved here
		planet = new FakePlanet[NUMBER_OF_PLANET_CREATE];
		
		// Now create them
		float currentDistance = ORBIT_DISTANCE_MAX;
		for (int i=0; i<NUMBER_OF_PLANET_CREATE; i++) {
			currentDistance += Random.Range (ORBIT_DISTANCE_MIN, ORBIT_DISTANCE_MAX);
			
			int chosen 	= Random.Range(0, planetsToPickFrom.Count);
			int size 	= Random.Range(0, planetSizesToChoose.Count);
			
			int 	planetID 		= planetsToPickFrom[chosen];
			int		planetSize		= planetSizesToChoose[size];
			float 	planetDistance	= currentDistance;
			float	planetAngle		= Random.Range(0, 360);
			float 	orbitalSpeed 	= Mathf.Sqrt(GRAVITY_CONSTANT * SUN_MASS / currentDistance);
			float 	angularSpeed 	= Mathf.Atan(orbitalSpeed / currentDistance) * SCR_Helper.RAD_TO_DEG;
			
			planet[i] = new FakePlanet(planetID, planetSize, planetDistance, planetAngle, angularSpeed);
			
			planetsToPickFrom.RemoveAt (chosen);
			planetSizesToChoose.RemoveAt (size);
		}
		
		// Send a command to client
		AppendCommand (System.BitConverter.GetBytes((int)Command.CREATE_PLANET));
		AppendCommand (System.BitConverter.GetBytes(NUMBER_OF_PLANET_CREATE));
		for (int i=0; i<NUMBER_OF_PLANET_CREATE; i++) {
			AppendCommand (System.BitConverter.GetBytes(planet[i].id));
			AppendCommand (System.BitConverter.GetBytes(planet[i].size));
			AppendCommand (System.BitConverter.GetBytes(planet[i].distance));
			AppendCommand (System.BitConverter.GetBytes(planet[i].angle));
			AppendCommand (System.BitConverter.GetBytes(planet[i].speed));
		}
    }
	
	

    private void FixedUpdate() {
		if (SCR_Loading.firstTimeRun) {
			return;
		}
		
		float dt = Time.fixedDeltaTime;
		
		// Update planets movement
		for (int i=0; i<NUMBER_OF_PLANET_CREATE; i++) {
			planet[i].FixedUpdate (dt);
		}
		
		AppendCommand (System.BitConverter.GetBytes((int)Command.UPDATE_PLANET));
		for (int i=0; i<NUMBER_OF_PLANET_CREATE; i++) {
			AppendCommand (System.BitConverter.GetBytes(planet[i].angle));
		}
		
		
		
		// Send packet
        if (packet.Length > 0) {
			SCR_Client.instance.OnDataReceive (packet);
			packet = new byte[0];
		}
    }
	
	
	
	
	
	
	private void AppendCommand(byte[] data) {
		int originalLength = packet.Length;
		System.Array.Resize<byte>(ref packet, packet.Length + data.Length);
		System.Array.Copy(data, 0, packet, originalLength, data.Length);
	}
}