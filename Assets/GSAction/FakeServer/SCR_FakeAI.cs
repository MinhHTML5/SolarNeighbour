using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FakeAI {
	private int 		playerID;
	private byte[]		packet;
	private GameState 	gameState;
	
	private int 		planetID;
	private float 		timeCounter;
	
	private float		cooldown;
	private float		cooldownMax;
	private int			upgradeTarget;
	private int			upgradeNumber;
	private bool[]		upgradeState;
	
	
    public void Init(int id) {
		gameState = GameState.INIT;
		packet = new byte[0];
        playerID = id;
		planetID = -1;
		timeCounter = playerID * 0.5f + UnityEngine.Random.Range (0, 0.5f);
		upgradeState = new bool [SCR_Config.upgrades.Length];
		cooldownMax = SCR_Config.MISSILE_BASE_COOLDOWN;
		upgradeNumber = 0;
		ChooseAnUpgradeRandomly();
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
				int byteLength = System.BitConverter.ToInt32(data, readIndex + 3 * 4);
				
				if (playerID == playerIndex) {
					planetID = planetIndex;
				}
				
				readIndex += 4 * 4 + byteLength;
			}
			else if (commandID == (int)Command.SERVER_START_GAME) {
				gameState = GameState.ACTION;
				cooldown = UnityEngine.Random.Range(0, 20) + cooldownMax;
				readIndex += 4;
			}
			else if (commandID == (int)Command.SERVER_UPDATE_PLANET) {
				// Cheat
				int planetNumber = SCR_Action.instance.planets.Length;
				readIndex += (planetNumber * 3 + 1) * 4;
			}
			else if (commandID == (int)Command.SERVER_CREATE_MISSILE) {
				readIndex += 5 * 4;
			}
			else if (commandID == (int)Command.SERVER_UPDATE_MISSILE) {
				int missileNumber = BitConverter.ToInt32(data, readIndex + 1 * 4);
				readIndex += (missileNumber * 5 + 2) * 4;
			}
			else if (commandID == (int)Command.SERVER_KILL_MISSILE) {
				readIndex += 2 * 4;
			}
			else if (commandID == (int)Command.SERVER_UPGRADE) {
				int pid = BitConverter.ToInt32(data, readIndex + 1 * 4);
				int upgradeID = BitConverter.ToInt32(data, readIndex + 2 * 4);
				
				if (playerID == pid) {
					upgradeState[upgradeID] = true;
					if (upgradeID == (int)UpgradeType.RELOADER) {
						cooldownMax = SCR_Config.MISSILE_UPGRADE_COOLDOWN;
					}
					break;
				}
				readIndex += 3 * 4;
			}
			else {
				// Just to avoid loop
				// Shouldn't go here
				readIndex += 4;
			}
		}
	}

    public void FixedUpdate(float dt) {
        if (gameState == GameState.INIT) {
			// Fake ready after a while
			timeCounter -= dt;
			if (timeCounter <= 0) {
				AppendCommand (System.BitConverter.GetBytes((int)Command.CLIENT_READY));
				
				string name = "";
				int choose = UnityEngine.Random.Range (0, 10);
				if (choose == 0) name = "BOT_Minh";
				if (choose == 1) name = "BOT_Canh";
				if (choose == 2) name = "BOT_AnHai";
				if (choose == 3) name = "BOT_Bikini";
				if (choose == 4) name = "BOT_Attila";
				if (choose == 5) name = "BOT_Sameer";
				if (choose == 6) name = "BOT_Noel";
				if (choose == 7) name = "BOT_Stephanie";
				if (choose == 8) name = "BOT_Loic";
				if (choose == 9) name = "BOT_Arthur";
				
				byte[] encoded = System.Text.Encoding.UTF8.GetBytes(name);
				AppendCommand (System.BitConverter.GetBytes((int)encoded.Length));
				AppendCommand (encoded);
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
		else if (gameState == GameState.ACTION) {
			cooldown -= dt;
			if (cooldown <= 0) {
				AppendCommand (System.BitConverter.GetBytes((int)Command.CLIENT_SHOOT));
				AppendCommand (System.BitConverter.GetBytes(UnityEngine.Random.Range(0, 360)));
				AppendCommand (System.BitConverter.GetBytes(1.0f));
				
				if (upgradeNumber < 3) {
					cooldown = UnityEngine.Random.Range(0, 20) + cooldownMax;
				}
				else if (upgradeNumber < 6) {
					cooldown = UnityEngine.Random.Range(0, 10) + cooldownMax;
				}
				else if (upgradeNumber < 9) {
					cooldown = UnityEngine.Random.Range(0, 5) + cooldownMax;
				}
				else {
					cooldown = cooldownMax;
				}
			}
			
			if (upgradeTarget == -1) {
				upgradeNumber ++;
				ChooseAnUpgradeRandomly();
			}
			else {
				if (SCR_FakeServer.instance.planet[planetID].resource > SCR_Config.upgrades[upgradeTarget].cost + 300) {
					AppendCommand (System.BitConverter.GetBytes((int)Command.CLIENT_UPGRADE));
					AppendCommand (System.BitConverter.GetBytes(upgradeTarget));
					upgradeTarget = -1;
				}
			}
		}
		
		
		
		if (packet.Length > 0) {
			SCR_FakeServer.instance.OnDataReceive (packet, playerID);
			packet = new byte[0];
		}
    }
	
	
	private string ByteToString (byte[] byteArray, int index, int length) {
		byte[] newByte = new byte[length];
		System.Array.Copy(byteArray, index, newByte, 0, length);
		return System.Text.Encoding.UTF8.GetString(newByte);
	}
	
	private void ChooseAnUpgradeRandomly() {
		if (upgradeNumber == (int)UpgradeType.COUNT - 1) {
			upgradeTarget = -1;
		}
		else {
			bool ok = false;
			while (!ok) {
				upgradeTarget = UnityEngine.Random.Range (0, (int)UpgradeType.COUNT);
				if (upgradeState[upgradeTarget] == false) {
					ok = true;
				}
			}
		}
	}
}
