using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Net.Sockets;

using UnityEngine;
using UnityEngine.Networking;



public enum Command {
	PING = 0,
	CLIENT_READY,
	CLIENT_PICK_PLANET,
	CLIENT_SHOOT,
	CLIENT_UPGRADE,
	SERVER_PROVIDE_ID,
	SERVER_SWITCH_STATE,
	SERVER_CREATE_PLANET,
	SERVER_PROVIDE_PLANET,
	SERVER_START_GAME,
	SERVER_UPDATE_PLANET,
	SERVER_CREATE_MISSILE,
	SERVER_UPDATE_MISSILE,
	SERVER_KILL_MISSILE,
	SERVER_UPGRADE,
}


public class SCR_Client : MonoBehaviour {
	public static SCR_Client instance;
	TcpClient client;
	NetworkStream stream;
	
	private byte[]	packet;
	
	private void Start() {
		if (SCR_Loading.firstTimeRun) {
			return;
		}
        instance = this;
		packet = new byte[0];
		
		// Connect real
		if (!SCR_FakeServer.useFakeServer) {
			ConnectToRealServer();
		}
		else {
			// Send ready command
			AppendCommand (System.BitConverter.GetBytes((int)Command.CLIENT_READY));
			
			// Send name
			byte[] encoded = System.Text.Encoding.UTF8.GetBytes(SCR_Action.playerName);
			AppendCommand (System.BitConverter.GetBytes((int)encoded.Length));
			AppendCommand (encoded);
		}
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
				SCR_FakeServer.instance.OnDataReceive (packet, 0);
			}
			else {
				// Real TCP goes here
				stream.Write(packet, 0, packet.Length);
			}
			packet = new byte[0];
		}
		
		// Receive packet
		byte[] data = new byte[512];
		int dataLength = stream.Read(data, 0, data.Length);
		if (dataLength > 0) {
			OnDataReceive (data, dataLength);
		}
    }
	
	
	
	public void ChoosePlanet(int index) {
		AppendCommand (System.BitConverter.GetBytes((int)Command.CLIENT_PICK_PLANET));
		AppendCommand (System.BitConverter.GetBytes(index));
	}
	
	public void Shoot(float angle, float force) {
		AppendCommand (System.BitConverter.GetBytes((int)Command.CLIENT_SHOOT));
		AppendCommand (System.BitConverter.GetBytes(angle));
		AppendCommand (System.BitConverter.GetBytes(force));
	}
	
	public void Upgrade(int upgradeID) {
		AppendCommand (System.BitConverter.GetBytes((int)Command.CLIENT_UPGRADE));
		AppendCommand (System.BitConverter.GetBytes(upgradeID));
	}
	
	
	public void OnDataReceive(byte[] data, int length = 0) {
		int readIndex = 0;
		int commandID = 0;
		
		if (length == 0) length = data.Length;
		
		while (readIndex < length) {
			commandID = BitConverter.ToInt32(data, readIndex);
			if (commandID == (int)Command.SERVER_PROVIDE_ID) {
				// After we send client ready to server, server will reply with playerID
				SCR_Action.instance.playerID = BitConverter.ToInt32(data, readIndex + 1 * 4);
				readIndex += 2 * 4;
			}
			else if (commandID == (int)Command.SERVER_CREATE_PLANET) {
				// If all 4 players are ready, we'll get planet list
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
			else if (commandID == (int)Command.SERVER_PROVIDE_PLANET) {
				int playerIndex = BitConverter.ToInt32(data, readIndex + 1 * 4);
				int planetIndex = BitConverter.ToInt32(data, readIndex + 2 * 4);
				int byteLength = System.BitConverter.ToInt32(data, readIndex + 3 * 4);
				
				SCR_Action.instance.playerNames[playerIndex] = ByteToString (data, readIndex + 4 * 4, byteLength);
				SCR_Action.instance.PickPlanet (playerIndex, planetIndex);
				
				SCR_UILeftControl.instance.SetName (playerIndex, SCR_Action.instance.playerNames[playerIndex]);
				
				readIndex += 4 * 4 + byteLength;
			}
			else if (commandID == (int)Command.SERVER_START_GAME) {
				SCR_Action.instance.StartGame();
				readIndex += 4;
			}
			else if (commandID == (int)Command.SERVER_UPDATE_PLANET) {
				int planetNumber	= SCR_Action.instance.planets.Length;
				float[] planetAngle	= new float[planetNumber];
				int[] population	= new int[planetNumber];
				int[] resource		= new int[planetNumber];
				for (int i=0; i<planetNumber; i++) {
					planetAngle[i]	= BitConverter.ToSingle(data, readIndex + (i * 3 + 1) * 4);
					population[i]	= BitConverter.ToInt32(data, readIndex + (i * 3 + 2) * 4);
					resource[i]		= BitConverter.ToInt32(data, readIndex + (i * 3 + 3) * 4);
				}
				
				readIndex += (planetNumber * 3 + 1) * 4;
				SCR_Action.instance.UpdatePlanet (planetAngle, population, resource);
			}
			else if (commandID == (int)Command.SERVER_CREATE_MISSILE) {
				int id = BitConverter.ToInt32(data, readIndex + 1 * 4);
				float x = BitConverter.ToSingle(data, readIndex + 2 * 4);
				float y = BitConverter.ToSingle(data, readIndex + 3 * 4);
				int owner = BitConverter.ToInt32(data, readIndex + 4 * 4);
				readIndex += 5 * 4;
				SCR_Action.instance.SpawnMissile (id, x, y, owner);
			}
			else if (commandID == (int)Command.SERVER_UPDATE_MISSILE) {
				int missileNumber = BitConverter.ToInt32(data, readIndex + 1 * 4);
				
				for (int i=0; i<missileNumber; i++) {
					int id = BitConverter.ToInt32(data, readIndex + (i * 5 + 2) * 4);
					float x = BitConverter.ToSingle(data, readIndex + (i * 5 + 3) * 4);
					float y = BitConverter.ToSingle(data, readIndex + (i * 5 + 4) * 4);
					float vx = BitConverter.ToSingle(data, readIndex + (i * 5 + 5) * 4);
					float vy = BitConverter.ToSingle(data, readIndex + (i * 5 + 6) * 4);
					
					SCR_Action.instance.UpdateMissile (id, x, y, vx, vy);
				}
				
				readIndex += (missileNumber * 5 + 2) * 4;
			}
			else if (commandID == (int)Command.SERVER_KILL_MISSILE) {
				int id = BitConverter.ToInt32(data, readIndex + 1 * 4);
				SCR_Action.instance.KillMissile (id);
				readIndex += 2 * 4;
			}
			else if (commandID == (int)Command.SERVER_UPGRADE) {
				int playerID = BitConverter.ToInt32(data, readIndex + 1 * 4);
				int upgradeID = BitConverter.ToInt32(data, readIndex + 2 * 4);
				
				if (SCR_Action.instance.playerID == playerID) {
					SCR_Action.instance.upgradeState[upgradeID] = true;
					if (upgradeID == (int)UpgradeType.RELOADER) {
						SCR_Action.instance.cooldown = SCR_Config.MISSILE_UPGRADE_COOLDOWN;
					}
					
					break;
				}
				
				readIndex += 3 * 4;
			}
			else {
				// Just to avoid loop
				// Shouldn't go here
				Debug.Log ("ERROR");
				readIndex ++;
			}
		}
	}
	
	
	private string ByteToString (byte[] byteArray, int index, int length) {
		byte[] newByte = new byte[length];
		System.Array.Copy(byteArray, index, newByte, 0, length);
		return System.Text.Encoding.UTF8.GetString(newByte);
	}
	
	
	
	
	
	
	
	
	
	private void ConnectToRealServer() {
		client = new TcpClient("127.0.0.1", 3011);
		stream = client.GetStream();
		
		AppendCommand (System.BitConverter.GetBytes((int)Command.CLIENT_READY));
		
		byte[] encoded = System.Text.Encoding.UTF8.GetBytes(SCR_Action.playerName);
		AppendCommand (System.BitConverter.GetBytes((int)encoded.Length));
		AppendCommand (encoded);
	}
}
