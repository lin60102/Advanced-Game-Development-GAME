using UnityEngine;
using UnityEditor;
using System.Collections;


[CustomEditor(typeof(SimpleFSM))]
public class SimpleFSMEditor : Editor {
	
	private SimpleFSM bot;
	
	void OnEnable()
	{
		bot = target as SimpleFSM;
	}
	
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI ();
	}
	
	void OnSceneGUI()
	{
		if(bot.showHearing)
		{
			Handles.color = new Color32 (0, 0, 255, 20);
			Handles.DrawSolidDisc (bot.transform.position, bot.transform.up, bot.hearingRange);
		}
		
		if(bot.showSight)
		{
			Handles.color = new Color32 (255, 0, 0, 20);
			Handles.DrawSolidArc (bot.transform.position, bot.transform.up, 
			                      Quaternion.Euler (0, -bot.sightAngle/2f, 0) * bot.transform.forward,
			                      bot.sightAngle, bot.sightDistance);
			
			Handles.color = Color.white;
			bot.sightDistance = Handles.ScaleValueHandle (bot.sightDistance, 
			                                                bot.transform.position + bot.transform.forward * bot.sightDistance, bot.transform.rotation,
			                                                5f, Handles.ArrowCap, 1f);
		}
		
	}
	
}
