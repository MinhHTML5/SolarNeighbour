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
	
	public int		population;
	public float	resource;
	public int		damage;
	
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
		damage = 1500;
		cooldown = 10;
	}
	
	public void FixedUpdate (float dt) {
		angle += speed * dt;
		if (angle > 360)	angle -= 360;
		if (angle < 0) 		angle += 360;
		
		x = SCR_Helper.Sin(angle) * distance;
		z = SCR_Helper.Cos(angle) * distance;
		
		resource += Mathf.Sqrt(population) * SCR_Config.RESOURCE_MULTIPLIER * dt;
		
		if (cooldownCounter > 0) {
			cooldownCounter -= dt;
		}
	}
	
	public Vector2 GetPosition() {
		return new Vector2(x, z);
	}
	public Vector2 GetVelocity() {
		float moveAngle = angle + 90;
		return new Vector2(orbitalSpeed * SCR_Helper.Sin(moveAngle), orbitalSpeed * SCR_Helper.Cos(moveAngle));
	}
	
	
	public void Shoot (float angle, float force) {
		if (cooldownCounter <= 0) {
			cooldownCounter = cooldown;
			float speed = force * SCR_Config.MISSILE_SPEED;
			Vector2 velocity = new Vector2 (speed * SCR_Helper.Sin (angle), speed * SCR_Helper.Cos (angle));
			Vector2 position = new Vector2 (0, 0);
			position = GetPosition();
			velocity += GetVelocity();
			
			SCR_FakeServer.instance.StartMissile (playerID, planetID, position, velocity, damage);
		}
	}
	public void Hit (int damage) {
		population -= damage;
		if (population < 0) population = 0;
	}
}
