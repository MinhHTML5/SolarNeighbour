using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Camera : MonoBehaviour {
	public static SCR_Camera instance;
	
	public GameObject BTN_ViewHome;
	public GameObject BTN_ViewSun;
	
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
	
	public float OFFSET_AMOUNT;
	
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
	private float offsetSpeed			= 0;
	
	private float currentXAngle			= 30;
	private float currentYAngle			= 0;
	private float currentZDistance		= 200;
	private float currentOffset			= 0;
	
	private float targetXAngle			= 30;
	private float targetYAngle			= 0;
	private float targetZDistance		= 200;
	private float targetOffset			= 0;
	
	private bool  lockView				= false;
	private float previousXAngle		= 30;
	private float previousZDistance		= 200;
	
	
	


	private void Start() {
        instance = this;
    }
    private void Update() {
		float dt = Time.deltaTime;
		
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
		
		
		// Move offset
		float targetOSpeed = (targetOffset - currentOffset) * DISTANCE_MULTIPLIER;
		float offsetAcceleration = DISTANCE_ACCELERATION * dt;
		if (targetOSpeed > offsetSpeed + offsetAcceleration) 			offsetSpeed += offsetAcceleration;
		else if (targetOSpeed < offsetSpeed - offsetAcceleration) 		offsetSpeed -= offsetAcceleration;
		else															offsetSpeed = targetOSpeed;
		currentOffset += offsetSpeed * dt;
		
		
		
		
		// Apply actual transformation
		float flatDistance = SCR_Helper.Cos(currentXAngle) * currentZDistance;
		float y = SCR_Helper.Sin(currentXAngle) * currentZDistance;
		float x = SCR_Helper.Sin(currentYAngle) * flatDistance + centerX + SCR_Helper.Sin(currentYAngle - 90) * currentOffset;
		float z = SCR_Helper.Cos(currentYAngle) * flatDistance + centerZ + SCR_Helper.Cos(currentYAngle - 90) * currentOffset;
		
		gameObject.transform.position = new Vector3(x, y, z);
		gameObject.transform.localEulerAngles = new Vector3(currentXAngle, currentYAngle + 180, 0);
    }
	
	
	
	
	
	public void ViewHome() {
		if (SCR_Action.instance.homePlanet && !lockView) {
			viewHome = true;
			centerTargetX = SCR_Action.instance.homePlanet.transform.position.x;
			centerTargetZ = SCR_Action.instance.homePlanet.transform.position.z;
			
			BTN_ViewHome.SetActive (false);
			BTN_ViewSun.SetActive (true);
		}
	}
	public void ViewSun() {
		if (!lockView) {
			viewHome = false;
			centerTargetX = 0;
			centerTargetZ = 0;
			targetOffset = 0;
			BTN_ViewHome.SetActive (true);
			BTN_ViewSun.SetActive (false);
		}
	}
	public void TacticalView() {
		ViewSun();
		previousXAngle = targetXAngle;
		previousZDistance = targetZDistance;
		targetOffset = OFFSET_AMOUNT;
		targetXAngle = MAX_X_ANGLE;
		targetZDistance = MAX_DISTANCE;
		lockView = true;
	}
	public void CasualView() {
		targetOffset = 0;
		targetXAngle = previousXAngle;
		targetZDistance = previousZDistance;
		lockView = false;
	}

	public void Rotate(float deltaX, float deltaY) {
		if (!lockView) {
			targetYAngle += deltaX * MOUSE_X_SENSITIVITY;
			targetXAngle += deltaY * MOUSE_Y_SENSITIVITY;
			
			if (targetXAngle > MAX_X_ANGLE) targetXAngle = MAX_X_ANGLE;
			if (targetXAngle < MIN_X_ANGLE) targetXAngle = MIN_X_ANGLE;
		}
	}
	
	public void Zoom (float scroll) {
		if (!lockView) {
			targetZDistance += scroll * MOUSE_Z_SENSITIVITY;
			if (targetZDistance > MAX_DISTANCE) targetZDistance = MAX_DISTANCE;
			if (targetZDistance < MIN_DISTANCE) targetZDistance = MIN_DISTANCE;
		}
	}
}

