using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Camera : MonoBehaviour {
	public float ROTATE_ACCELERATION;
	public float ROTATE_MULTIPLIER;
	public float DISTANCE_ACCELERATION;
	public float DISTANCE_MULTIPLIER;
	public float MOVE_ACCELERATION;
	public float MOVE_MULTIPLIER;

	public float MIN_DISTANCE;
	public float MAX_DISTANCE;
	
	public float MIN_X_ANGLE;
	public float MAX_X_ANGLE;
	
	public float MOUSE_X_SENSITIVITY;
	public float MOUSE_Y_SENSITIVITY;
	public float MOUSE_Z_SENSITIVITY;
	
	
	private bool  viewHome				= false;
	private float centerX				= 0;
	private float centerZ				= 0;
	private float centerSpeed			= 0;
	private float centerTargetX			= 0;
	private float centerTargetZ			= 0;
	
	private float rotateXSpeed			= 0;
	private float rotateYSpeed			= 0;
	private float distanceZSpeed		= 0;
	
	private float currentXAngle			= 30;
	private float currentYAngle			= 0;
	private float currentZDistance		= 200;
	
	private float targetXAngle			= 30;
	private float targetYAngle			= 0;
	private float targetZDistance		= 200;
	
	private bool  mouseDown				= false;
	private float mouseDownX			= 0;
	private float mouseDownY			= 0;
	
    private void Start() {
        
    }
	
	private void HandleKey() {
		if (Input.GetKeyDown(KeyCode.V)) {
			if (viewHome == false) {
				viewHome = true;
				centerTargetX = SCR_Action.instance.homePlanet.transform.position.x;
				centerTargetZ = SCR_Action.instance.homePlanet.transform.position.z;
			}
			else {
				viewHome = false;
				centerTargetX = 0;
				centerTargetZ = 0;
			}
		}
	}
	
	public void Rotate(float deltaX, float deltaY) {
		targetYAngle += deltaX * MOUSE_X_SENSITIVITY;
		targetXAngle += deltaY * MOUSE_Y_SENSITIVITY;
		
		if (targetXAngle > MAX_X_ANGLE) targetXAngle = MAX_X_ANGLE;
		if (targetXAngle < MIN_X_ANGLE) targetXAngle = MIN_X_ANGLE;
	}
	
	public void Zoom (float scroll) {
		targetZDistance += scroll * MOUSE_Z_SENSITIVITY;
		if (targetZDistance > MAX_DISTANCE) targetZDistance = MAX_DISTANCE;
		if (targetZDistance < MIN_DISTANCE) targetZDistance = MIN_DISTANCE;
	}
	

    private void Update() {
		float dt = Time.deltaTime;
		
		// Input command
		HandleKey();
		
		// Chase center point
		if (viewHome == true) {
			centerTargetX = SCR_Action.instance.homePlanet.transform.position.x;
			centerTargetZ = SCR_Action.instance.homePlanet.transform.position.z;
		}
        
		float centerDistance 		= SCR_Helper.DistanceBetweenTwoPoint (centerX, centerZ, centerTargetX, centerTargetZ);
		float centerAngle 			= SCR_Helper.AngleBetweenTwoPoint (centerX, centerZ, centerTargetX, centerTargetZ);
		float centerTargetSpeed 	= centerDistance * MOVE_MULTIPLIER;
		float centerAcceleration 	= MOVE_ACCELERATION * dt;
		if (centerTargetSpeed > centerSpeed + centerAcceleration) 		centerSpeed += centerAcceleration;
		else if (centerTargetSpeed < centerSpeed - centerAcceleration) 	centerSpeed -= centerAcceleration;
		else															centerSpeed = centerTargetSpeed;
		centerX += centerSpeed * SCR_Helper.Sin(centerAngle);
		centerZ += centerSpeed * SCR_Helper.Cos(centerAngle);
		
		
		// Movement around center point
		float targetYSpeed = (targetYAngle - currentYAngle) * ROTATE_MULTIPLIER;
		float rotateYAcceleration = ROTATE_ACCELERATION * dt;
		if (targetYSpeed > rotateYSpeed + rotateYAcceleration) 			rotateYSpeed += rotateYAcceleration;
		else if (targetYSpeed < rotateYSpeed - rotateYAcceleration) 	rotateYSpeed -= rotateYAcceleration;
		else															rotateYSpeed = targetYSpeed;
		currentYAngle += rotateYSpeed * dt;
		
		float targetXSpeed = (targetXAngle - currentXAngle) * ROTATE_MULTIPLIER;
		float rotateXAcceleration = ROTATE_ACCELERATION * dt;
		if (targetXSpeed > rotateXSpeed + rotateXAcceleration) 			rotateXSpeed += rotateXAcceleration;
		else if (targetXSpeed < rotateXSpeed - rotateXAcceleration) 	rotateXSpeed -= rotateXAcceleration;
		else															rotateXSpeed = targetXSpeed;
		currentXAngle += rotateXSpeed * dt;
		
		
		// Get near to the center point
		float targetZSpeed = (targetZDistance - currentZDistance) * DISTANCE_MULTIPLIER;
		float distanceZAcceleration = DISTANCE_ACCELERATION * dt;
		
		if (targetZSpeed > distanceZSpeed + distanceZAcceleration) 		distanceZSpeed += distanceZAcceleration;
		else if (targetZSpeed < distanceZSpeed - distanceZAcceleration) distanceZSpeed -= distanceZAcceleration;
		else															distanceZSpeed = targetZSpeed;
		currentZDistance += distanceZSpeed * dt;
		
		
		
		
		// Apply actual transformation
		float flatDistance = SCR_Helper.Cos(currentXAngle) * currentZDistance;
		float y = SCR_Helper.Sin(currentXAngle) * currentZDistance;
		float x = SCR_Helper.Sin(currentYAngle) * flatDistance + centerX;
		float z = SCR_Helper.Cos(currentYAngle) * flatDistance + centerZ;
		
		gameObject.transform.position = new Vector3(x, y, z);
		gameObject.transform.localEulerAngles = new Vector3(currentXAngle, currentYAngle + 180, 0);
    }
}

