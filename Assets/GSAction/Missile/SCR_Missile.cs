using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Missile : MonoBehaviour {
	public const float DELAY_KILL = 5.0f;
	float delayKillCount = 0;
	Vector3 velocity = new Vector3 (0, 0, 0);
	
    private void Start() {
        
    }
	private void Update() {
		float dt = Time.deltaTime;
        if (delayKillCount > 0) {
			delayKillCount -= dt;
			
			if (delayKillCount <= 0) {
				gameObject.SetActive (false);
			}
		}
		else {
			transform.position += velocity * dt;
		}
    }
	
	public void Spawn(float x, float z) {
		gameObject.GetComponent<TrailRenderer>().Clear();
		gameObject.SetActive (false);
		transform.position = new Vector3(x, 0, z);
		gameObject.SetActive (true);
	}
	
	public void UpdatePos (float x, float z, float vx, float vz) {
		transform.position = new Vector3(x, 0, z);
		velocity = new Vector3(vx, 0, vz);
	}
	
	public void Kill() {
		delayKillCount = DELAY_KILL;
	}
}
