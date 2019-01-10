using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Sun : MonoBehaviour {
	public float ROTATION_SPEED;
	public float MASS;
	
	private float angle = 0;
	
    private void Start() {
        
    }

    private void Update() {
        float dt = Time.deltaTime;
		angle += ROTATION_SPEED * dt;
		if (angle > 360) angle -= 360;
		else if (angle < 0) angle += 360;
		
		gameObject.transform.localEulerAngles = new Vector3(0, angle, 0);
    }
}
