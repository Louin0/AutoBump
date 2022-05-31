using UnityEngine;
using UnityEditor;

//Change all Dummyclass references into the class name
[CustomEditor(typeof(EnableGameObjectsOnTrigger))]
public class EnableGameObjectsOnTriggerEditor : Editor
{
	[Tooltip("Used to check public variables from the target class")]
	private int toolBarTab;

	private string currentTab;

	[Tooltip("Used to check public variables from the target class")]
	private EnableGameObjectsOnTrigger myObject;

	private SerializedObject soTarget;

	private SerializedProperty useTag;
	private SerializedProperty revertOn;
	private SerializedProperty revertAfterCooldown;
	private SerializedProperty requireResources;
	private SerializedProperty resourceManager;
	private SerializedProperty resourceIndex;
	private SerializedProperty resourceCostOnUse;
	private SerializedProperty requireInput;
	private SerializedProperty inputName;


	private bool showComponents = false;

	private void OnEnable()
	{
		myObject = (EnableGameObjectsOnTrigger) target;
		soTarget = new SerializedObject(target);

		useTag = soTarget.FindProperty("useTag");
		revertOn = soTarget.FindProperty("revertOn");
		revertAfterCooldown = soTarget.FindProperty("revertAfterCooldown");
		requireResources = soTarget.FindProperty("requireResources");
		resourceManager = soTarget.FindProperty("resourceManager");
		resourceIndex = soTarget.FindProperty("resourceIndex");
		resourceCostOnUse = soTarget.FindProperty("resourceCostOnUse");
		requireInput = soTarget.FindProperty("requireInput");
		inputName = soTarget.FindProperty("inputName");

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
				toolBarTab = GUILayout.Toolbar(toolBarTab, new string[] {"General", "GameObjects", "Resources", "Input"},
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
				0 => "General",
				1 => "GameObjects",
				2 => "Resources",
				3 => "Input",
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
					EditorGUILayout.BeginHorizontal(UIHelper.SubStyle1);
					{						
						EditorGUILayout.PropertyField(useTag);

						if (myObject.useTag)
						{
							EditorGUILayout.BeginVertical(UIHelper.SubStyle2);
							{
								myObject.tagName = EditorGUILayout.TagField((myObject.tagName));
							}
							EditorGUILayout.EndVertical();
						}
					}
					EditorGUILayout.EndHorizontal();
					
					EditorGUILayout.BeginVertical(UIHelper.SubStyle1);
					{	
						EditorGUILayout.PropertyField(revertOn);

						if (myObject.revertOn == EnableGameObjectsOnTrigger.RevertOn.Timer)
						{
							EditorGUILayout.PropertyField(revertAfterCooldown);
						}
					}
					EditorGUILayout.EndVertical();
				} 
					break;
				
				case "Resources":
				{
					EditorGUILayout.BeginVertical(UIHelper.SubStyle1);
					{						
						EditorGUILayout.PropertyField(requireResources);

						if (myObject.requireResources)
						{
							EditorGUILayout.BeginVertical(UIHelper.SubStyle2);
							{
								EditorGUILayout.PropertyField(resourceManager);
								EditorGUILayout.PropertyField(resourceIndex);
								EditorGUILayout.PropertyField(resourceCostOnUse);
							}
							EditorGUILayout.EndVertical();
						}
					}
					EditorGUILayout.EndVertical();
				} 
					break;

				case "Input":
				{
					EditorGUILayout.BeginVertical(UIHelper.SubStyle1);
					{						
						EditorGUILayout.PropertyField(requireInput);

						if (myObject.requireInput)
						{
							EditorGUILayout.BeginVertical(UIHelper.SubStyle2);
							{
								EditorGUILayout.PropertyField(inputName);
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
								if (GUILayout.Button(" Show GameObjects (" + myObject.gameObjectToEnable.Count + ")",
									    UIHelper.RedButtonStyle, GUILayout.MaxHeight(20f)))
								{
									showComponents = true;
								}
							}
							else
							{
								if (GUILayout.Button(" Hide GameObjects (" + myObject.gameObjectToEnable.Count + ")",
									    UIHelper.GreenButtonStyle, GUILayout.MaxHeight(20f)))
								{
									showComponents = false;
								}
							}

							if (GUILayout.Button(" Add GameObject ", UIHelper.GreenButtonStyle,
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
								for (int i = 0; i < myObject.gameObjectToEnable.Count; i++)
								{
									EditorGUILayout.BeginHorizontal(UIHelper.SubStyle2);
									{
										myObject.gameObjectToEnable[i] =
											(GameObject) EditorGUILayout.ObjectField(myObject.gameObjectToEnable[i],
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
				if (myObject.gameObjectToEnable.Count == 0)
				{
					if (GUILayout.Button("No GameObjects to Enable/Disable set ! Click here to add one",
						    UIHelper.RedButtonStyle))
					{
						AddComponent();
						showComponents = true;

						toolBarTab = 1;
						currentTab = "GameObjects";
					}
				}
			}

			#endregion
		}
		EditorGUILayout.EndVertical();
	}

	private void AddComponent()
	{
		myObject.gameObjectToEnable.Add(null);
	}

	private void RemoveComponent(int index)
	{
		myObject.gameObjectToEnable.RemoveAt(index);

		if (myObject.gameObjectToEnable.Count == 0)
		{
			showComponents = false;
		}
	}
}