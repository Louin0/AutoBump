using UnityEngine;
using UnityEditor;

//Change all Dummyclass references into the class name
[CustomEditor(typeof(EnableGameObjectsOnDestroyAll))]
public class EnableGameObjectsOnDestroyAllEditor : Editor
{
	[Tooltip("Used to check public variables from the target class")]
	private int toolBarTab;

	private string currentTab;

	[Tooltip("Used to check public variables from the target class")]
	private EnableGameObjectsOnDestroyAll myObject;

	private SerializedObject soTarget;

	private SerializedProperty gameObjectsToDetect;
	private SerializedProperty searchByTag;


	private bool showComponents = false;

	private void OnEnable()
	{
		myObject = (EnableGameObjectsOnDestroyAll) target;
		soTarget = new SerializedObject(target);

		gameObjectsToDetect = soTarget.FindProperty("gameObjectsToDetect");
		searchByTag = soTarget.FindProperty("searchByTag");


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
				toolBarTab = GUILayout.Toolbar(toolBarTab, new string[] {"Targets", "GameObjects"},
					GUILayout.MinHeight(25));

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

			currentTab = toolBarTab switch
			{
				0 => "Targets",
				1 => "GameObjects",
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
				case "Targets":
				{
					EditorGUILayout.BeginVertical(UIHelper.SubStyle1);
					{						
						EditorGUILayout.BeginVertical(UIHelper.SubStyle2);
						{
							EditorGUILayout.PropertyField(searchByTag);
						}
						EditorGUILayout.EndVertical();
						
						EditorGUILayout.BeginVertical(UIHelper.SubStyle2);
						{
							if (myObject.searchByTag)
							{
								myObject.tagName = EditorGUILayout.TagField("Tag", myObject.tagName);
							}
							else
							{
								EditorGUILayout.PropertyField(gameObjectsToDetect);
							}
						}
						EditorGUILayout.EndVertical();
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
								if (GUILayout.Button(" Show Components (" + myObject.gameObjectsToEnable.Count + ")",
									    UIHelper.RedButtonStyle, GUILayout.MaxHeight(20f)))
								{
									showComponents = true;
								}
							}
							else
							{
								if (GUILayout.Button(" Hide Components (" + myObject.gameObjectsToEnable.Count + ")",
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
		}
		EditorGUILayout.EndVertical();
	}

	private void AddComponent()
	{
		myObject.gameObjectsToEnable.Add(null);
	}

	private void RemoveComponent(int index)
	{
		myObject.gameObjectsToEnable.RemoveAt(index);

		if (myObject.gameObjectsToEnable.Count == 0)
		{
			showComponents = false;
		}
	}
}