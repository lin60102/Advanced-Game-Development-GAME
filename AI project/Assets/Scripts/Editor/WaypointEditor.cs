using UnityEngine;
using UnityEditor;
using System.Collections;

// This Editor visualises the way points of the guard. We can then move the waypoints as if they were transforms without having the overhead
// of using actual game objects to create waypoints.
[CanEditMultipleObjects]
[CustomEditor(typeof(BotNavigation))]
public class WaypointEditor : Editor {

	private BotNavigation botNav;

	void OnEnable()
	{
		botNav = target as BotNavigation;
	}

	public override void OnInspectorGUI()
	{
		serializedObject.Update ();

		EditorGUILayout.PropertyField(serializedObject.FindProperty("showWayPoints"));
		EditorGUILayout.PropertyField(serializedObject.FindProperty("showWayPointHandles"));
		EditorList.Show (serializedObject.FindProperty ("wayPoints"));
		serializedObject.ApplyModifiedProperties ();
	}

	void OnSceneGUI()
	{
		Handles.color = new Color32 (255, 125, 0, 255); 

		if(botNav.showWayPoints)
		{
			for(int i=0; i < botNav.wayPoints.Length; ++i)
			{
				if(botNav.showWayPointHandles)
				{
					Vector3 newPos = Handles.PositionHandle(botNav.wayPoints[i],Quaternion.identity);
					
					if(newPos != botNav.wayPoints[i])
					{
						Undo.RecordObject(botNav, "Move");
						
						botNav.wayPoints[i] = newPos;
					}
				}

				Handles.SphereCap(0, botNav.wayPoints[i],Quaternion.identity, 0.5f);

				//Handles.Label(botNav.wayPoints[i] + new Vector3(0f,3f,0f),(i+1).ToString() + ".", DemoResources.wayPointLabelStyle);

			}


		}
	}
}
