using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Sun : MonoBehaviour {
	public float ROTATION_SPEED;
	public float MASS;
	
	public static float M;
	
	private float rotation = 0;
	
    private void Start() {
        M = MASS;
    }

    private void Update() {
        float dt = Time.deltaTime;
		rotation += ROTATION_SPEED * dt;
		if (rotation > 360) rotation -= 360;
		else if (rotation < 0) rotation += 360;
		
		gameObject.transform.localEulerAngles = new Vector3(0, rotation, 0);
    }
}
