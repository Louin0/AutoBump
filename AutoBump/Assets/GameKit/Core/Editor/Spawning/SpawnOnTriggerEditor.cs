using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SpawnOnTrigger))]
public class SpawnOnTriggerEditor : Editor
{
	int toolBarTab;
	string currentTab;

	SpawnOnTrigger myObject;
	SerializedObject soTarget;

	private SerializedProperty useTagOnTrigger;
	private SerializedProperty onlyOnce;

	private SerializedProperty shareOrientation;
	private SerializedProperty prefabToSpawn;
	private SerializedProperty randomMinOffset;
	private SerializedProperty randomMaxOffset;

	private SerializedProperty spawnInsideParent;
	private SerializedProperty spawnInsideCollidingObject;

	private SerializedProperty requireResources;
	private SerializedProperty resourceManager;
	private SerializedProperty resourceIndex;
	private SerializedProperty resourceCostOnUse;

	private SerializedProperty requireInput;
	private SerializedProperty inputName;

	private SerializedProperty parent;

	private void OnEnable ()
	{
		myObject = (SpawnOnTrigger)target;
		soTarget = new SerializedObject(target);

		////
		useTagOnTrigger = soTarget.FindProperty("useTagOnTrigger");
		onlyOnce = soTarget.FindProperty("onlyOnce");

		shareOrientation = soTarget.FindProperty("shareOrientation");
		prefabToSpawn = soTarget.FindProperty("prefabToSpawn");
		randomMinOffset = soTarget.FindProperty("randomMinOffset");
		randomMaxOffset = soTarget.FindProperty("randomMaxOffset");

		////

		spawnInsideParent = soTarget.FindProperty("spawnInsideParent");
		spawnInsideCollidingObject = soTarget.FindProperty("spawnInsideCollidingObject");
		parent = soTarget.FindProperty("parent");

		////

		requireResources = soTarget.FindProperty("requireResources");
		resourceManager = soTarget.FindProperty("resourceManager");
		resourceIndex = soTarget.FindProperty("resourceIndex");
		resourceCostOnUse = soTarget.FindProperty("resourceCostOnUse");

		requireInput = soTarget.FindProperty("requireInput");
		inputName = soTarget.FindProperty("inputName");

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

		EditorGUILayout.BeginHorizontal(UIHelper.MainStyle);
		{
			toolBarTab = GUILayout.Toolbar(toolBarTab, new string[] { "Collision", "Spawning", "Nested Spawning", "Resources", "Input" }, GUILayout.MinHeight(25));
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
			currentTab = "Collision";
			break;

			case 1:
			currentTab = "Spawning";
			break;

			case 2:
			currentTab = "Nested Spawning";
			break;

			case 3:
			currentTab = "Resources";
			break;

			case 4:
			currentTab = "Input";
			break;
		}

		if (EditorGUI.EndChangeCheck())
		{
			soTarget.ApplyModifiedProperties();
			GUI.FocusControl(null);
		}

		EditorGUI.BeginChangeCheck();

		switch (currentTab)
		{
			case "Collision":
				{
					EditorGUILayout.BeginVertical(UIHelper.MainStyle);
					{
						EditorGUILayout.PropertyField(onlyOnce);

						EditorGUILayout.PropertyField(useTagOnTrigger);
						if (myObject.useTagOnTrigger)
						{
							EditorGUILayout.BeginVertical(UIHelper.SubStyle1);
							{
								myObject.tagName = EditorGUILayout.TagField("Tag :", myObject.tagName);
							}
							EditorGUILayout.EndVertical();
						}
					}
					EditorGUILayout.EndVertical();
				}
			break;

			case "Spawning":
				{
					EditorGUILayout.BeginVertical(UIHelper.MainStyle);
					{
						EditorGUILayout.PropertyField(prefabToSpawn);

						EditorGUILayout.LabelField("Random Offset");
						EditorGUILayout.BeginHorizontal(UIHelper.MainStyle);
						{
							EditorGUILayout.LabelField("X : ", GUILayout.MaxWidth(40f));
							myObject.randomMinOffset.x = EditorGUILayout.FloatField(myObject.randomMinOffset.x, GUILayout.MaxWidth(50f));
							EditorGUILayout.MinMaxSlider(ref myObject.randomMinOffset.x, ref myObject.randomMaxOffset.x, -100f, 100f, GUILayout.MinWidth(100f));
							myObject.randomMaxOffset.x = EditorGUILayout.FloatField(myObject.randomMaxOffset.x, GUILayout.MaxWidth(50f));
						}
						EditorGUILayout.EndHorizontal();

						EditorGUILayout.BeginHorizontal(UIHelper.MainStyle);
						{
							EditorGUILayout.LabelField("Y : ", GUILayout.MaxWidth(40f));
							myObject.randomMinOffset.y = EditorGUILayout.FloatField(myObject.randomMinOffset.y, GUILayout.MaxWidth(50f));
							EditorGUILayout.MinMaxSlider(ref myObject.randomMinOffset.y, ref myObject.randomMaxOffset.y, -100f, 100f, GUILayout.MinWidth(100f));
							myObject.randomMaxOffset.y = EditorGUILayout.FloatField(myObject.randomMaxOffset.y, GUILayout.MaxWidth(50f));
						}
						EditorGUILayout.EndHorizontal();

						EditorGUILayout.BeginHorizontal(UIHelper.MainStyle);
						{
							EditorGUILayout.LabelField("Z : ", GUILayout.MaxWidth(40f));
							myObject.randomMinOffset.z = EditorGUILayout.FloatField(myObject.randomMinOffset.z, GUILayout.MaxWidth(50f));
							EditorGUILayout.MinMaxSlider(ref myObject.randomMinOffset.z, ref myObject.randomMaxOffset.z, -100f, 100f, GUILayout.MinWidth(100f));
							myObject.randomMaxOffset.z = EditorGUILayout.FloatField(myObject.randomMaxOffset.z, GUILayout.MaxWidth(50f));
						}
						EditorGUILayout.EndHorizontal();

						EditorGUILayout.PropertyField(shareOrientation);
					}
					EditorGUILayout.EndVertical();
				}
			break;

			case "Nested Spawning":
				{
					EditorGUILayout.BeginVertical(UIHelper.MainStyle);
					{
						EditorGUILayout.PropertyField(spawnInsideParent);
						if (myObject.spawnInsideParent)
						{
							EditorGUILayout.BeginVertical(UIHelper.SubStyle1);
							{
								EditorGUILayout.PropertyField(spawnInsideCollidingObject);
								if (myObject.spawnInsideCollidingObject == false)
								{
									EditorGUILayout.PropertyField(parent);
								}
							}
							EditorGUILayout.EndVertical();
						}

					}
					EditorGUILayout.EndVertical();
				}
			break;

			case "Resources":
				{
					EditorGUILayout.BeginVertical(UIHelper.MainStyle);
					{
						EditorGUILayout.PropertyField(requireResources);
						if (myObject.requireResources)
						{
							EditorGUILayout.BeginVertical(UIHelper.SubStyle1);
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
					EditorGUILayout.BeginVertical(UIHelper.MainStyle);
					{
						EditorGUILayout.PropertyField(requireInput);

						if (myObject.requireInput)
						{
							EditorGUILayout.BeginVertical(UIHelper.SubStyle1);
							{
								EditorGUILayout.PropertyField(inputName);
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

		EditorGUILayout.Space();

		if(myObject.displayDebugInfo)
		{
			if (myObject.prefabToSpawn.Length == 0 || myObject.prefabToSpawn[0] == null)
			{
				EditorGUILayout.BeginVertical(UIHelper.WarningStyle);
				{
					EditorGUILayout.LabelField("No Prefab to spawn set !! Please add one", EditorStyles.boldLabel);
				}
				EditorGUILayout.EndVertical();
			}

			if(myObject.requireInput && myObject.inputName == "")
			{
				EditorGUILayout.BeginVertical(UIHelper.WarningStyle);
				{
					EditorGUILayout.LabelField("No Input name set ! Either add one or disable Require Input", EditorStyles.boldLabel);
				}
				EditorGUILayout.EndVertical();
			}
		}
	}
}
