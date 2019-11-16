//========= Copyright 2017, Sam Tague, All rights reserved. ===================
//
// VRSmooth allows for smooth locomotion, it adds a NavMeshAgent to the vr rig root
// when it initializes. Make sure you've baked navigation
//
//===================Contact Email: Sam@MassGames.co.uk===========================
#if VRInteraction
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRInteraction;
using UnityEngine.AI;

namespace VRArcTeleporter
{
	public class VRSmooth : MonoBehaviour 
	{    
		public enum MoveSetup
		{
			MOVE,
			TURN
		}

		public MoveSetup moveSetup;
		public float runSpeed = 20f;
		public float turnAngle = 8f;
		public float turnSpeed = 0.2f;
		public AudioClip turnSound;
		public Transform vrRigRoot;

		private VRInteractor _interactor;
		private NavMeshAgent _agent;

		private bool _rotating;
		private bool _leftRotate;
		private float t;
		private float rotationTime;
		private float startTime;

		void OnEnable()
		{
			_interactor = GetComponent<VRInteractor>();
			if (_interactor == null)
			{
				Debug.LogError("VRSmooth requires the VRInteractor script", gameObject);
				return;
			}
			if (vrRigRoot == null) vrRigRoot = _interactor.GetVRRigRoot;
			_agent = vrRigRoot.GetComponent<NavMeshAgent>();
			if (_agent == null) _agent = vrRigRoot.gameObject.AddComponent<NavMeshAgent>();
			_agent.enabled = true;
			_agent.updatePosition = false;
		}

		void OnDisable()
		{
			_agent = vrRigRoot.GetComponent<NavMeshAgent>();
			if (_agent != null) _agent.enabled = false;
		}

		void FixedUpdate() 
		{
			if (_rotating)
			{
				float elapsedTime = Time.time - startTime;
				t = elapsedTime / rotationTime;
				vrRigRoot.RotateAround(Camera.main.transform.position, Vector3.up, (_leftRotate?-t:t)*(turnAngle*0.75f));
				if (t >= 1f) _rotating = false;
			}

			if (_interactor.vrInput.hmdType == VRInput.HMDType.VIVE)
			{
				switch(moveSetup)
				{
				case MoveSetup.MOVE:
					if (_interactor.vrInput.PadPressed) Move();
					break;
				case MoveSetup.TURN:
					if (_interactor.vrInput.PadPressed)
					{
						if ((!_rotating || _leftRotate) && _interactor.vrInput.PadPosition.x > 0)
						{
							StartRotate(false);
						} else if (!_rotating || !_leftRotate)
						{
							StartRotate(true);
						}
					}
					break;
				}
			} else
			{
				switch(moveSetup)
				{
				case MoveSetup.MOVE:
					Move();
					break;
				case MoveSetup.TURN:
					if ((!_rotating || _leftRotate) && _interactor.vrInput.PadPosition.x > 0.1f)
					{
						StartRotate(false);
					} else if ((!_rotating || !_leftRotate) && _interactor.vrInput.PadPosition.x < -0.1f)
					{
						StartRotate(true);
					}
					break;
				}
			}
		}

		private void StartRotate(bool left)
		{
			_rotating = true;
			_leftRotate = left;
			if (turnSound != null) AudioSource.PlayClipAtPoint(turnSound, transform.position);
			t = 0;
			rotationTime = turnSpeed;
			startTime = Time.time;
		}

		private void Move() 
		{
			Vector2 padPos = _interactor.vrInput.PadPosition;
			if (padPos == Vector2.zero) return;

			Vector3 moveDir = new Vector3(padPos.x, 0, padPos.y);
			moveDir = (Camera.main.transform.rotation * moveDir) * (runSpeed*0.01f);

			_agent.nextPosition = transform.position;
			_agent.baseOffset = transform.position.y - _agent.transform.position.y;
			if (_agent.isOnNavMesh)
			{
				Vector3 rigCamOffset = transform.position - _agent.transform.position;
				_agent.Move(moveDir * runSpeed * Time.deltaTime);
				_agent.transform.position = _agent.nextPosition - rigCamOffset;
			}
		}
	}
}
#endif
