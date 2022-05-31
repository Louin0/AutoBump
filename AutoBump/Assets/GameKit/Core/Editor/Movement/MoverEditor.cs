using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Mover))]
public class MoverEditor : Editor
{
	int toolBarTab;
	string currentTab;

	Mover myMover;
	SerializedObject soTarget;

	private SerializedProperty movementType;

	private SerializedProperty useXAxis;
	private SerializedProperty useYAxis;
	private SerializedProperty useZAxis;

	private SerializedProperty requiresInput;
	private SerializedProperty xInputName;
	private SerializedProperty yInputName;
	private SerializedProperty zInputName;

	private SerializedProperty xSpeed;
	private SerializedProperty ySpeed;
	private SerializedProperty zSpeed;
	private SerializedProperty maximumVelocity;
	private SerializedProperty lookAtDirection;
	private SerializedProperty isMovementLocal;

	private SerializedProperty useWallAvoidance;
	private SerializedProperty wallAvoidanceSize;
	private SerializedProperty wallAvoidanceOffset;
	private SerializedProperty wallAvoidanceLayers;

	private SerializedProperty animator;
	private SerializedProperty movementParticleSystem;
	private SerializedProperty minVelocityToToggle;

	private SerializedProperty xDirParameterName;
	private SerializedProperty yDirParameterName;
	private SerializedProperty zDirParameterName;

	private SerializedProperty keepOrientationOnIdle;

	private SerializedProperty trackSpeed;
	private SerializedProperty speedParameterName;

	private SerializedProperty animateWhenGroundedOnly;
	private SerializedProperty collisionCheckRadius;
	private SerializedProperty collisionOffset;
	private SerializedProperty groundedParameterName;
	private SerializedProperty groundLayerMask;

	private SerializedProperty useGravity;

	private void OnEnable ()
	{
		myMover = (Mover)target;
		soTarget = new SerializedObject(target);

		movementType = soTarget.FindProperty("movementType");

		useXAxis = soTarget.FindProperty("useXAxis");
		useYAxis = soTarget.FindProperty("useYAxis");
		useZAxis = soTarget.FindProperty("useZAxis");

		requiresInput = soTarget.FindProperty("requiresInput");
		xInputName = soTarget.FindProperty("xInputName");
		yInputName = soTarget.FindProperty("yInputName");
		zInputName = soTarget.FindProperty("zInputName");

		xSpeed = soTarget.FindProperty("xSpeed");
		ySpeed = soTarget.FindProperty("ySpeed");
		zSpeed = soTarget.FindProperty("zSpeed");
		maximumVelocity = soTarget.FindProperty("maximumVelocity");
		lookAtDirection = soTarget.FindProperty("lookAtDirection");
		isMovementLocal = soTarget.FindProperty("isMovementLocal");

		useWallAvoidance = soTarget.FindProperty("useWallAvoidance");
		wallAvoidanceSize = soTarget.FindProperty("wallAvoidanceSize");
		wallAvoidanceOffset = soTarget.FindProperty("wallAvoidanceOffset");
		wallAvoidanceLayers = soTarget.FindProperty("wallAvoidanceLayers");

		animator = soTarget.FindProperty("animator");
		movementParticleSystem = soTarget.FindProperty("movementParticleSystem");
		minVelocityToToggle = soTarget.FindProperty("minVelocityToToggle");

		xDirParameterName = soTarget.FindProperty("xDirParameterName");
		yDirParameterName = soTarget.FindProperty("yDirParameterName");
		zDirParameterName = soTarget.FindProperty("zDirParameterName");

		keepOrientationOnIdle = soTarget.FindProperty("keepOrientationOnIdle");
		trackSpeed = soTarget.FindProperty("trackSpeed");
		speedParameterName = soTarget.FindProperty("speedParameterName");

		animateWhenGroundedOnly = soTarget.FindProperty("animateWhenGroundedOnly");
		collisionCheckRadius = soTarget.FindProperty("collisionCheckRadius");
		collisionOffset = soTarget.FindProperty("collisionOffset");
		groundedParameterName = soTarget.FindProperty("groundedParameterName");
		groundLayerMask = soTarget.FindProperty("groundLayerMask");

		useGravity = soTarget.FindProperty("useGravity");
	}


	public override void OnInspectorGUI ()
	{
		//Initializing Custom GUI Styles
		UIHelper.InitializeStyles();

		soTarget.Update();
		EditorGUI.BeginChangeCheck();

		EditorGUILayout.BeginHorizontal(UIHelper.MainStyle);
		{
			EditorGUILayout.BeginVertical(UIHelper.SubStyle1);
			{
				EditorGUILayout.PropertyField(movementType);
			}
			EditorGUILayout.EndVertical();

			EditorGUILayout.BeginVertical(UIHelper.SubStyle1);
			{
				if (myMover.displayDebugInfo)
				{
					if (GUILayout.Button(" Debug ON ", UIHelper.GreenButtonStyle, GUILayout.MaxHeight(20f)))
					{
						myMover.displayDebugInfo = !myMover.displayDebugInfo;
					}
				}
				else
				{
					if (GUILayout.Button(" Debug OFF ", UIHelper.RedButtonStyle, GUILayout.MaxHeight(20f)))
					{
						myMover.displayDebugInfo = !myMover.displayDebugInfo;
					}
				}

			}
			EditorGUILayout.EndVertical();
		}
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginVertical(UIHelper.MainStyle);
		{
			toolBarTab = GUILayout.Toolbar(toolBarTab, new string[] { "Axis Constraints", "Input", "Movement", "Animation" });

			switch (toolBarTab)
			{
				case 0:
				currentTab = "Axis Constraints";
				break;

				case 1:
				currentTab = "Input";
				break;

				case 2:
				currentTab = "Movement";
				break;

				case 3:
				currentTab = "Animation";
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
				case "Axis Constraints":
				EditorGUILayout.BeginVertical(UIHelper.MainStyle);
				{
					EditorGUILayout.PropertyField(useXAxis);
					EditorGUILayout.PropertyField(useYAxis);
					EditorGUILayout.PropertyField(useZAxis);
				}
				EditorGUILayout.EndVertical();
				break;

				case "Input":
				EditorGUILayout.BeginVertical(UIHelper.MainStyle);
				{
					EditorGUILayout.PropertyField(requiresInput);
					if (myMover.requiresInput && (myMover.useXAxis || myMover.useYAxis || myMover.useZAxis))
					{
						EditorGUILayout.BeginVertical(UIHelper.SubStyle1);
						{
							if (myMover.useXAxis && myMover.requiresInput)
							{
								EditorGUILayout.PropertyField(xInputName);
							}
							if (myMover.useYAxis && myMover.requiresInput)
							{
								EditorGUILayout.PropertyField(yInputName);
							}
							if (myMover.useZAxis && myMover.requiresInput)
							{
								EditorGUILayout.PropertyField(zInputName);
							}
						}
						EditorGUILayout.EndVertical();
					}
				}
				EditorGUILayout.EndVertical();
				break;

				case "Movement":
				EditorGUILayout.BeginVertical(UIHelper.MainStyle);
				{
					EditorGUILayout.BeginVertical(UIHelper.SubStyle1);
					{
						EditorGUILayout.PropertyField(isMovementLocal);
					}
					EditorGUILayout.EndVertical();

					EditorGUILayout.BeginVertical(UIHelper.SubStyle1);
					{
						if (myMover.useXAxis)
						{
							EditorGUILayout.PropertyField(xSpeed);
						}
						if (myMover.useYAxis)
						{
							EditorGUILayout.PropertyField(ySpeed);
						}
						if (myMover.useZAxis)
						{
							EditorGUILayout.PropertyField(zSpeed);
						}
					}
					EditorGUILayout.EndVertical();

					if (myMover.movementType == Mover.MovementType.Rigidbody)
					{
						EditorGUILayout.BeginVertical(UIHelper.SubStyle1);
						{

							EditorGUILayout.PropertyField(maximumVelocity);

						}
						EditorGUILayout.EndVertical();
					}

					if (myMover.movementType == Mover.MovementType.Controller)
					{
						EditorGUILayout.BeginVertical(UIHelper.SubStyle1);
						{
							EditorGUILayout.PropertyField(useGravity);
						}
						EditorGUILayout.EndVertical();
					}


					EditorGUILayout.BeginVertical(UIHelper.SubStyle1);
					{
						EditorGUILayout.PropertyField(lookAtDirection);
					}
					EditorGUILayout.EndVertical();

					EditorGUILayout.BeginVertical(UIHelper.SubStyle1);
					{
						EditorGUILayout.PropertyField(useWallAvoidance);
						if (myMover.useWallAvoidance)
						{
							EditorGUILayout.BeginVertical(UIHelper.SubStyle2);
							{
								EditorGUILayout.PropertyField(wallAvoidanceSize);
								EditorGUILayout.PropertyField(wallAvoidanceOffset);
								EditorGUILayout.PropertyField(wallAvoidanceLayers);
							}
							EditorGUILayout.EndVertical();
						}
					}
					EditorGUILayout.EndVertical();
				}
				EditorGUILayout.EndVertical();
				break;

				case "Animation":

				EditorGUILayout.BeginVertical(UIHelper.MainStyle);
				{
					EditorGUILayout.PropertyField(animator);
					if (myMover.animator != null)
					{
						EditorGUILayout.BeginVertical(UIHelper.SubStyle1);
						{
							if (myMover.useXAxis)
							{
								EditorGUILayout.PropertyField(xDirParameterName);
							}
							if (myMover.useYAxis)
							{
								EditorGUILayout.PropertyField(yDirParameterName);
							}
							if (myMover.useZAxis)
							{
								EditorGUILayout.PropertyField(zDirParameterName);
							}
						}
						EditorGUILayout.EndVertical();

						EditorGUILayout.BeginVertical(UIHelper.SubStyle1);
						{
							EditorGUILayout.PropertyField(keepOrientationOnIdle);
						}
						EditorGUILayout.EndVertical();


						EditorGUILayout.BeginVertical(UIHelper.SubStyle1);
						{
							EditorGUILayout.PropertyField(trackSpeed);
							if (myMover.trackSpeed)
							{
								EditorGUILayout.BeginVertical(UIHelper.SubStyle2);
								{
									EditorGUILayout.PropertyField(speedParameterName);
								}
								EditorGUILayout.EndVertical();
							}
						}
						EditorGUILayout.EndVertical();

						EditorGUILayout.BeginVertical(UIHelper.SubStyle1);
						{
							EditorGUILayout.PropertyField(animateWhenGroundedOnly);
							if (myMover.animateWhenGroundedOnly)
							{
								EditorGUILayout.BeginVertical(UIHelper.SubStyle2);
								{
									EditorGUILayout.PropertyField(collisionCheckRadius);
									EditorGUILayout.PropertyField(collisionOffset);
									EditorGUILayout.PropertyField(groundedParameterName);
									EditorGUILayout.PropertyField(groundLayerMask);
								}
								EditorGUILayout.EndVertical();
							}
						}
						EditorGUILayout.EndVertical();
					}

					EditorGUILayout.PropertyField(movementParticleSystem);
					if (myMover.movementParticleSystem != null)
					{
						EditorGUILayout.BeginVertical(UIHelper.SubStyle1);
						{
							EditorGUILayout.PropertyField(minVelocityToToggle);
						}
						EditorGUILayout.EndVertical();
					}
				}
				EditorGUILayout.EndVertical();
				break;
			}

		}
		EditorGUILayout.EndVertical();

		if (EditorGUI.EndChangeCheck())
		{
			soTarget.ApplyModifiedProperties();
		}

		EditorGUILayout.Space();
		///

		#region DebugMessages

		switch (toolBarTab)
		{
			case 0:
			if (!myMover.useXAxis && !myMover.useYAxis && !myMover.useZAxis)
			{
				EditorGUILayout.BeginVertical(UIHelper.WarningStyle);
				{
					EditorGUILayout.LabelField("Add at least one axis !", EditorStyles.boldLabel);
				}
				EditorGUILayout.EndVertical();
			}
			break;

			case 1:
			if (myMover.xInputName == "" && myMover.useXAxis && myMover.requiresInput)
			{
				EditorGUILayout.BeginVertical(UIHelper.WarningStyle);
				{
					EditorGUILayout.LabelField("Input name not set for X Axis !", EditorStyles.boldLabel);
				}
				EditorGUILayout.EndVertical();
			}
			if (myMover.yInputName == "" && myMover.useYAxis && myMover.requiresInput)
			{
				EditorGUILayout.BeginVertical(UIHelper.WarningStyle);
				{
					EditorGUILayout.LabelField("Input name not set for Y Axis !", EditorStyles.boldLabel);
				}
				EditorGUILayout.EndVertical();
			}
			if (myMover.zInputName == "" && myMover.useZAxis && myMover.requiresInput)
			{
				EditorGUILayout.BeginVertical(UIHelper.WarningStyle);
				{
					EditorGUILayout.LabelField("Input name not set for Z Axis !", EditorStyles.boldLabel);
				}
				EditorGUILayout.EndVertical();
			}
			break;

			case 2:
			if (myMover.xSpeed == 0 && myMover.useXAxis)
			{
				EditorGUILayout.BeginVertical(UIHelper.WarningStyle);
				{
					EditorGUILayout.LabelField("X Speed is equal to 0. Either disable useXAxis or set a speed to X Axis", EditorStyles.boldLabel);
				}
				EditorGUILayout.EndVertical();
			}
			if (myMover.ySpeed == 0 && myMover.useYAxis)
			{
				EditorGUILayout.BeginVertical(UIHelper.WarningStyle);
				{
					EditorGUILayout.LabelField("Y Speed is equal to 0. Either disable useYAxis or set a speed to Y Axis", EditorStyles.boldLabel);
				}
				EditorGUILayout.EndVertical();
			}
			if (myMover.zSpeed == 0 && myMover.useZAxis)
			{
				EditorGUILayout.BeginVertical(UIHelper.WarningStyle);
				{
					EditorGUILayout.LabelField("Z Speed is equal to 0. Either disable useZAxis or set a speed to Z Axis", EditorStyles.boldLabel);
				}
				EditorGUILayout.EndVertical();
			}
			break;

			case 3:

			break;
		}

		#endregion
	}
}
