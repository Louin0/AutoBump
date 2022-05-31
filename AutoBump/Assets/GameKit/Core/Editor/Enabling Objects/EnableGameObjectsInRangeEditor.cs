using UnityEngine;
using UnityEditor;

//Change all Dummyclass references into the class name
[CustomEditor(typeof(EnableGameObjectsInRange))]
public class EnableGameObjectsInRangeEditor : Editor
{
[Tooltip("Used to check public variables from the target class")]
	private int toolBarTab;
	private string currentTab;

	[Tooltip("Used to check public variables from the target class")]
	private EnableGameObjectsInRange myObject;
	private SerializedObject soTarget;

	private SerializedProperty range;
	private SerializedProperty specificTarget;

	private SerializedProperty layerMask;
	private SerializedProperty revertWhenOutOfRange;
	private SerializedProperty findWithTag;

	private bool showComponents = false;
	private void OnEnable ()
	{
		myObject = (EnableGameObjectsInRange)target;
		soTarget = new SerializedObject(target);

		range = soTarget.FindProperty("range");
		specificTarget = soTarget.FindProperty("specificTarget");


		findWithTag = soTarget.FindProperty("findWithTag");
		layerMask = soTarget.FindProperty("layerMask");
		revertWhenOutOfRange = soTarget.FindProperty("revertWhenOutOfRange");
	}

	public override void OnInspectorGUI ()
	{
		UIHelper.InitializeStyles();

		soTarget.Update();
		EditorGUI.BeginChangeCheck();
		EditorGUILayout.BeginVertical(UIHelper.MainStyle);
		{
			EditorGUILayout.BeginHorizontal(UIHelper.SubStyle1);
			{
				toolBarTab = GUILayout.Toolbar(toolBarTab, new string[] { "General", "Targets", "GameObjects"}, GUILayout.MinHeight(25));
				
				if (myObject.disableInstead)
				{
					if (GUILayout.Button("Set Active False", UIHelper.RedButtonStyle, GUILayout.MaxHeight(20f)))
					{
						myObject.disableInstead = !myObject.disableInstead;
					}
				}
				else
				{
					if (GUILayout.Button("Set Active True", UIHelper.GreenButtonStyle, GUILayout.MaxHeight(20f)))
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
		}
		EditorGUILayout.EndVertical();

		EditorGUILayout.BeginVertical(UIHelper.MainStyle);
		{
			currentTab = toolBarTab switch
			{
				0 => "General",
				1 => "Targets",
				2 => "GameObjects",
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
						EditorGUILayout.PropertyField(range);
						EditorGUILayout.PropertyField(revertWhenOutOfRange);
					}
					EditorGUILayout.EndVertical();
				}
				break;

				case "Targets":
				{
						EditorGUILayout.BeginVertical(UIHelper.SubStyle1);
						{
							if (myObject.findWithTag)
							{
								if (GUILayout.Button("Check Specific Target", UIHelper.ButtonStyle))
								{
									myObject.findWithTag = false;
								}
								
								EditorGUILayout.BeginVertical(UIHelper.SubStyle2);
								{
									myObject.targetTagName = EditorGUILayout.TagField("Target Tag ", myObject.targetTagName);
									EditorGUILayout.PropertyField(layerMask);
								}
								EditorGUILayout.EndVertical();
							}
							else
							{			
								if (GUILayout.Button("Check From Tag", UIHelper.ButtonStyle))
								{
									myObject.findWithTag = true;
								}
								
								EditorGUILayout.BeginVertical(UIHelper.SubStyle2);
								{
									EditorGUILayout.PropertyField(specificTarget);								
								}
								EditorGUILayout.EndVertical();
							}
						}
						EditorGUILayout.EndVertical();
				}
				break;

				case "GameObjects":
				{
					EditorGUILayout.BeginVertical(UIHelper.SubStyle1);
					{
						EditorGUILayout.BeginHorizontal();
						{
							if (!showComponents)
							{
								if (GUILayout.Button(" Show GameObjects (" + myObject.gameObjectsToEnable.Count + ")",
									    UIHelper.RedButtonStyle, GUILayout.MaxHeight(20f)))
								{
									showComponents = true;
								}
							}
							else
							{
								if (GUILayout.Button(" Hide GameObjects (" + myObject.gameObjectsToEnable.Count + ")",
									    UIHelper.GreenButtonStyle, GUILayout.MaxHeight(20f)))
								{
									showComponents = false;
								}
							}

							if (GUILayout.Button(" Add GameObject ", UIHelper.GreenButtonStyle, GUILayout.MaxHeight(20f)))
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
								for (int i = 0; i < myObject.gameObjectsToEnable.Count; i++)
								{
									EditorGUILayout.BeginHorizontal(UIHelper.SubStyle2);
									{
										myObject.gameObjectsToEnable[i] =
											(GameObject) EditorGUILayout.ObjectField(myObject.gameObjectsToEnable[i],
												typeof(GameObject), true, GUILayout.MaxWidth(200f));
									
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
				if (myObject.gameObjectsToEnable.Count == 0)
				{
					if (GUILayout.Button("No GameObjects to Enable/Disable set ! Click here to add one", UIHelper.RedButtonStyle))
					{
						AddComponent();
						showComponents = true;
						
						toolBarTab = 2;
						currentTab = "Components";
					}
				}
				
				if (myObject.specificTarget == null && myObject.targetTagName == "")
				{
					if (GUILayout.Button("Either set a specific target or a tag to look for", UIHelper.RedButtonStyle))
					{
						toolBarTab = 1;
						currentTab = "Targets";
					}
				}
			}
			#endregion
		}
		EditorGUILayout.EndVertical();
	}
	
	private void AddComponent ()
	{
		myObject.gameObjectsToEnable.Add(null);
	}

	private void RemoveComponent (int index)
	{
		myObject.gameObjectsToEnable.RemoveAt(index);

		if (myObject.gameObjectsToEnable.Count == 0)
		{
			showComponents = false;
		}
	}
}
