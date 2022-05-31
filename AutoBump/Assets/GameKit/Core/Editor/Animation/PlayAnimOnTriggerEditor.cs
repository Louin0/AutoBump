using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayAnimOnTrigger))]
public class PlayAnimOnTriggerEditor : Editor
{

	PlayAnimOnTrigger myObject;
	SerializedObject soTarget;

	private SerializedProperty animator;
	private SerializedProperty triggerName;
	private SerializedProperty useTag;
	private SerializedProperty tagName;
	private SerializedProperty triggerOnce;

	private void OnEnable ()
	{
		myObject = (PlayAnimOnTrigger)target;
		soTarget = new SerializedObject(target);

		////

		animator = soTarget.FindProperty("animator");
		triggerName = soTarget.FindProperty("triggerName");
		useTag = soTarget.FindProperty("useTag");
		tagName = soTarget.FindProperty("tagName");
		triggerOnce = soTarget.FindProperty("triggerOnce");
	}

	public override void OnInspectorGUI ()
	{
		//Initializing Custom GUI Styles
		UIHelper.InitializeStyles();

		soTarget.Update();
		EditorGUI.BeginChangeCheck();

		EditorGUILayout.LabelField("Collision", EditorStyles.boldLabel);

		EditorGUILayout.BeginVertical(UIHelper.MainStyle);
		{
			EditorGUILayout.PropertyField(animator);
			if(myObject.animator != null)
			{
				EditorGUILayout.BeginVertical(UIHelper.SubStyle1);
				{
					EditorGUILayout.PropertyField(triggerName);
					EditorGUILayout.PropertyField(triggerOnce);
				}
				EditorGUILayout.EndVertical();

				EditorGUILayout.BeginVertical(UIHelper.SubStyle1);
				{
					EditorGUILayout.PropertyField(useTag);

					if(myObject.useTag)
					{
						EditorGUILayout.BeginVertical(UIHelper.SubStyle2);
						{
							EditorGUILayout.PropertyField(tagName);
						}
						EditorGUILayout.EndVertical();
					}
				}
				EditorGUILayout.EndVertical();
			}
		}
		EditorGUILayout.EndVertical();

		

		if (EditorGUI.EndChangeCheck())
		{
			soTarget.ApplyModifiedProperties();
			//GUI.FocusControl(GUI.GetNameOfFocusedControl());
		}

		EditorGUILayout.Space();

	}
}
