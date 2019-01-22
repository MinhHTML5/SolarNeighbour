using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeMissile {
	public const float DELAY_KILL = 5.0f;
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
		lifeTime = SCR_Config.MISSILE_LIFE + DELAY_KILL;
		position = p;
		velocity = v;
	}
	
	public void FixedUpdate (float dt) {
		if (lifeTime > DELAY_KILL) {
			lifeTime -= dt;
			position += velocity * dt;
			if (lifeTime <= DELAY_KILL) {
				SCR_FakeServer.instance.KillMissile (id);
			}
		}
		else if (lifeTime > 0) {
			lifeTime -= dt;
		}
	}
}
