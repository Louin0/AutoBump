using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(FirstPersonCamera))]
public class FirstPersonCameraEditor : Editor
{
	[Tooltip("Used to check public variables from the target class")]
	private int toolBarTab;
	private string currentTab;
	
	[Tooltip("Used to check public variables from the target class")]
	private FirstPersonCamera myObject;
	private SerializedObject soTarget;

	private SerializedProperty transformToRotateWithCamera;
	private SerializedProperty mouseXInputName;
	private SerializedProperty mouseYInputName;
	private SerializedProperty mouseSensitivityX;
	private SerializedProperty mouseSensitivityY;
	private SerializedProperty minXAxisClamp;
	private SerializedProperty maxXAxisClamp;

	private void OnEnable ()
	{
		myObject = (FirstPersonCamera)target;
		soTarget = new SerializedObject(target);

		transformToRotateWithCamera = soTarget.FindProperty("transformToRotateWithCamera");
		
		mouseXInputName = soTarget.FindProperty("mouseXInputName");
		mouseYInputName = soTarget.FindProperty("mouseYInputName");
		
		mouseSensitivityX = soTarget.FindProperty("mouseSensitivityX");
		mouseSensitivityY = soTarget.FindProperty("mouseSensitivityY");
		
		minXAxisClamp = soTarget.FindProperty("minXAxisClamp");
		maxXAxisClamp = soTarget.FindProperty("maxXAxisClamp");

	}

	public override void OnInspectorGUI ()
	{
		UIHelper.InitializeStyles();

		soTarget.Update();
		EditorGUI.BeginChangeCheck();

		EditorGUILayout.BeginVertical(UIHelper.MainStyle);
		{
			EditorGUILayout.PropertyField(transformToRotateWithCamera);
		}
		EditorGUILayout.EndVertical();

		EditorGUILayout.BeginVertical(UIHelper.MainStyle);
		{
			EditorGUILayout.BeginHorizontal(UIHelper.SubStyle1);
			{
				toolBarTab = GUILayout.Toolbar(toolBarTab, new string[] { "Input", "Sensitivity", "X Angle Clamp", "Curve" }, GUILayout.MinHeight(25));

				if (myObject.displayDebugInfo)
				{
					if (GUILayout.Button("Debug ON", UIHelper.GreenButtonStyle, GUILayout.MaxHeight(20f)))
					{
						myObject.displayDebugInfo = !myObject.displayDebugInfo;
					}
				}
				else
				{
					if (GUILayout.Button("Debug OFF", UIHelper.RedButtonStyle, GUILayout.MaxHeight(20f)))
					{
						myObject.displayDebugInfo = !myObject.displayDebugInfo;
					}
				}
			}
			EditorGUILayout.EndHorizontal();
		}
		EditorGUILayout.EndVertical();

		currentTab = toolBarTab switch
		{
			0 => "Input",
			1 => "Sensitivity",
			2 => "X Angle Clamp",
			3 => "Curve",
			_ => currentTab
		};
		
		//Apply modified properties to avoid data loss
		if (EditorGUI.EndChangeCheck())
		{
			soTarget.ApplyModifiedProperties();
			GUI.FocusControl(null);
		}
		
		EditorGUI.BeginChangeCheck();

		switch (currentTab)
		{
			case "Input":
				EditorGUILayout.BeginVertical(UIHelper.SubStyle1);
			{
				EditorGUILayout.PropertyField(mouseXInputName);
				EditorGUILayout.PropertyField(mouseYInputName);
			}
				EditorGUILayout.EndVertical();
				break;
			
			case "Sensitivity":
				EditorGUILayout.BeginVertical(UIHelper.SubStyle1);
			{
				EditorGUILayout.PropertyField(mouseSensitivityX);
				EditorGUILayout.PropertyField(mouseSensitivityY);
			}
			EditorGUILayout.EndVertical();
				break;

			case "X Angle Clamp":
			EditorGUILayout.BeginHorizontal(UIHelper.SubStyle1);
			{
				EditorGUILayout.LabelField("X Angle Clamp : ", GUILayout.MaxWidth(50f));

				myObject.minXAxisClamp = EditorGUILayout.FloatField(myObject.minXAxisClamp, GUILayout.MaxWidth(50f));
				EditorGUILayout.MinMaxSlider(ref myObject.minXAxisClamp, ref myObject.maxXAxisClamp, -90f, 90f);
				myObject.maxXAxisClamp = EditorGUILayout.FloatField(myObject.maxXAxisClamp, GUILayout.MaxWidth(50f));
			}
			EditorGUILayout.EndHorizontal();
			break;
			
			case "Curve":
				break;
		}
		
		
		if (EditorGUI.EndChangeCheck())
		{
			soTarget.ApplyModifiedProperties();
		}
	}
}
