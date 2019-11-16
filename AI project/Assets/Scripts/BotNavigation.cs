
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
