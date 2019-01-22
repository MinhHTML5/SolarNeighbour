using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeMissile {
    public int 		id;
	public int 		planetID;
	public int 		damage;
	public float 	lifeTime;
	
	public Vector2 position;
	public Vector2 velocity;
	
	public FakeMissile (int index) {
		id = index;
		lifeTime = 0;
	}
	
	public void Spawn (Vector2 p, Vector2 v) {
		lifeTime = SCR_Config.MISSILE_LIFE;
		position = p;
		velocity = v;
	}
	
	public void FixedUpdate (float dt) {
		lifeTime -= dt;
		
		if (lifeTime > 0) {
			position += velocity * dt;
		}
	}
}
