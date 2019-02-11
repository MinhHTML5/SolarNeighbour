using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class FakePlanet {
	public int 		planetID;
	public int 		prefabID;
	public int		size;
	public float 	mass;
    public float 	distance;
	public float 	angle;
	public float 	speed;
	public float 	orbitalSpeed;
	public int 		playerID;
	
	public float	population;
	public float	resource;
	public int		damage;
	public bool[]	upgradeState;
	
	public float 	cooldown;
	public float 	cooldownCounter;
	
	
	public float 	x;
	public float 	z;
	
	public FakePlanet (int pID, int prefab, int pSize, float pDistance, float pAngle, float pSpeed, float poSpeed) {
		planetID		= pID;
		prefabID		= prefab;
		size 			= pSize;
		distance 		= pDistance;
		angle 			= pAngle;
		speed 			= pSpeed;
		orbitalSpeed 	= poSpeed;
		playerID 		= -1;
				
		
		x = SCR_Helper.Sin(angle) * distance;
		z = SCR_Helper.Cos(angle) * distance;
		
		if (pSize == 1) {
			mass = SCR_Config.MASS_TINY;
			population = SCR_Config.POPULATION_TINY;
		}
		else if (pSize == 2) {
			mass = SCR_Config.MASS_MEDIUM;
			population = SCR_Config.POPULATION_MEDIUM;
		}
		else if (pSize == 3) {
			mass = SCR_Config.MASS_LARGE;
			population = SCR_Config.POPULATION_LARGE;
		}
		
		resource = 0;
		damage = SCR_Config.MISSILE_BASE_DAMAGE;
		cooldown = SCR_Config.MISSILE_BASE_COOLDOWN;
		upgradeState = new bool [SCR_Config.upgrades.Length];
	}
	
	public void FixedUpdate (float dt) {
		angle += speed * dt;
		if (angle > 360)	angle -= 360;
		if (angle < 0) 		angle += 360;
		
		x = SCR_Helper.Sin(angle) * distance;
		z = SCR_Helper.Cos(angle) * distance;
		
		if (upgradeState[(int)UpgradeType.AUTOMATE] == true) {
			int popCap = 0;
			if (mass == SCR_Config.MASS_TINY) 			popCap = SCR_Config.POPULATION_TINY;
			else if (mass == SCR_Config.MASS_MEDIUM) 	popCap = SCR_Config.POPULATION_MEDIUM;
			else if (mass == SCR_Config.MASS_LARGE)		popCap = SCR_Config.POPULATION_LARGE;
			resource += Mathf.Sqrt(popCap) * SCR_Config.RESOURCE_MULTIPLIER * dt;
		}
		else {
			resource += Mathf.Sqrt(population) * SCR_Config.RESOURCE_MULTIPLIER * dt;
		}
		
		if (cooldownCounter > 0) {
			cooldownCounter -= dt;
		}
		
		
		if (population > 0 && upgradeState[(int)UpgradeType.REGENERATE] == true) {
			int popCap = 0;
			if (mass == SCR_Config.MASS_TINY) {
				popCap = SCR_Config.POPULATION_TINY;
			}
			else if (mass == SCR_Config.MASS_MEDIUM) {
				popCap = SCR_Config.POPULATION_MEDIUM;
			}
			else if (mass == SCR_Config.MASS_LARGE) {
				popCap = SCR_Config.POPULATION_LARGE;
			}
			
			if (population < popCap) {
				population += population * SCR_Config.PLANET_REGEN_RATE * dt;
				if (population > popCap) {
					population = popCap;
				}
			}
		}
	}
	
	public Vector2 GetPosition() {
		return new Vector2(x, z);
	}
	public Vector2 GetVelocity() {
		float moveAngle = angle + 90;
		return new Vector2(orbitalSpeed * SCR_Helper.Sin(moveAngle), orbitalSpeed * SCR_Helper.Cos(moveAngle));
	}
	
	public void Hit (int damage, int ownerID) {
		if (ownerID == planetID && upgradeState[(int)UpgradeType.SELF_DESTRUCT] == true) {
			
		}
		else {
			if (upgradeState[(int)UpgradeType.DEFEND] == true) {
				population -= (damage * 3) / 4;
			}
			else {
				population -= damage;
			}
			
			if (population < 0) population = 0;
		}
	}
	public void Shoot (float angle, float force) {
		if (cooldownCounter <= 0 && population > 0) {
			int price = SCR_Config.MISSILE_BASE_PRICE;
			if (upgradeState[(int)UpgradeType.MATERIAL] == true) {
				price = SCR_Config.MISSILE_UPGRADE_PRICE;
			}
			if (upgradeState[(int)UpgradeType.CLUSTER] == true) {
				price *= 2;
			}

			if (resource >= price) {
				bool steer = false;
				if (upgradeState[(int)UpgradeType.SELF_STEERING]) {
					steer = true;
				}
				resource -= price;
				cooldownCounter = cooldown;
				float speed = force * SCR_Config.MISSILE_SPEED;
				Vector2 velocity = new Vector2 (speed * SCR_Helper.Sin (angle), speed * SCR_Helper.Cos (angle));
				Vector2 position = new Vector2 (0, 0);
				position = GetPosition();
				velocity += GetVelocity();
				
				SCR_FakeServer.instance.StartMissile (playerID, planetID, position, velocity, damage, steer);
				if (upgradeState[(int)UpgradeType.CLUSTER] == true) {
					velocity = new Vector2 (speed * SCR_Helper.Sin (angle + 10), speed * SCR_Helper.Cos (angle + 10));
					SCR_FakeServer.instance.StartMissile (playerID, planetID, position, velocity, damage, steer);
					velocity = new Vector2 (speed * SCR_Helper.Sin (angle - 10), speed * SCR_Helper.Cos (angle - 10));
					SCR_FakeServer.instance.StartMissile (playerID, planetID, position, velocity, damage, steer);
				}
			}
		}
	}
	public void Upgrade (int upgradeID) {
		if (population > 0 && upgradeState[upgradeID] == false) {
			if (resource >= SCR_Config.upgrades[upgradeID].cost) {
				upgradeState[upgradeID] = true;
				resource -= SCR_Config.upgrades[upgradeID].cost;
				SCR_FakeServer.instance.Upgrade (playerID, upgradeID);
				
				if (upgradeID == (int)UpgradeType.DAMAGE_1) {
					damage += SCR_Config.MISSILE_UPGRADE_DAMAGE_1;
				}
				else if (upgradeID == (int)UpgradeType.DAMAGE_2) {
					damage += SCR_Config.MISSILE_UPGRADE_DAMAGE_2;
				}
				else if (upgradeID == (int)UpgradeType.SELF_DESTRUCT) {
				}
				else if (upgradeID == (int)UpgradeType.SELF_STEERING) {
				}
				else if (upgradeID == (int)UpgradeType.RELOADER) {
					cooldown = SCR_Config.MISSILE_UPGRADE_COOLDOWN;
				}
				else if (upgradeID == (int)UpgradeType.MATERIAL) {
				}
				else if (upgradeID == (int)UpgradeType.CLUSTER) {
				}
				else if (upgradeID == (int)UpgradeType.DEFEND) {
				}
				else if (upgradeID == (int)UpgradeType.REGENERATE) {
				}
				else if (upgradeID == (int)UpgradeType.AUTOMATE) {
				}
			}
		}
	}
}
