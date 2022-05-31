using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(SetAnimBoolOnInput))]
public class SetAnimBoolOnInputEditor : Editor
{
	[Tooltip("Used to check public variables from the target class")]
	SetAnimBoolOnInput myObject;
	SerializedObject soTarget;

	private SerializedProperty revertOn;
	private SerializedProperty inputName;
	private SerializedProperty bools;

	bool showBools = true;

	private void OnEnable ()
	{
		myObject = (SetAnimBoolOnInput)target;
		soTarget = new SerializedObject(target);

		revertOn = soTarget.FindProperty("revertOn");
		inputName = soTarget.FindProperty("inputName");
		bools = soTarget.FindProperty("bools");
	}

	void AddBool ()
	{
		myObject.bools.Add(new SetAnimBoolOnInput.BoolInfo("", true, null));
	}

	void RemoveBool (int i)
	{
		myObject.bools.RemoveAt(i);
	}

	public override void OnInspectorGUI ()
	{
		UIHelper.InitializeStyles();

		soTarget.Update();
		EditorGUI.BeginChangeCheck();

		EditorGUILayout.BeginVertical(UIHelper.MainStyle);
		{
			EditorGUILayout.PropertyField(inputName);
			EditorGUILayout.PropertyField(revertOn);
		}
		EditorGUILayout.EndVertical();

		EditorGUILayout.BeginVertical(UIHelper.MainStyle);
		{
			EditorGUILayout.BeginHorizontal();
			{
				if (!showBools)
				{
					if (GUILayout.Button(" Show Booleans (" + myObject.bools.Count + ")", UIHelper.RedButtonStyle, GUILayout.MaxHeight(20f)))
					{
						showBools = true;
					}
				}
				else
				{
					if (GUILayout.Button(" Hide Booleans (" + myObject.bools.Count + ")", UIHelper.GreenButtonStyle, GUILayout.MaxHeight(20f)))
					{
						showBools = false;
					}
				}
				if (GUILayout.Button(" Add Boolean ", UIHelper.GreenButtonStyle, GUILayout.MaxHeight(20f)))
				{
					AddBool();

					if (showBools == false)
					{
						showBools = true;
					}
				}
			}
			EditorGUILayout.EndHorizontal();


			if (showBools && myObject.bools.Count > 0)
			{
				EditorGUILayout.BeginVertical(UIHelper.SubStyle1);
				{
					for (int i = 0; i < myObject.bools.Count; i++)
					{
						EditorGUILayout.BeginHorizontal(UIHelper.SubStyle2);
						{
							EditorGUILayout.LabelField("Animator", GUILayout.MaxWidth(60f));
							myObject.bools[i].animator = (Animator)EditorGUILayout.ObjectField(myObject.bools[i].animator, typeof(Animator), true, GUILayout.MaxWidth(200f));
							EditorGUILayout.LabelField("Parameter Name", GUILayout.MaxWidth(100f));
							myObject.bools[i].boolParameterName = EditorGUILayout.TextField(myObject.bools[i].boolParameterName);
							EditorGUILayout.LabelField("Value", GUILayout.MaxWidth(60f));
							myObject.bools[i].valueSetOnInput = EditorGUILayout.Toggle(myObject.bools[i].valueSetOnInput);

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