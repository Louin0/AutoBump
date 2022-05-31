using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(SetAnimTriggerOnInput))]
public class SetAnimTriggerOnInputEditor : Editor
{
	[Tooltip("Used to check public variables from the target class")]
	SetAnimTriggerOnInput myObject;
	SerializedObject soTarget;

	private SerializedProperty playOnce;
	private SerializedProperty inputName;

	bool showTriggers = true;

	private void OnEnable ()
	{
		myObject = (SetAnimTriggerOnInput)target;
		soTarget = new SerializedObject(target);

		playOnce = soTarget.FindProperty("playOnce");
		inputName = soTarget.FindProperty("inputName");

	}

	void AddBool ()
	{
		myObject.triggers.Add(new SetAnimTriggerOnInput.TriggerInfo("", null));
	}

	void RemoveBool (int i)
	{
		myObject.triggers.RemoveAt(i);
	}

	public override void OnInspectorGUI ()
	{
		UIHelper.InitializeStyles();

		soTarget.Update();
		EditorGUI.BeginChangeCheck();

		EditorGUILayout.BeginVertical(UIHelper.MainStyle);
		{
			EditorGUILayout.PropertyField(inputName);
			EditorGUILayout.PropertyField(playOnce);
		}
		EditorGUILayout.EndVertical();

		EditorGUILayout.BeginVertical(UIHelper.MainStyle);
		{
			EditorGUILayout.BeginHorizontal();
			{
				if (!showTriggers)
				{
					if (GUILayout.Button(" Show Triggers (" + myObject.triggers.Count + ")", UIHelper.RedButtonStyle, GUILayout.MaxHeight(20f)))
					{
						showTriggers = true;
					}
				}
				else
				{
					if (GUILayout.Button(" Hide Triggers (" + myObject.triggers.Count + ")", UIHelper.GreenButtonStyle, GUILayout.MaxHeight(20f)))
					{
						showTriggers = false;
					}
				}
				if (GUILayout.Button(" Add Trigger ", UIHelper.GreenButtonStyle, GUILayout.MaxHeight(20f)))
				{
					AddBool();

					if (showTriggers == false)
					{
						showTriggers = true;
					}
				}
			}
			EditorGUILayout.EndHorizontal();


			if (showTriggers && myObject.triggers.Count > 0)
			{
				EditorGUILayout.BeginVertical(UIHelper.SubStyle1);
				{
					for (int i = 0; i < myObject.triggers.Count; i++)
					{
						EditorGUILayout.BeginHorizontal(UIHelper.SubStyle2);
						{
							EditorGUILayout.LabelField("Animator", GUILayout.MaxWidth(60f));
							myObject.triggers[i]._animator = (Animator)EditorGUILayout.ObjectField(myObject.triggers[i]._animator, typeof(Animator), true, GUILayout.MaxWidth(200f));
							EditorGUILayout.LabelField("Parameter Name", GUILayout.MaxWidth(100f));
							myObject.triggers[i]._triggerParameterName = EditorGUILayout.TextField(myObject.triggers[i]._triggerParameterName);


							if (GUILayout.Button("X", UIHelper.RedButtonStyle))
							{
								RemoveBool(i);
							}

						}
						EditorGUILayout.EndHorizontal();
					}
				}
				EditorGUILayout.EndVertical();
			}
		}
		EditorGUILayout.EndVertical();

		if (EditorGUI.EndChangeCheck())
		{
			soTarget.ApplyModifiedProperties();
		}
	}
}