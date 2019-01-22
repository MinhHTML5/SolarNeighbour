using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class FakePlanet {
	public int 		id;
	public int		size;
	public float 	mass;
    public float 	distance;
	public float 	angle;
	public float 	speed;
	public float 	playerID;
	
	public int		population;
	public float	resource;
	
	public float 	x;
	public float 	z;
	
	public FakePlanet (int pID, int pSize, float pDistance, float pAngle, float pSpeed) {
		id 			= pID;
		size 		= pSize;
		distance 	= pDistance;
		angle 		= pAngle;
		speed 		= pSpeed;
		playerID 	= -1;
		
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
	}
	
	public void FixedUpdate (float dt) {
		angle += speed * dt;
		if (angle > 360)	angle -= 360;
		if (angle < 0) 		angle += 360;
		
		x = SCR_Helper.Sin(angle) * distance;
		z = SCR_Helper.Cos(angle) * distance;
		
		
		resource += Mathf.Sqrt(population) * SCR_Config.RESOURCE_MULTIPLIER * dt;
	}
	
	public Vector2 GetPosition() {
		return new Vector2(x, z);
	}
	public Vector2 GetVelocity() {
		return new Vector2(0, 0);
	}
}
