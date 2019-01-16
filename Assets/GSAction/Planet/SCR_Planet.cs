using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Vectrosity;

public class SCR_Planet : MonoBehaviour {
	public float ROTATION_SPEED_MIN = -1.5f;
	public float ROTATION_SPEED_MAX = 1.5f;
	
	public Material orbitMaterial;
	
	public float mass;
	public float distance;
	public float angle;
	public float speed;
	
	private float rotation = 0;
	private float rotateSpeed = 0;
	private VectorLine orbitLine;
	
    private void Start() {
        
    }
	
	public void Init (int size, float d, float a, float s) {
		if (size == 1) 		gameObject.transform.localScale = new Vector3(3, 3, 3);
		else if (size == 2) gameObject.transform.localScale = new Vector3(6, 6, 6);
		else if (size == 3) gameObject.transform.localScale = new Vector3(9, 9, 9);
		
		distance = d;
		angle = a;
		speed = s;
		
		rotateSpeed = Random.Range (ROTATION_SPEED_MIN, ROTATION_SPEED_MAX);
		
		VectorManager.useDraw3D = true;
		
		List<Vector3> circlePoints = new List<Vector3>();
		for (int i=0; i<=360; i++) {
			float tempX = SCR_Helper.Sin(i) * distance;
			float tempY = SCR_Helper.Cos(i) * distance;
			circlePoints.Add (new Vector3(tempX, 0, tempY));
		}
		orbitLine = new VectorLine("VEC_Orbit", circlePoints, 1.0f);
		orbitLine.lineType = LineType.Continuous;
		orbitLine.material = orbitMaterial;
		orbitLine.Draw3DAuto();
	}
	
	public void UpdateAngle (float a) {
		// Only reupdate position if difference is too big
		if (Mathf.Abs(angle - a) > 1) {
			angle = a;
		}
	}

    private void Update() {
        float dt = Time.deltaTime;
		
		if (SCR_Action.instance.gameState == GameState.ACTION) {
			rotation += rotateSpeed * dt;
			if (rotation > 360) rotation -= 360;
			else if (rotation < 0) rotation += 360;
		
			angle += speed * dt;
			if (angle > 360) angle -= 360;
			else if (angle < 0) angle += 360;
		}
		
		gameObject.transform.localEulerAngles = new Vector3(0, rotation, 0);
		gameObject.transform.position = new Vector3(distance * SCR_Helper.Sin(angle), 0, distance * SCR_Helper.Cos(angle));
    }
}
