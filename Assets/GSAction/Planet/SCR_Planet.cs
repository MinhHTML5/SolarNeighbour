using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Vectrosity;

public class SCR_Planet : MonoBehaviour {
	public float MASS;
	public float ROTATION_SPEED;
	
	public Material orbitMaterial;
	
	public float distance;
	public float angle;
	public float speed;
	
	private float rotation = 0;
	private VectorLine orbitLine;
	
    private void Start() {
        
    }
	
	public void Init (float d, float a, float s) {
		distance = d;
		angle = a;
		speed = s;
		
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

    private void Update() {
        float dt = Time.deltaTime;
		rotation += ROTATION_SPEED * dt;
		if (rotation > 360) rotation -= 360;
		else if (rotation < 0) rotation += 360;
		gameObject.transform.localEulerAngles = new Vector3(0, rotation, 0);
		
		angle += speed * dt;
		if (angle > 360) angle -= 360;
		else if (angle < 0) angle += 360;
		gameObject.transform.position = new Vector3(distance * SCR_Helper.Sin(angle), 0, distance * SCR_Helper.Cos(angle));
    }
	
	private void DrawOrbit() {
		
	}
}
