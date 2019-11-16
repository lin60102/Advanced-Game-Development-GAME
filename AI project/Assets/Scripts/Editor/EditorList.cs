using UnityEngine;
using UnityEditor;
using System.Collections;

public static class EditorList {

	public static void Show(SerializedProperty list)
	{
		if(!list.isArray)
		{
			EditorGUILayout.HelpBox(list.name + " is not an array nor a list!", MessageType.Error);
			return;
		}

		EditorGUILayout.PropertyField (list);
		EditorGUI.indentLevel += 1;

		if(list.isExpanded)
		{
			ShowElements(list);
		}
	}

	private static GUIContent
		moveButtonContent = new GUIContent("\u21b4", "move down"),
		duplicateButtonContent = new GUIContent("+", "duplicate"),
		deleteButtonContent = new GUIContent("-", "delete"),
		addButtonContent = new GUIContent("+", "add element");
	
	private static void ShowElements (SerializedProperty list) {

		for (int i = 0; i < list.arraySize; i++) {

			EditorGUILayout.BeginHorizontal();
			{

				EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i));

				ShowButtons(list, i);
			
			} EditorGUILayout.EndHorizontal();

		}

		if (list.arraySize == 0 && GUILayout.Button(addButtonContent, EditorStyles.miniButton)) {
			list.arraySize += 1;
		}
	}
	
	private static GUILayoutOption miniButtonWidth = GUILayout.Width(20f);
	
	private static void ShowButtons (SerializedProperty list, int index) {
		if (GUILayout.Button(moveButtonContent, EditorStyles.miniButtonLeft, miniButtonWidth)) {
			list.MoveArrayElement(index, index + 1);
		}
		if (GUILayout.Button(duplicateButtonContent, EditorStyles.miniButtonMid, miniButtonWidth)) {
			list.InsertArrayElementAtIndex(index);
		}
		if (GUILayout.Button(deleteButtonContent, EditorStyles.miniButtonRight, miniButtonWidth)) {
			int oldSize = list.arraySize;
			list.DeleteArrayElementAtIndex(index);
			if (list.arraySize == oldSize) {
				list.DeleteArrayElementAtIndex(index);
			}
		}
	}
}
