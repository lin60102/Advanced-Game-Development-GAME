

using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Rigidbody))]

public class BotFreeMovementMotor : BotMovementMotor {	

	public float walkingSpeed = 5.0f;
	public float walkingSnappyness = 50f;
	public float turningSmoothing = 0.3f; //0.3f
	
	public void FixedUpdate () {
	
		Vector3 targetVelocity = movementDirection * walkingSpeed;
		Vector3 deltaVelocity = targetVelocity - GetComponent<Rigidbody>().velocity;
		if (GetComponent<Rigidbody>().useGravity)
			deltaVelocity.y = 0f;
		GetComponent<Rigidbody>().AddForce (deltaVelocity * walkingSnappyness, ForceMode.Acceleration);
		
		
		Vector3 faceDir = facingDirection;
		if (faceDir == Vector3.zero)
			faceDir = movementDirection;
		
		
		if (faceDir == Vector3.zero) {
			GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
		}
		else {
			float rotationAngle = AngleAroundAxis (transform.forward, faceDir, Vector3.up);
			GetComponent<Rigidbody>().angularVelocity = (Vector3.up * rotationAngle * turningSmoothing);
		}
	}	
	
	
	private float AngleAroundAxis (Vector3 dirA, Vector3 dirB, Vector3 axis) {
	    
	    dirA = dirA - Vector3.Project (dirA, axis);
	    dirB = dirB - Vector3.Project (dirB, axis);
	   
	    
	    float angle = Vector3.Angle (dirA, dirB);
	   
	    
	    return angle * (Vector3.Dot (axis, Vector3.Cross (dirA, dirB)) < 0f ? -1f : 1f);
	}
}
