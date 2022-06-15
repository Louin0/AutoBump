using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Bumper))]
public class BumperEditor : Editor
{
	[Tooltip("Used to check public variables from the target class")]
	int toolBarTab;
	string currentTab;

	Bumper myObject;
	SerializedObject soTarget;

	private SerializedProperty bumpForce;
	private SerializedProperty bumpTowardsOther;
	private SerializedProperty additionalForceTowardsOther;
	private SerializedProperty preventInputHolding;

	private SerializedProperty useTag;

	private void OnEnable ()
	{
		myObject = (Bumper)target;
		soTarget = new SerializedObject(target);

		////

		bumpForce = soTarget.FindProperty("bumpForce");
		bumpTowardsOther = soTarget.FindProperty("bumpTowardsOther");
		additionalForceTowardsOther = soTarget.FindProperty("additionalForceTowardsOther");
		preventInputHolding = soTarget.FindProperty("preventInputHolding");

		useTag = soTarget.FindProperty("useTag");
	}

	public override void OnInspectorGUI ()
	{
		//Initializing Custom GUI Styles
		if (!UIHelper.IsUIInitialized)
		{
			UIHelper.InitializeStyles();
			UIHelper.IsUIInitialized = true;
		}

		soTarget.Update();
		EditorGUI.BeginChangeCheck();

		EditorGUILayout.BeginHorizontal(UIHelper.SubStyle1);
		{
			toolBarTab = GUILayout.Toolbar(toolBarTab, new string[] { "Forces", "Tag"}, GUILayout.MinHeight(25));

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

		switch (toolBarTab)
		{
			case 0:
			currentTab = "Forces";
			break;

			case 1:
			currentTab = "Tag";
			break;
		}

		//Apply modified properties to avoid data loss
		if (EditorGUI.EndChangeCheck())
		{
			soTarget.ApplyModifiedProperties();
			GUI.FocusControl(null);
		}

		EditorGUI.BeginChangeCheck();

		switch (currentTab)
		{
			case "Forces":
				{
					EditorGUILayout.BeginVertical(UIHelper.SubStyle1);
					{
						EditorGUILayout.PropertyField(bumpForce);
						EditorGUILayout.PropertyField(bumpTowardsOther);

						if (myObject.bumpTowardsOther)
						{
							EditorGUILayout.BeginVertical(UIHelper.SubStyle2);
							{
								EditorGUILayout.PropertyField(additionalForceTowardsOther);
							}
							EditorGUILayout.EndVertical();
						}
						EditorGUILayout.PropertyField(preventInputHolding);
					}
					EditorGUILayout.EndVertical();
				}
			break;

			case "Tag":
				{
					EditorGUILayout.BeginVertical(UIHelper.SubStyle1);
					{
						EditorGUILayout.PropertyField(useTag);

						if (myObject.useTag)
						{
							EditorGUILayout.BeginVertical(UIHelper.SubStyle2);
							{
								myObject.tagName = EditorGUILayout.TagField("Tag :", myObject.tagName);
							}
							EditorGUILayout.EndVertical();
						}
					}
					EditorGUILayout.EndVertical();
				}
			break;
		}

		if (EditorGUI.EndChangeCheck())
		{
			soTarget.ApplyModifiedProperties();
			//GUI.FocusControl(GUI.GetNameOfFocusedControl());
		}

		EditorGUILayout.Space();

	}
}
