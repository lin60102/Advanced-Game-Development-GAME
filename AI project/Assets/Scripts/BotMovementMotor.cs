using UnityEngine;
using System.Collections;

public class BotMovementMotor : MonoBehaviour {

	[HideInInspector]
	public Vector3 movementDirection;
	
	[HideInInspector]
	public Vector3 facingDirection;
	
	[HideInInspector]
	public Vector3 movementTarget;
}
