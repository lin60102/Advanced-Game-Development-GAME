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

public class BotNavigation : MonoBehaviour {
	
	public bool showWayPoints = true;
	public bool showWayPointHandles = true;
	
	public Vector3[] wayPoints;
	
	private int _currentWayPoint = 0;
	private int currentWayPoint
	{
		get
		{
			return _currentWayPoint;
		}
		set
		{
			if(value < wayPoints.Length)
			{
				_currentWayPoint = value;
			}
			else
			{
				_currentWayPoint = 0;
			}
		}
	}
	
	public Vector3 NextPosition()
	{
		return wayPoints [currentWayPoint++];
	}
	
	public Vector3 CurrentPosition()
	{
		return wayPoints[currentWayPoint];
	}
	
	public IEnumerator ClosestWayPoint(UnityEngine.AI.NavMeshAgent bot, Vector3 pos, Action<Vector3> Result)
	{
		yield return StartCoroutine(FindNearestWayPoint (bot, pos));
		
		Result(wayPoints[currentWayPoint]);
	}

	//We use the navmesh to calculate the distance to each of the waypoints and selec the closest one
	IEnumerator FindNearestWayPoint(UnityEngine.AI.NavMeshAgent bot, Vector3 pos)
	{
		float minDist = float.MaxValue;
		
		for(int i=0; i< wayPoints.Length; ++i)
		{
			bot.SetDestination(wayPoints[i]);
			
			while(bot.pathPending || Mathf.Approximately(0f, bot.remainingDistance))
			{
				yield return new WaitForEndOfFrame();
			}
			
			if(bot.remainingDistance < minDist)
			{
				minDist = bot.remainingDistance;
				currentWayPoint = i;
			}
		}
	}
	
	
}
