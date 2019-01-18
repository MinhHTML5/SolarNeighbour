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
		
		if (pSize == 1)
			mass = 1000;
		else if (pSize == 2)
			mass = 10000;
		else if (pSize == 3)
			mass = 50000;
	}
	
	public void FixedUpdate (float dt) {
		angle += speed * dt;
		if (angle > 360)	angle -= 360;
		if (angle < 0) 		angle += 360;
		
		x = SCR_Helper.Sin(angle) * distance;
		z = SCR_Helper.Cos(angle) * distance;
	}
}
