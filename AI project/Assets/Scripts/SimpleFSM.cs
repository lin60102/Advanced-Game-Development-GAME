// Copyright (c) 2014-2015, coAdjoint Ltd
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are met:
// 
// 1. Redistributions of source code must retain the above copyright notice,
// this list of conditions and the following disclaimer.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
// ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE
// LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
// CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
// SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
// INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN
// CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
// ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE
// POSSIBILITY OF SUCH DAMAGE.

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Simple FSM
/// 
/// Idea is to start long running and repetitive tasks when we enter a state.
/// Implement tasks using coroutines.
/// Problem: need to interrupt and kill these tasks when we change state!
/// Solution: Use coroutine manager (Prime31) to stop coroutines
/// Benefits over other FSM implementations: no complicated boolean logic to control states
/// - No problems cleaning up code when we enter a new state 
/// - Highly customisable logic when changing state
/// - Coroutines are used to perform tasks over many frames
/// 
/// </summary>

public class SimpleFSM : MonoBehaviour {
	
	private UnityEngine.AI.NavMeshAgent guard;	
	private Transform _transform;

	// States of the Guard implemented as Prime31 coroutine jobs
	private Job hear;
	private Job see;	
	private Job patrol;	
	private Job investigate;

	//Used to control rotation and movement of character
	private BotFreeMovementMotor guard_motor;

	private BotNavigation botNav;
	
	private float patrolSpeed = 3.5f;
	private float alertSpeed = 5.0f;

	[Range(0f,180f)]
	public float sightAngle = 45f;
	[Range(0f,30f)]
	public float sightDistance = 60f;

	[Range(0f,100f)]
	public float sightMaxHeight = 10f;

	[Range(0f,100f)]
	public float hearingRange = 10f;

	private float atPointRange = 2f;

	private float playerSpeed = 0f;
	
	private Vector3 playerPosition;
	
	private Vector3 suspiciousPosition;

	public bool showHearing;
	public bool showSight;

	public LayerMask raycastLayerMask;

	public Transform player;	
		
	//This is the current state of the Guard
	public enum State
	{
		Patrol, //= green
		Investigate, // = yellow
		Alert // = red
	}

	//Uses a property to fire enter and exit state methods when we enter a new state (and thus exit the previous one)
	public State _state;
	public State state
	{
		get
		{
			return _state;
		}
		set
		{
			ExitState(_state);
			_state = value;
			EnterState(_state);
		}
	}

	void Awake()
	{		
		guard = GetComponent<UnityEngine.AI.NavMeshAgent>();
		guard.updateRotation = false;
		
		guard_motor = GetComponent<BotFreeMovementMotor>();

		botNav = GetComponent<BotNavigation>();
		
		_transform = transform;

		//Explicitly set state so that we call the EnterState method
		state = State.Patrol;

		raycastLayerMask = ~raycastLayerMask;
	}

	/* In the enter state method we start long running jobs, such as performing a patrol route, listening for the player's movements and looking for the player.
	 * We use the Prime31 jobs, effectively wrappers for coroutines allowing us to pause and kill them, so that when we enter a new state we can cleanly exit
	   the previous state.
	 */
	void EnterState(State stateEntered)
	{
		switch(stateEntered)
		{
		case State.Patrol:
			
			SetColour(Color.green);

			//Starting the long running patrolling method.
			patrol = new Job(Patrolling(),true);

			//Here we start the long-running seeing method. When the player is seen, we run a lambda function (see below)
			see = new Job(Seeing(player, sightAngle,sightDistance,sightMaxHeight,0.5f,true,
			    /* The code below is known as a lambda function in C#. This are methods that don't have names and infer parameter types for their usage	
			     * at compile time. The lambda function below is called when the guard sees the player. Lambda functions are useful when 
			     * you want to run a custom section of code when another frequently used method finishes. 
                 */ 
				() => 
				{	
					Debug.Log ("Saw you!");
					
					state = State.Alert;
			
				}),true); 
			
			hear = new Job(Hearing(
				() =>
				{
					Debug.Log ("What was that?");
					
					state = State.Investigate;
			
				}),true);
			
			break;
			
		case State.Investigate:

			SetColour(Color.yellow);

			investigate = new Job(Investigating(),true);
			
			see = new Job(Seeing(player, sightAngle,sightDistance,sightMaxHeight,0.5f,true,
				() => 
				{	
					Debug.Log ("Saw you!");
					
					state = State.Alert;
			
				}),true);			
			
			break;
			
		case State.Alert:
			
			guard.speed = alertSpeed;

			SetColour(Color.red);

			see = new Job(Seeing(player, sightAngle,sightDistance,sightMaxHeight,0.5f,false,
				() => 
				{	
					Debug.Log ("Where you gone?");
					
					state = State.Investigate;
			
				}),true);				
			
			break;
		}
	}

	// In the ExitState method we kill all long running jobs when we exit a state. This ensures that we don't retain artefacts from previous running states 
	// (e.g. coroutines that haven't finished running).
	void ExitState(State stateExited)
	{
		switch(stateExited)
		{
		case State.Patrol:
			
			if(patrol != null) patrol.Kill();
			if(see != null) see.Kill();
			if(hear != null) hear.Kill();
			
			break;
		case State.Investigate:
			
			if(investigate != null) investigate.Kill();
			if(see != null) see.Kill();
			
			break;
		case State.Alert:
			
			if(see != null) see.Kill();
			
			break;
		}
	}	

	//Per-frame jobs such as updating the facing direction of the guard are still run in the update, but the method body is light and uncomplicated
	void Update()
	{		
		switch(state)
		{
		case State.Alert:
			
			guard.SetDestination(player.position);
			guard_motor.facingDirection = player.position - _transform.position;
			
			break;
		}
	}	

	//Used to calculate player velocity
	void LateUpdate()
	{		
		//Calculate player velocity
		Vector3 vel = (playerPosition - player.position)/Time.deltaTime;
		
		playerPosition = player.position;
		
		playerSpeed = vel.magnitude;
	}

	//The patrolling job. The BotNavigation class is used to manage the movement of the guard from waypoint to waypoint.
	//Note that the BotNavigation class has a nice visual editor, make sure you take a look!
	IEnumerator Patrolling()
	{
		while(true)
		{
			guard.speed = patrolSpeed;
			
			guard.SetDestination(botNav.CurrentPosition());
			
			while(!WithinRange(_transform.position, guard.destination))
			{
				guard_motor.facingDirection = botNav.CurrentPosition() - _transform.position;
				yield return null;
			}

			botNav.NextPosition();

			guard_motor.facingDirection = botNav.CurrentPosition() - _transform.position;
			
			guard.speed = 0f;
			
			yield return new WaitForSeconds(1f);			
		}
	}
	
	IEnumerator Investigating()
	{
		suspiciousPosition = player.position;
		
		while(true)
		{
			guard_motor.facingDirection = suspiciousPosition - _transform.position;
			
			guard.speed = 0f;
			
			yield return new WaitForSeconds(1f);			
			
			guard.SetDestination(suspiciousPosition);
			
			guard.speed = alertSpeed;
						
			while((_transform.position - guard.destination).sqrMagnitude > 2f)
			{
				guard_motor.facingDirection = guard.desiredVelocity;
				yield return null;
			}  
			
			guard.speed = 0f;
			
			yield return new WaitForSeconds(1f);

			// This coroutine uses the Navmesh to calculate the nearest waypoint to the bot. 
			// This should really be implemented as a Job, not a coroutine, as the guard's investigate state
			// could be interrupted. However, the coroutine will only run for 4 frames (the number of waypoints), so it's unlikely to happen.
			yield return StartCoroutine(botNav.ClosestWayPoint (guard, transform.position,
	        (res) =>
	        {
				//This is so we don't go backwards on our patrol route
				botNav.NextPosition();

				state = State.Patrol;				
			}));
		}
	}

	//The job responsible for seeing the player. The parameters are customisable in the editor
	// Note the Action paramter: actions are function function pointers allowing us to pass a reference to a function.
	// We can then run the function in the code. We use Actions to pass lambda functions to the coroutine to run when the player has been
	// seen.
	IEnumerator Seeing(Transform target, float angle, float distance, float maxHeight, float time, bool inRange, Action OnComplete)
	{		
		while(true)
		{		
			float timer = 0f;

			if(inRange)
			{
				while(IsInFov(target, angle, maxHeight) && (VisionCheck(target,distance)) && timer < time) 
				{			
					timer += Time.deltaTime;			
					yield return null;			
				}
			}
			else if(!inRange)
			{
				while((!IsInFov(target, angle, maxHeight) || !VisionCheck(target,distance)) && timer < time) 
				{			
					timer += Time.deltaTime;			
					yield return null;			
				}			
			}
			
			if(timer > time && OnComplete != null)
			{
				OnComplete(); 
			}

			yield return null;
		}
	}	
	
	IEnumerator Hearing(Action onComplete)
	{
		while(true)
		{			
			bool heardNoise = false;
			
			while(!heardNoise && (_transform.position - player.position).sqrMagnitude < hearingRange*hearingRange && playerSpeed > 20f)
			{
				heardNoise = true;
			}
				
			if(heardNoise && onComplete != null) onComplete();
			
			yield return null;
		}
	}	
	
	
	public bool VisionCheck(Transform target, float distance)
	{
		RaycastHit hit;

		if(Physics.Raycast(_transform.position, target.position-_transform.position,out hit,distance, raycastLayerMask.value))
		{
			if(hit.transform == player)
			{
				return true;
			}
			else return false;
		}
		else return false;
	}	
	
	public bool IsInFov(Transform target, float angle, float maxHeight)
	{
		var relPos = target.position - _transform.position; 
		float height = relPos.y;
		relPos.y = 0;
		
		if(Mathf.Abs(Vector3.Angle(relPos,transform.forward)) < angle)
		{
			if(Mathf.Abs(height) < maxHeight)
			{			
				return true;
			}
			else
			{				
				return false;
			}			
		}
		else return false;
	}	

	private void SetColour(Color colour)
	{
		_transform.GetComponent<Renderer>().material.color = colour;
		_transform.GetChild(0).GetComponent<Renderer>().material.color = colour;
	}

	private bool WithinRange(Vector3 a, Vector3 b, float d)
	{
		return (a-b).sqrMagnitude < d*d ? true : false;
	}

	private bool WithinRange(Vector3 a, Vector3 b)
	{
		return WithinRange (a, b, atPointRange);
	}
	
}
