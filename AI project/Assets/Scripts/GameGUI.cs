

using UnityEngine;
using System.Collections;

public class GameGUI : MonoBehaviour {

	void OnGUI()
	{
		GUILayout.Label("Use W,A,S and D to control your sphere");
		GUILayout.Label("Warning: can go through walls");
	}
}
