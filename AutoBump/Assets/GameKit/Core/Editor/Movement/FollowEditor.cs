using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Follow))]
public class FollowEditor : Editor
{
	int toolBarTab;
	string currentTab;

	Follow myFollow;
	SerializedObject soTarget;

	private SerializedProperty _target;

	private SerializedProperty followOnXAxis;
	private SerializedProperty followOnYAxis;
	private SerializedProperty followOnZAxis;
	private SerializedProperty smoothTime;
	private SerializedProperty maxDistance;

	private SerializedProperty shareOrientation;
	private SerializedProperty rotateOnXAxis;
	private SerializedProperty rotateOnYAxis;
	private SerializedProperty rotateOnZAxis;
	private SerializedProperty rotateSpeed;

	private void OnEnable ()
	{
		myFollow = (Follow)target;
		soTarget = new SerializedObject(target);

		////

		_target = soTarget.FindProperty("target");

		followOnXAxis = soTarget.FindProperty("followOnXAxis");
		followOnYAxis = soTarget.FindProperty("followOnYAxis");
		followOnZAxis = soTarget.FindProperty("followOnZAxis");
		smoothTime = soTarget.FindProperty("smoothTime");
		maxDistance = soTarget.FindProperty("maxDistance");

		shareOrientation = soTarget.FindProperty("shareOrientation");
		rotateOnXAxis = soTarget.FindProperty("rotateOnXAxis");
		rotateOnYAxis = soTarget.FindProperty("rotateOnYAxis");
		rotateOnZAxis = soTarget.FindProperty("rotateOnZAxis");
		rotateSpeed = soTarget.FindProperty("rotateSpeed");
	}


	public override void OnInspectorGUI ()
	{
		UIHelper.InitializeStyles();

		soTarget.Update();
		EditorGUI.BeginChangeCheck();

		EditorGUILayout.Space();

		if(myFollow.target == null)
		{
			EditorGUILayout.BeginVertical(UIHelper.WarningStyle);
			{
				EditorGUILayout.PropertyField(_target);
			}
			EditorGUILayout.EndVertical();
		}
		else
		{
			EditorGUILayout.BeginVertical(UIHelper.MainStyle);
			{
				EditorGUILayout.PropertyField(_target);
			}
			EditorGUILayout.EndVertical();
		}

		EditorGUILayout.BeginVertical(UIHelper.MainStyle);
		{
			toolBarTab = GUILayout.Toolbar(toolBarTab, new string[] { "Position", "Direction" });

			currentTab = toolBarTab switch
			{
				0 => "Position",
				1 => "Direction",
				_ => currentTab
			};

			if (EditorGUI.EndChangeCheck())
			{
				soTarget.ApplyModifiedProperties();
				GUI.FocusControl(null);
			}

			EditorGUI.BeginChangeCheck();

			switch (currentTab)
			{
				case "Position":
				{
					EditorGUILayout.BeginVertical(UIHelper.MainStyle);
					{
						EditorGUILayout.BeginVertical(UIHelper.SubStyle1);
						{
							EditorGUILayout.PropertyField(followOnXAxis);
							EditorGUILayout.PropertyField(followOnYAxis);
							EditorGUILayout.PropertyField(followOnZAxis);
						}
						EditorGUILayout.EndVertical();

						EditorGUILayout.BeginVertical(UIHelper.SubStyle1);
						{
							EditorGUILayout.PropertyField(smoothTime);
							EditorGUILayout.PropertyField(maxDistance);
						}
						EditorGUILayout.EndVertical();
					}
					EditorGUILayout.EndVertical();
				}
				break;

				case "Direction":
				{
					EditorGUILayout.BeginVertical(UIHelper.MainStyle);
					{
						EditorGUILayout.PropertyField(shareOrientation);
						if (myFollow.shareOrientation)
						{
							EditorGUILayout.BeginVertical(UIHelper.SubStyle1);
							{
								EditorGUILayout.BeginVertical(UIHelper.SubStyle2);
								{
									EditorGUILayout.PropertyField(rotateOnXAxis);
									EditorGUILayout.PropertyField(rotateOnYAxis);
									EditorGUILayout.PropertyField(rotateOnZAxis);
								}
								EditorGUILayout.EndVertical();

								EditorGUILayout.BeginVertical(UIHelper.SubStyle2);
								{
									EditorGUILayout.PropertyField(rotateSpeed);
								}
								EditorGUILayout.EndVertical();
							}
							EditorGUILayout.EndVertical();
						}
					}
					EditorGUILayout.EndVertical();
				}
				break;
			}

		}
		EditorGUILayout.EndVertical();

		if (EditorGUI.EndChangeCheck())
		{
			soTarget.ApplyModifiedProperties();
		}

		if (myFollow.target == null)
		{
			EditorGUILayout.BeginVertical(UIHelper.WarningStyle);
			{
				EditorGUILayout.LabelField("No target set !", EditorStyles.boldLabel);
			}
			EditorGUILayout.EndVertical();
		}
	}
}