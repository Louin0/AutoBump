using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

[CustomEditor(typeof(DirectionalImpulse))]
public class DirectionalImpulseEditor : Editor
{
	[Tooltip("Used to check public variables from the target class")]
	int toolBarTab;
	string currentTab;

	DirectionalImpulse myObject;
	SerializedObject soTarget;

	private SerializedProperty directionInputType;
	private SerializedProperty horizontalAxis;
	private SerializedProperty horizontalAxisName;

	private SerializedProperty verticalAxis;
	private SerializedProperty verticalAxisName;

	private SerializedProperty impulseInputName;

	private SerializedProperty depthAxis;

	private SerializedProperty resetVelocityOnImpulse;
	private SerializedProperty impulseForce;

	private SerializedProperty useCooldown;
	private SerializedProperty cooldown;

	private SerializedProperty minimalHeightToImpulse;
	private SerializedProperty heightAxis;
	private SerializedProperty groundDetectionLayerMask;
	private SerializedProperty collisionOffset;

	private SerializedProperty requireResources;
	private SerializedProperty resourceManager;
	private SerializedProperty resourceIndex;
	private SerializedProperty resourceCostOnUse;

	private SerializedProperty preventMovingAfterImpulse;
	private SerializedProperty mover;
	private SerializedProperty movePreventionDuration;

	private SerializedProperty fXFeedbackType;
	private SerializedProperty impulseParticleSystem;
	//UNCOMMENT ONLY IF USING VFX GRAPH PACKAGE
	//private SerializedProperty impulseVFX;
	//private SerializedProperty impulseEventName;
	private SerializedProperty spawnedFX;
	private SerializedProperty FXOffset;
	private SerializedProperty timeBeforeDestroyFX;

	private SerializedProperty animator;
	private SerializedProperty dashTriggerName;

	private void OnEnable ()
	{
		myObject = (DirectionalImpulse)target;
		soTarget = new SerializedObject(target);
		
		////

		directionInputType = soTarget.FindProperty("directionInputType");
		horizontalAxis = soTarget.FindProperty("horizontalAxis");
		horizontalAxisName = soTarget.FindProperty("horizontalAxisName");

		verticalAxis = soTarget.FindProperty("verticalAxis");
		verticalAxisName = soTarget.FindProperty("verticalAxisName");

		impulseInputName = soTarget.FindProperty("impulseInputName");
		depthAxis = soTarget.FindProperty("depthAxis");

		resetVelocityOnImpulse = soTarget.FindProperty("resetVelocityOnImpulse");
		impulseForce = soTarget.FindProperty("impulseForce");

		useCooldown = soTarget.FindProperty("useCooldown");
		cooldown = soTarget.FindProperty("cooldown");

		minimalHeightToImpulse = soTarget.FindProperty("minimalHeightToImpulse");
		heightAxis = soTarget.FindProperty("heightAxis");
		groundDetectionLayerMask = soTarget.FindProperty("groundDetectionLayerMask");
		collisionOffset = soTarget.FindProperty("collisionOffset");

		requireResources = soTarget.FindProperty("requireResources");
		resourceManager = soTarget.FindProperty("resourceManager");
		resourceIndex = soTarget.FindProperty("resourceIndex");
		resourceCostOnUse = soTarget.FindProperty("resourceCostOnUse");

		preventMovingAfterImpulse = soTarget.FindProperty("preventMovingAfterImpulse");
		mover = soTarget.FindProperty("mover");
		movePreventionDuration = soTarget.FindProperty("movePreventionDuration");

		impulseParticleSystem = soTarget.FindProperty("impulseParticleSystem");

		//UNCOMMENT ONLY IF USING VFX GRAPH PACKAGE
		//impulseVFX = soTarget.FindProperty("impulseVFX");
		//impulseEventName = soTarget.FindProperty("impulseEventName");

		fXFeedbackType = soTarget.FindProperty("fXFeedbackType");
		spawnedFX = soTarget.FindProperty("spawnedFX");
		FXOffset = soTarget.FindProperty("FXOffset");
		timeBeforeDestroyFX = soTarget.FindProperty("timeBeforeDestroyFX");

		animator = soTarget.FindProperty("animator");
		dashTriggerName = soTarget.FindProperty("dashTriggerName");

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

				toolBarTab = GUILayout.SelectionGrid(toolBarTab, new string[] { "Input", "Force", "Cooldown", "Collision", "Resources", "Movement Prevention", "FX", "Animation" }, 4, GUILayout.MinHeight(40));

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
				currentTab = "Input";
				break;

				case 1:
				currentTab = "Force";
				break;

				case 2:
				currentTab = "Cooldown";
				break;

				case 3:
				currentTab = "Collision";
				break;

				case 4:
				currentTab = "Resources";
				break;

				case 5:
				currentTab = "Movement";
				break;

				case 6:
				currentTab = "FX";
				break;

				case 7:
				currentTab = "Animation";
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
				case "Input":
				EditorGUILayout.BeginVertical(UIHelper.MainStyle);
				{
					EditorGUILayout.PropertyField(impulseInputName);

					EditorGUILayout.Space();

					EditorGUILayout.PropertyField(directionInputType);

					switch (myObject.directionInputType)
					{
						case DirectionalImpulse.DirectionInputType.Axis:
						EditorGUILayout.BeginVertical(UIHelper.SubStyle1);
						{
							EditorGUILayout.PropertyField(horizontalAxis);
							if (myObject.horizontalAxis != DirectionalImpulse.Axis.None)
							{
								EditorGUILayout.BeginVertical(UIHelper.SubStyle2);
								{
									EditorGUILayout.PropertyField(horizontalAxisName);
								}
								EditorGUILayout.EndVertical();
							}
						}
						EditorGUILayout.EndVertical();

						EditorGUILayout.BeginVertical(UIHelper.SubStyle1);
						{
							EditorGUILayout.PropertyField(verticalAxis);
							if (myObject.verticalAxis != DirectionalImpulse.Axis.None)
							{
								EditorGUILayout.BeginVertical(UIHelper.SubStyle2);
								{
									EditorGUILayout.PropertyField(verticalAxisName);
								}
								EditorGUILayout.EndVertical();
							}
						}
						EditorGUILayout.EndVertical();
						break;

						case DirectionalImpulse.DirectionInputType.MousePos:
						EditorGUILayout.BeginVertical(UIHelper.SubStyle1);
						{
							EditorGUILayout.PropertyField(depthAxis);
						}
						EditorGUILayout.EndVertical();
						break;
					}
				}
				EditorGUILayout.EndVertical();
				break;

				case "Force":
				EditorGUILayout.BeginVertical(UIHelper.MainStyle);
				{
					EditorGUILayout.PropertyField(resetVelocityOnImpulse);
					EditorGUILayout.PropertyField(impulseForce);
				}
				EditorGUILayout.EndVertical();
				break;

				case "Cooldown":
				EditorGUILayout.BeginVertical(UIHelper.MainStyle);
				{
					EditorGUILayout.BeginVertical(UIHelper.SubStyle1);
					{
						EditorGUILayout.PropertyField(useCooldown);

						if (myObject.useCooldown)
						{
							EditorGUILayout.BeginVertical(UIHelper.SubStyle2);
							{
								EditorGUILayout.PropertyField(cooldown);
							}
							EditorGUILayout.EndVertical();
						}
					}
					EditorGUILayout.EndVertical();
				}
				EditorGUILayout.EndVertical();
				break;

				case "Collision":
				EditorGUILayout.BeginVertical(UIHelper.MainStyle);
				{
					EditorGUILayout.BeginVertical(UIHelper.SubStyle1);
					{
						EditorGUILayout.PropertyField(minimalHeightToImpulse);

						if (myObject.minimalHeightToImpulse > 0)
						{
							EditorGUILayout.BeginVertical(UIHelper.SubStyle2);
							{
								EditorGUILayout.PropertyField(heightAxis);
								EditorGUILayout.PropertyField(groundDetectionLayerMask);
								EditorGUILayout.PropertyField(collisionOffset);
							}
							EditorGUILayout.EndVertical();
						}
					}
					EditorGUILayout.EndVertical();
				}
				EditorGUILayout.EndVertical();
				break;

				case "Resources":
				EditorGUILayout.BeginVertical(UIHelper.MainStyle);
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
				EditorGUILayout.EndVertical();
				break;

				case "Movement":
				EditorGUILayout.BeginVertical(UIHelper.MainStyle);
				{
					EditorGUILayout.BeginVertical(UIHelper.SubStyle1);
					{
						EditorGUILayout.PropertyField(preventMovingAfterImpulse);

						if (myObject.preventMovingAfterImpulse)
						{
							EditorGUILayout.BeginVertical(UIHelper.SubStyle2);
							{
								EditorGUILayout.PropertyField(mover);
								EditorGUILayout.PropertyField(movePreventionDuration);
							}
							EditorGUILayout.EndVertical();
						}
					}
					EditorGUILayout.EndVertical();
				}
				EditorGUILayout.EndVertical();
				break;

				case "FX":
				EditorGUILayout.BeginVertical(UIHelper.MainStyle);
				{
					EditorGUILayout.BeginVertical(UIHelper.SubStyle1);
					{
						EditorGUILayout.PropertyField(fXFeedbackType);
						switch (myObject.fXFeedbackType)
						{
							case Helper.FXFeedbackType.Instantiate:
								{
									EditorGUILayout.BeginVertical(UIHelper.SubStyle2);
									{
										EditorGUILayout.PropertyField(spawnedFX);
										if (myObject.spawnedFX != null)
										{
											EditorGUILayout.PropertyField(FXOffset);
											EditorGUILayout.PropertyField(timeBeforeDestroyFX);
										}
									}
									EditorGUILayout.EndVertical();
								}
							break;

							case Helper.FXFeedbackType.ParticleSystem:
								{
									EditorGUILayout.BeginVertical(UIHelper.SubStyle2);
									{
										EditorGUILayout.PropertyField(impulseParticleSystem);
									}
									EditorGUILayout.EndVertical();
								}
							break;

							case Helper.FXFeedbackType.VFXGraph:
								{
									EditorGUILayout.BeginVertical(UIHelper.SubStyle2);
									{
										//UNCOMMENT ONLY IF USING VFX GRAPH PACKAGE
										//EditorGUILayout.PropertyField(impulseVFX);
										//EditorGUILayout.PropertyField(impulseEventName);

										//Delete those 3 lines if using VFX Graph package !
										EditorGUILayout.BeginVertical(UIHelper.WarningStyle);
										{
											EditorGUILayout.LabelField("VFX Not working, please uncomment lines of code in DirectionalImpulse and DirectionalImpulseEditor scripts", EditorStyles.boldLabel);
										}
										EditorGUILayout.EndVertical();
									}
									EditorGUILayout.EndVertical();
								}
								break;
						}

						
					}
					EditorGUILayout.EndVertical();
				}
				EditorGUILayout.EndVertical();
				break;

				case "Animation":
				EditorGUILayout.BeginVertical(UIHelper.MainStyle);
				{
					EditorGUILayout.BeginVertical(UIHelper.SubStyle1);
					{
						EditorGUILayout.PropertyField(animator);

						if (myObject.animator != null)
						{
							EditorGUILayout.BeginVertical(UIHelper.SubStyle2);
							{
								EditorGUILayout.PropertyField(dashTriggerName);
							}
							EditorGUILayout.EndVertical();
						}
					}
					EditorGUILayout.EndVertical();
				}
				EditorGUILayout.EndVertical();
				break;
			}

			if (EditorGUI.EndChangeCheck())
			{
				soTarget.ApplyModifiedProperties();
			}

			EditorGUILayout.Space();

			// Can be used to display contextual error messages
			#region DebugMessages
			if (myObject.preventMovingAfterImpulse && myObject.mover == null)
			{
				EditorGUILayout.BeginVertical(UIHelper.WarningStyle);
				{
					EditorGUILayout.LabelField("Mover Component not found ! If you want to prevent movement, start by adding a mover !", EditorStyles.boldLabel);
				}
				EditorGUILayout.EndVertical();
			}

			if (myObject.fXFeedbackType == Helper.FXFeedbackType.Instantiate && myObject.spawnedFX == null)
			{
				EditorGUILayout.BeginVertical(UIHelper.WarningStyle);
				{
					EditorGUILayout.LabelField("Spawned FX not found ! Please reference a prefab to spawn from the Project folder", EditorStyles.boldLabel);
				}
				EditorGUILayout.EndVertical();
			}
			#endregion
		}
		EditorGUILayout.EndVertical();
	}
}