using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Vectrosity;

public class SCR_Planet : MonoBehaviour {
	public GameObject[] PFB_Planet;
	
	public const float 	ROTATION_SPEED_MIN = 5.0f;
	public const float 	ROTATION_SPEED_MAX = 20.0f;
	public const float 	MARKING_RADIUS = 15.0f;
	
	public Material 	orbitMaterial;
	public Material		orbitMaterialHighlight;
	public Material		orbitMaterialEnemy;
	public Material		orbitMaterialMe;
	
	public float		x;
	public float		z;
	public int			playerID;
	public float 		mass;
	public float 		distance;
	public float 		angle;
	public float 		speed;
	
	private float 		rotation = 0;
	private float 		rotateSpeed = 0;
	
	
	private float 		orbitSkipAngle;
	private GameObject	planet;
	private VectorLine 	orbitLine;
	private VectorLine 	markingLine;
	
	
    private void Start() {
        playerID = -1;
    }
	
	public void Init (int id, int size, float d, float a, float s) {
		distance = d;
		angle = a;
		speed = s;
		
		planet = Instantiate (PFB_Planet[id]);
		planet.transform.SetParent (transform);
		planet.transform.position = new Vector3 (0, 0, distance);
		
		if (size == 1) 		planet.transform.localScale = new Vector3(3, 3, 3);
		else if (size == 2) planet.transform.localScale = new Vector3(6, 6, 6);
		else if (size == 3) planet.transform.localScale = new Vector3(9, 9, 9);
		
		rotateSpeed = Random.Range (ROTATION_SPEED_MIN, ROTATION_SPEED_MAX);
		
		VectorManager.useDraw3D = true;
		
		List<Vector3> orbitPoints = new List<Vector3>();
		orbitSkipAngle = Mathf.Atan(MARKING_RADIUS / distance) * SCR_Helper.RAD_TO_DEG;
		float tempX = SCR_Helper.Sin(orbitSkipAngle) * distance;
		float tempZ = SCR_Helper.Cos(orbitSkipAngle) * distance;
		orbitPoints.Add (new Vector3(tempX, 0, tempZ));
		for (int i=Mathf.CeilToInt(orbitSkipAngle); i<=360-Mathf.CeilToInt(orbitSkipAngle); i++) {
			tempX = SCR_Helper.Sin(i) * distance;
			tempZ = SCR_Helper.Cos(i) * distance;
			orbitPoints.Add (new Vector3(tempX, 0, tempZ));
		}
		tempX = SCR_Helper.Sin(-orbitSkipAngle) * distance;
		tempZ = SCR_Helper.Cos(-orbitSkipAngle) * distance;
		orbitPoints.Add (new Vector3(tempX, 0, tempZ));
		
		orbitLine = new VectorLine("VEC_Orbit", orbitPoints, 1.0f);
		orbitLine.lineType = LineType.Continuous;
		orbitLine.material = orbitMaterial;
		orbitLine.Draw3DAuto();
		
		/*
		List<Vector3> markingPoints = new List<Vector3>();
		for (int i=0; i<=180; i++) {
			tempX = SCR_Helper.Sin(i * 2) * MARKING_RADIUS;
			tempZ = SCR_Helper.Cos(i * 2) * MARKING_RADIUS + distance;
			markingPoints.Add (new Vector3(tempX, 0, tempZ));
		}
		markingLine = new VectorLine("VEC_Marking", markingPoints, 1.0f);
		markingLine.lineType = LineType.Continuous;
		markingLine.material = orbitMaterial;
		*/
	}
	
	public void UpdateAngle (float a) {
		// Only reupdate position if difference is too big
		if (Mathf.Abs(angle - a) > 1) {
			angle = a;
		}
	}
	
	public Vector3 GetPosition () {
		return new Vector3(x, 0 ,z);
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
		
		x = SCR_Helper.Sin (angle) * distance;
		z = SCR_Helper.Cos (angle) * distance;
		
		planet.transform.localEulerAngles = new Vector3(0, rotation, 0);
		gameObject.transform.localEulerAngles = new Vector3(0, angle, 0);
		
		orbitLine.drawTransform = transform;
		//markingLine.drawTransform = transform;
    }
	
	
	public void HighlightOrbit(int type) {
		if (type == 0) {
			orbitLine.material = orbitMaterial;
			//markingLine.material = orbitMaterial;
		}
		else if (type == 1) {
			orbitLine.material = orbitMaterialHighlight;
			//markingLine.material = orbitMaterialHighlight;
		}
		else if (type == 2) {
			orbitLine.material = orbitMaterialEnemy;
			//markingLine.material = orbitMaterialEnemy;
		}
		else if (type == 3) {
			orbitLine.material = orbitMaterialMe;
			//markingLine.material = orbitMaterialMe;
		}
	}
}
