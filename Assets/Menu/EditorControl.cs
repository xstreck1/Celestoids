using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(ResetScript))]
public class EditorControl : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		
		ResetScript myScript = (ResetScript)target;
		if(GUILayout.Button("Reset stored data"))
		{
			myScript.Reset();
		}
	}
}