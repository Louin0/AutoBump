using UnityEngine;
using UnityEditor;

//Change all Dummyclass references into the class name
[CustomEditor(typeof(EnableComponentsWithTimer))]
public class EnableComponentsWithTimerEditor : Editor
{
	[Tooltip("Used to check public variables from the target class")]
	private int toolBarTab;

	private string currentTab;

	[Tooltip("Used to check public variables from the target class")]
	private EnableComponentsWithTimer myObject;

	private SerializedObject soTarget;

	private SerializedProperty timeBeforeEnable;
	private SerializedProperty disableAfter;
	private SerializedProperty timeBeforeDisable;
	private SerializedProperty loop;


	private bool showComponents = false;

	private void OnEnable()
	{
		myObject = (EnableComponentsWithTimer) target;
		soTarget = new SerializedObject(target);

		timeBeforeEnable = soTarget.FindProperty("timeBeforeEnable");
		disableAfter = soTarget.FindProperty("disableAfter");
		timeBeforeDisable = soTarget.FindProperty("timeBeforeDisable");
		loop = soTarget.FindProperty("loop");
	}

	public override void OnInspectorGUI()
	{
		UIHelper.InitializeStyles();

		soTarget.Update();
		EditorGUI.BeginChangeCheck();
		EditorGUILayout.BeginVertical(UIHelper.MainStyle);
		{
			EditorGUILayout.BeginHorizontal(UIHelper.SubStyle1);
			{
				toolBarTab = GUILayout.Toolbar(toolBarTab, new string[] {"General", "Components"},
					GUILayout.MinHeight(25));

				if (myObject.disableInstead)
				{
					if (GUILayout.Button("Disabling Components", UIHelper.RedButtonStyle, GUILayout.MaxHeight(20f)))
					{
						myObject.disableInstead = !myObject.disableInstead;
					}
				}
				else
				{
					if (GUILayout.Button("Enabling Components", UIHelper.GreenButtonStyle, GUILayout.MaxHeight(20f)))
					{
						myObject.disableInstead = !myObject.disableInstead;
					}
				}

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

			currentTab = toolBarTab switch
			{
				0 => "General",
				1 => "Components",
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
				case "General":
				{
					EditorGUILayout.BeginVertical(UIHelper.SubStyle1);
					{						
						EditorGUILayout.PropertyField(timeBeforeEnable);
						EditorGUILayout.PropertyField(disableAfter);

						if (myObject.disableAfter)
						{
							EditorGUILayout.BeginVertical(UIHelper.SubStyle2);
							{
								EditorGUILayout.PropertyField(timeBeforeDisable);
								EditorGUILayout.PropertyField(loop);
							}
							EditorGUILayout.EndVertical();
						}
					}
					EditorGUILayout.EndVertical();
				} 
					break;

				case "Components":
				{
					EditorGUILayout.BeginVertical(UIHelper.SubStyle1);
					{
						EditorGUILayout.BeginHorizontal();
						{
							if (!showComponents)
							{
								if (GUILayout.Button(" Show Components (" + myObject.components.Count + ")",
									    UIHelper.RedButtonStyle, GUILayout.MaxHeight(20f)))
								{
									showComponents = true;
								}
							}
							else
							{
								if (GUILayout.Button(" Hide Components (" + myObject.components.Count + ")",
									    UIHelper.GreenButtonStyle, GUILayout.MaxHeight(20f)))
								{
									showComponents = false;
								}
							}

							if (GUILayout.Button(" Add Component ", UIHelper.GreenButtonStyle,
								    GUILayout.MaxHeight(20f)))
							{
								AddComponent();

								if (showComponents == false)
								{
									showComponents = true;
								}
							}
						}
						EditorGUILayout.EndHorizontal();

						if (showComponents)
						{
							EditorGUILayout.BeginVertical(UIHelper.SubStyle1);
							{
								for (int i = 0; i < myObject.components.Count; i++)
								{
									EditorGUILayout.BeginHorizontal(UIHelper.SubStyle2);
									{
										myObject.components[i] =
											(Behaviour) EditorGUILayout.ObjectField(myObject.components[i],
												typeof(Behaviour), true, GUILayout.MaxWidth(200f));

										if (GUILayout.Button("X", UIHelper.RedButtonStyle))
										{
											RemoveComponent(i);
										}
									}
									EditorGUILayout.EndHorizontal();
								}
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
			}

			// Can be used to display contextual error messages

			#region DebugMessages

			if (myObject.displayDebugInfo)
			{
				if (myObject.components.Count == 0)
				{
					if (GUILayout.Button("No Components to Enable/Disable set ! Click here to add one",
						    UIHelper.RedButtonStyle))
					{
						AddComponent();
						showComponents = true;

						toolBarTab = 1;
						currentTab = "Components";
					}
				}
			}

			#endregion
		}
		EditorGUILayout.EndVertical();
	}

	private void AddComponent()
	{
		myObject.components.Add(null);
	}

	private void RemoveComponent(int index)
	{
		myObject.components.RemoveAt(index);

		if (myObject.components.Count == 0)
		{
			showComponents = false;
		}
	}
}