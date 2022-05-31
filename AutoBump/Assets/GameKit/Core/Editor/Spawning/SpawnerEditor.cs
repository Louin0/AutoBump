using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(Spawner))]

public class SpawnerEditor : Editor
{
	[Tooltip("Used to check public variables from the target class")]
	int toolBarTab;
	string currentTab;

	Spawner myObject;
	SerializedObject soTarget;

	List<Spawner.PrefabToSpawn> prefabsToSpawn;
	private SerializedProperty randomMinOffset;
	private SerializedProperty randomMaxOffset;
	private SerializedProperty shareOrientation;

	private SerializedProperty useCooldown;
	private SerializedProperty spawnCooldown;

	private SerializedProperty useInput;
	private SerializedProperty spawnInputName;

	private SerializedProperty spawnInsideParent;
	private SerializedProperty parent;

	private SerializedProperty requireResources;
	private SerializedProperty resourceManager;
	private SerializedProperty resourceIndex;
	private SerializedProperty resourceCostOnUse;

	private SerializedProperty destroyOnNoResourceLeft;

	bool showSpawns = false;

	private void OnEnable ()
	{
		myObject = (Spawner)target;
		soTarget = new SerializedObject(target);

		prefabsToSpawn = myObject.prefabToSpawn;

		shareOrientation = soTarget.FindProperty("shareOrientation");
		randomMinOffset = soTarget.FindProperty("randomMinOffset");
		randomMaxOffset = soTarget.FindProperty("randomMaxOffset");

		useCooldown = soTarget.FindProperty("useCooldown");
		spawnCooldown = soTarget.FindProperty("spawnCooldown");

		useInput = soTarget.FindProperty("useInput");
		spawnInputName = soTarget.FindProperty("spawnInputName");

		spawnInsideParent = soTarget.FindProperty("spawnInsideParent");
		parent = soTarget.FindProperty("parent");

		requireResources = soTarget.FindProperty("requireResources");
		resourceManager = soTarget.FindProperty("resourceManager");
		resourceIndex = soTarget.FindProperty("resourceIndex");
		resourceCostOnUse = soTarget.FindProperty("resourceCostOnUse");
		destroyOnNoResourceLeft = soTarget.FindProperty("destroyOnNoResourceLeft");

	}

	public override void OnInspectorGUI ()
	{
		//Initializing Custom GUI Styles
		UIHelper.InitializeStyles();

		soTarget.Update();
		EditorGUI.BeginChangeCheck();


		EditorGUILayout.BeginVertical(UIHelper.MainStyle);
		{
			EditorGUILayout.BeginHorizontal(UIHelper.SubStyle1);
			{
				toolBarTab = GUILayout.Toolbar(toolBarTab, new string[] { "Spawn", "Cooldown", "Input", "Nested Spawning", "Resources" }, GUILayout.MinHeight(25));

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
				0 => "Spawn",
				1 => "Cooldown",
				2 => "Input",
				3 => "Nested Spawning",
				4 => "Resources",
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
				case "Spawn":
				{
					EditorGUILayout.BeginVertical(UIHelper.MainStyle);
					{

						EditorGUILayout.Space(5);
						EditorGUILayout.BeginHorizontal();
						{
							if (!showSpawns)
							{
								if (GUILayout.Button(" Show Prefabs (" + myObject.prefabToSpawn.Count + ")",
									    UIHelper.RedButtonStyle, GUILayout.MaxHeight(20f)))
								{
									showSpawns = true;
								}
							}
							else
							{
								if (GUILayout.Button(" Hide Prefabs (" + myObject.prefabToSpawn.Count + ")",
									    UIHelper.GreenButtonStyle, GUILayout.MaxHeight(20f)))
								{
									showSpawns = false;
								}
							}

							if (GUILayout.Button(" Add Prefab ", UIHelper.GreenButtonStyle, GUILayout.MaxHeight(20f)))
							{
								AddSpawn();

								if (showSpawns == false)
								{
									showSpawns = true;
								}
							}

							if (GUILayout.Button(" Spawn Prefab ", UIHelper.GreenButtonStyle, GUILayout.MaxHeight(20f)))
							{
								myObject.SpawnObject();
								if (myObject.prefabToSpawn.Count == 0)
								{
									showSpawns = false;
								}
							}
						}
						EditorGUILayout.EndHorizontal();
						EditorGUILayout.Space(5);

						if (showSpawns)
						{
							EditorGUILayout.BeginVertical(UIHelper.SubStyle1);
							{
								for (int i = 0; i < prefabsToSpawn.Count; i++)
								{
									EditorGUILayout.BeginHorizontal(UIHelper.SubStyle2);
									{
										prefabsToSpawn[i].prefab =
											(GameObject) EditorGUILayout.ObjectField(myObject.prefabToSpawn[i].prefab,
												typeof(GameObject), false, GUILayout.MaxWidth(200f));
										EditorGUILayout.LabelField("Weight : ", GUILayout.MaxWidth(40f));
										prefabsToSpawn[i].weight =
											EditorGUILayout.IntField(myObject.prefabToSpawn[i].weight,
												GUILayout.MaxWidth(40f));

										if (GUILayout.Button("X", UIHelper.RedButtonStyle))
										{
											RemoveSpawn(i);
										}

									}
									EditorGUILayout.EndHorizontal();
								}
							}
							EditorGUILayout.EndVertical();
						}

						EditorGUILayout.BeginVertical(UIHelper.MainStyle);
						{
							EditorGUILayout.LabelField("Random Offset");
							EditorGUILayout.BeginHorizontal(UIHelper.MainStyle);
							{
								EditorGUILayout.LabelField("X : ", GUILayout.MaxWidth(40f));
								myObject.randomMinOffset.x = EditorGUILayout.FloatField(myObject.randomMinOffset.x,
									GUILayout.MaxWidth(50f));
								EditorGUILayout.MinMaxSlider(ref myObject.randomMinOffset.x,
									ref myObject.randomMaxOffset.x, -100f, 100f, GUILayout.MinWidth(100f));
								myObject.randomMaxOffset.x = EditorGUILayout.FloatField(myObject.randomMaxOffset.x,
									GUILayout.MaxWidth(50f));
							}
							EditorGUILayout.EndHorizontal();

							EditorGUILayout.BeginHorizontal(UIHelper.MainStyle);
							{
								EditorGUILayout.LabelField("Y : ", GUILayout.MaxWidth(40f));
								myObject.randomMinOffset.y = EditorGUILayout.FloatField(myObject.randomMinOffset.y,
									GUILayout.MaxWidth(50f));
								EditorGUILayout.MinMaxSlider(ref myObject.randomMinOffset.y,
									ref myObject.randomMaxOffset.y, -100f, 100f, GUILayout.MinWidth(100f));
								myObject.randomMaxOffset.y = EditorGUILayout.FloatField(myObject.randomMaxOffset.y,
									GUILayout.MaxWidth(50f));
							}
							EditorGUILayout.EndHorizontal();

							EditorGUILayout.BeginHorizontal(UIHelper.MainStyle);
							{
								EditorGUILayout.LabelField("Z : ", GUILayout.MaxWidth(40f));
								myObject.randomMinOffset.z = EditorGUILayout.FloatField(myObject.randomMinOffset.z,
									GUILayout.MaxWidth(50f));
								EditorGUILayout.MinMaxSlider(ref myObject.randomMinOffset.z,
									ref myObject.randomMaxOffset.z, -100f, 100f, GUILayout.MinWidth(100f));
								myObject.randomMaxOffset.z = EditorGUILayout.FloatField(myObject.randomMaxOffset.z,
									GUILayout.MaxWidth(50f));
							}
							EditorGUILayout.EndHorizontal();

							EditorGUILayout.PropertyField(shareOrientation);
						}
						EditorGUILayout.EndVertical();
					}
					EditorGUILayout.EndVertical();
				}
				break;

				case "Cooldown":
				{
					EditorGUILayout.BeginVertical(UIHelper.MainStyle);
					{
						EditorGUILayout.PropertyField(useCooldown);
						if (myObject.useCooldown)
						{
							EditorGUILayout.BeginVertical(UIHelper.SubStyle1);
							{
								EditorGUILayout.PropertyField(spawnCooldown);
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
						EditorGUILayout.PropertyField(useInput);
						if (myObject.useInput)
						{
							EditorGUILayout.BeginVertical(UIHelper.SubStyle1);
							{
								EditorGUILayout.PropertyField(spawnInputName);
							}
							EditorGUILayout.EndVertical();
						}
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
								EditorGUILayout.PropertyField(parent);
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
								EditorGUILayout.PropertyField(destroyOnNoResourceLeft);
							}
							EditorGUILayout.EndVertical();
						}
					}
					EditorGUILayout.EndVertical();
				}
				break;
			}

			soTarget.ApplyModifiedProperties();
			if (EditorGUI.EndChangeCheck())
			{
				GUI.FocusControl(GUI.GetNameOfFocusedControl());
			}

			// Can be used to display contextual error messages
			#region DebugMessages

			if(myObject.displayDebugInfo)
			{

				if (!myObject.useInput && !myObject.useCooldown)
				{
					EditorGUILayout.BeginHorizontal(UIHelper.WarningStyle);
					{
						EditorGUILayout.LabelField("Use Input and/or Cooldown-based spawning !", EditorStyles.boldLabel);
						if(GUILayout.Button("Input", UIHelper.ButtonStyle))
						{
							myObject.useInput = true;
							toolBarTab = 2;
							currentTab = "Input";
						}
						if (GUILayout.Button("Cooldown", UIHelper.ButtonStyle))
						{
							myObject.useCooldown = true;
							toolBarTab = 1;
							currentTab = "Cooldown";
						}
					}
					EditorGUILayout.EndHorizontal();
				}

				if (myObject.useInput && myObject.spawnInputName == "Input Name (InputManager)")
				{
					EditorGUILayout.BeginVertical(UIHelper.WarningStyle);
					{
						EditorGUILayout.LabelField("No Input set for spawning !", EditorStyles.boldLabel);
					}
					EditorGUILayout.EndVertical();
				}

				if (myObject.useCooldown && myObject.spawnCooldown == 0f)
				{
					EditorGUILayout.BeginVertical(UIHelper.WarningStyle);
					{
						EditorGUILayout.LabelField("Cooldown set for spawning is equal to 0 !", EditorStyles.boldLabel);
					}
					EditorGUILayout.EndVertical();
				}

				if (myObject.prefabToSpawn.Count == 0 || myObject.prefabToSpawn[0] == null)
				{
					EditorGUILayout.BeginVertical(UIHelper.WarningStyle);
					{
						EditorGUILayout.LabelField("No Prefab to spawn set !! Please add one", EditorStyles.boldLabel);
					}
					EditorGUILayout.EndVertical();
				}
			}
			
			#endregion
		}
		EditorGUILayout.EndVertical();
	}

	private void AddSpawn ()
	{
		prefabsToSpawn.Add(new Spawner.PrefabToSpawn(null, 1));
	}

	private void RemoveSpawn (int index)
	{
		prefabsToSpawn.RemoveAt(index);
		//Debug.Log("Prefab length " + myObject.prefabToSpawn.Count);
		if (myObject.prefabToSpawn.Count == 0)
		{
			showSpawns = false;
		}
	}
}
