using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeMissile {
	public const float DELAY_KILL = 5.0f;
	public const float AVOID_OWN_PLANET = 5.0f;
	
    public int 		id;
	public int 		planetID;
	public int 		damage;
	public float 	lifeTime;
	public bool		steer;
	
	public Vector2 position;
	public Vector2 velocity;
	
	private float avoidOwnPlanetCounter = 0;
	
	public FakeMissile (int index) {
		id = index;
		lifeTime = 0;
	}
	
	public void Spawn (int pid, Vector2 p, Vector2 v, int d, bool s) {
		planetID = pid;
		lifeTime = SCR_Config.MISSILE_LIFE + DELAY_KILL;
		position = p;
		velocity = v;
		damage 	 = d;
		avoidOwnPlanetCounter = AVOID_OWN_PLANET;
		steer = s;
	}
	
	public void FixedUpdate (float dt) {
		if (lifeTime > DELAY_KILL) {
			lifeTime -= dt;
			position += velocity * dt;
			if (lifeTime <= DELAY_KILL) {
				SCR_FakeServer.instance.KillMissile (id);
			}
			
			// Attract to Sun
			float distance = SCR_Helper.DistanceBetweenTwoPoint (position.x, position.y, 0, 0);
			float attractionForce = (SCR_Config.GRAVITY_CONSTANT * SCR_Config.SUN_MASS) / (distance * distance);
			float attractionAngle = SCR_Helper.AngleBetweenTwoPoint (position.x, position.y, 0, 0);
			Vector2 attraction = new Vector2(attractionForce * SCR_Helper.Sin(attractionAngle), attractionForce * SCR_Helper.Cos(attractionAngle));
			velocity += attraction * dt * SCR_Config.MISSILE_GRAVITY_BOOST;
			
			if (distance < SCR_Config.SUN_RADIUS) {
				lifeTime = DELAY_KILL;
				SCR_FakeServer.instance.KillMissile (id);
			}
			
			// Attract to planet
			for (int i=0; i<SCR_FakeServer.instance.planet.Length; i++) {
				if (i == planetID && avoidOwnPlanetCounter > 0) {
					
				}
				else {
					distance = SCR_Helper.DistanceBetweenTwoPoint (position.x, position.y, SCR_FakeServer.instance.planet[i].x, SCR_FakeServer.instance.planet[i].z);
					attractionForce = (SCR_Config.GRAVITY_CONSTANT * SCR_FakeServer.instance.planet[i].mass) / (distance * distance);
					if (steer && distance < SCR_Config.PLANET_RADIUS * 2 && SCR_FakeServer.instance.planet[i].playerID > -1) {
						attractionForce *= 10;
					}
					attractionAngle = SCR_Helper.AngleBetweenTwoPoint (position.x, position.y, SCR_FakeServer.instance.planet[i].x, SCR_FakeServer.instance.planet[i].z);
					attraction = new Vector2(attractionForce * SCR_Helper.Sin(attractionAngle), attractionForce * SCR_Helper.Cos(attractionAngle));
					velocity += attraction * dt * SCR_Config.MISSILE_GRAVITY_BOOST;
					
					if (distance < SCR_Config.PLANET_RADIUS) {
						lifeTime = DELAY_KILL;
						SCR_FakeServer.instance.KillMissile (id);
						SCR_FakeServer.instance.planet[i].Hit (Mathf.RoundToInt(Random.Range (0.8f, 1.2f) * damage), planetID);
					}
				}
			}
		}
		else if (lifeTime > 0) {
			lifeTime -= dt;
		}
		
		if (avoidOwnPlanetCounter > 0) {
			avoidOwnPlanetCounter -= dt;
		}
	}
}
