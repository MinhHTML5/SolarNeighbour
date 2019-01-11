using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Camera : MonoBehaviour {
	public float ROTATE_ACCELERATION;
	public float ROTATE_DECELERATION;
	public float ROTATE_MAX_SPEED;
	
	public float MOVE_ACCELERATION;
	public float MOVE_DECELERATION;
	public float MOVE_MAX_SPEED;
	public float MIN_DISTANCE;
	public float MAX_DISTANCE;
	
	public float MIN_X_ANGLE;
	public float MAX_X_ANGLE;
	
	
	
	private float currentXSpeed			= 0;
	private float currentYSpeed			= 0;
	private float currentZSpeed			= 0;
	
	private float currentXAngle			= 30;
	private float currentYAngle			= 0;
	private float currentDistance		= 200;
	
    private void Start() {
        
    }
	
	private void HandleKey() {
		float dt = Time.deltaTime;
		
		if (Input.GetKey(KeyCode.A)) {
			currentYSpeed += ROTATE_ACCELERATION * dt;
			if (currentYSpeed > ROTATE_MAX_SPEED) currentYSpeed = ROTATE_MAX_SPEED;
		}
		else {
			if (currentYSpeed > 0) {
				currentYSpeed -= ROTATE_DECELERATION * dt;
				if (currentYSpeed < 0) currentYSpeed = 0;
			}
		}
		if (Input.GetKey(KeyCode.D)) {
			currentYSpeed -= ROTATE_ACCELERATION * dt;
			if (currentYSpeed < -ROTATE_MAX_SPEED) currentYSpeed = -ROTATE_MAX_SPEED;
		}
		else {
			if (currentYSpeed < 0) {
				currentYSpeed += ROTATE_DECELERATION * dt;
				if (currentYSpeed > 0) currentYSpeed = 0;
			}
		}
		
		if (Input.GetKey(KeyCode.R)) {
			currentXSpeed += ROTATE_ACCELERATION * dt;
			if (currentXSpeed > ROTATE_MAX_SPEED) currentXSpeed = ROTATE_MAX_SPEED;
		}
		else {
			if (currentXSpeed > 0) {
				currentXSpeed -= ROTATE_DECELERATION * dt;
				if (currentXSpeed < 0) currentXSpeed = 0;
			}
		}
		if (Input.GetKey(KeyCode.F)) {
			currentXSpeed -= ROTATE_ACCELERATION * dt;
			if (currentXSpeed < -ROTATE_MAX_SPEED) currentXSpeed = -ROTATE_MAX_SPEED;
		}
		else {
			if (currentXSpeed < 0) {
				currentXSpeed += ROTATE_DECELERATION * dt;
				if (currentXSpeed > 0) currentXSpeed = 0;
			}
		}
		
		if (Input.GetKey(KeyCode.S)) {
			currentZSpeed += MOVE_ACCELERATION * dt;
			if (currentZSpeed > MOVE_MAX_SPEED) currentZSpeed = MOVE_MAX_SPEED;
		}
		else {
			if (currentZSpeed > 0) {
				currentZSpeed -= MOVE_DECELERATION * dt;
				if (currentZSpeed < 0) currentZSpeed = 0;
			}
		}
		if (Input.GetKey(KeyCode.W)) {
			currentZSpeed -= MOVE_ACCELERATION * dt;
			if (currentZSpeed < -MOVE_MAX_SPEED) currentZSpeed = -MOVE_MAX_SPEED;
		}
		else {
			if (currentZSpeed < 0) {
				currentZSpeed += MOVE_DECELERATION * dt;
				if (currentZSpeed > 0) currentZSpeed = 0;
			}
		}
	}

    private void Update() {
        float dt = Time.deltaTime;
		
		HandleKey();
		
		currentYAngle += currentYSpeed * dt;
		if (currentYAngle > 360) currentYAngle -= 360;
		if (currentYAngle < 0) currentYAngle += 360;
		
		currentXAngle += currentXSpeed * dt;
		if (currentXAngle < MIN_X_ANGLE) {
			currentXAngle = MIN_X_ANGLE;
			currentXSpeed = 0;
		}
		else if (currentXAngle > MAX_X_ANGLE) {
			currentXAngle = MAX_X_ANGLE;
			currentXSpeed = 0;
		}
		
		currentDistance += currentZSpeed * dt;
		if (currentDistance < MIN_DISTANCE) {
			currentDistance = MIN_DISTANCE;
			currentZSpeed = 0;
		}
		else if (currentDistance > MAX_DISTANCE) {
			currentDistance = MAX_DISTANCE;
			currentZSpeed = 0;
		}
		
		float flatDistance = SCR_Helper.Cos(currentXAngle) * currentDistance;
		float y = SCR_Helper.Sin(currentXAngle) * currentDistance;
		float x = SCR_Helper.Sin(currentYAngle) * flatDistance;
		float z = SCR_Helper.Cos(currentYAngle) * flatDistance;
		
		gameObject.transform.position = new Vector3(x, y, z);
		gameObject.transform.localEulerAngles = new Vector3(currentXAngle, currentYAngle + 180, 0);
    }
}

