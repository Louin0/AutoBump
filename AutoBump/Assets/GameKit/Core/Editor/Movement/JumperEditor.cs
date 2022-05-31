using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Jumper))]
public class JumperEditor : Editor
{
	int toolBarTab;
	string currentTab;

	Jumper myObject;
	SerializedObject soTarget;

	private SerializedProperty jumpInputName;
	private SerializedProperty jumpForce;
	private SerializedProperty coyoteTimeDuration;
	private SerializedProperty jumpLayerMask;
	private SerializedProperty fallGravityMultiplier;
	private SerializedProperty jumpEndYVelocity;
	private SerializedProperty lowJumpGravityMultiplier;

	private SerializedProperty jumpAmount;
	private SerializedProperty airJumpForce;
	private SerializedProperty resetVelocityOnAirJump;

	private SerializedProperty useCollisionCheck;
	private SerializedProperty collisionOffset;
	private SerializedProperty collisionCheckRadius;
	private SerializedProperty characterHeight;

	private SerializedProperty jumpFX;
	private SerializedProperty airJumpFX;
	private SerializedProperty FXOffset;
	private SerializedProperty timeBeforeDestroyFX;

	private SerializedProperty animator;
	private SerializedProperty jumpTriggerName;
	private SerializedProperty airJumpTriggerName;


	private void OnEnable ()
	{
		myObject = (Jumper)target;
		soTarget = new SerializedObject(target);

		jumpInputName = soTarget.FindProperty("jumpInputName");
		jumpForce = soTarget.FindProperty("jumpForce");
		coyoteTimeDuration = soTarget.FindProperty("coyoteTimeDuration");
		jumpLayerMask = soTarget.FindProperty("jumpLayerMask");

		jumpEndYVelocity = soTarget.FindProperty("jumpEndYVelocity");
		fallGravityMultiplier = soTarget.FindProperty("fallGravityMultiplier");
		lowJumpGravityMultiplier = soTarget.FindProperty("lowJumpGravityMultiplier");

		jumpAmount = soTarget.FindProperty("jumpAmount");
		airJumpForce = soTarget.FindProperty("airJumpForce");
		resetVelocityOnAirJump = soTarget.FindProperty("resetVelocityOnAirJump");

		useCollisionCheck = soTarget.FindProperty("useCollisionCheck");
		collisionOffset = soTarget.FindProperty("collisionOffset");
		collisionCheckRadius = soTarget.FindProperty("collisionCheckRadius");
		characterHeight = soTarget.FindProperty("characterHeight");

		jumpFX = soTarget.FindProperty("jumpFX");
		airJumpFX = soTarget.FindProperty("airJumpFX");
		FXOffset = soTarget.FindProperty("FXOffset");
		timeBeforeDestroyFX = soTarget.FindProperty("timeBeforeDestroyFX");

		animator = soTarget.FindProperty("animator");
		jumpTriggerName = soTarget.FindProperty("jumpTriggerName");
		airJumpTriggerName = soTarget.FindProperty("airJumpTriggerName");
	}

	public override void OnInspectorGUI ()
	{
		//Initializing Custom GUI Styles
		UIHelper.InitializeStyles();

		soTarget.Update();
		EditorGUI.BeginChangeCheck();

		EditorGUILayout.BeginHorizontal(UIHelper.MainStyle);
		{
			toolBarTab = GUILayout.Toolbar(toolBarTab, new string[] { "Jump", "Air Jump", "Collision", "FX", "Animation" }, GUILayout.MinHeight(25));
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
			currentTab = "Jump";
			break;

			case 1:
			currentTab = "Air Jump";
			break;

			case 2:
			currentTab = "Collision";
			break;

			case 3:
			currentTab = "FX";
			break;

			case 4:
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
			case "Jump":
			EditorGUILayout.BeginVertical(UIHelper.MainStyle);
			{
				EditorGUILayout.PropertyField(jumpInputName);
				EditorGUILayout.PropertyField(jumpForce);
				EditorGUILayout.PropertyField(coyoteTimeDuration);
				EditorGUILayout.PropertyField(jumpLayerMask);

				EditorGUILayout.Separator();

				EditorGUILayout.BeginVertical(UIHelper.SubStyle1);
				{
					EditorGUILayout.LabelField("Better Jump", EditorStyles.boldLabel);
					EditorGUILayout.PropertyField(jumpEndYVelocity);
					EditorGUILayout.PropertyField(fallGravityMultiplier);
					EditorGUILayout.PropertyField(lowJumpGravityMultiplier);
				}
				EditorGUILayout.EndVertical();
				
			}
			EditorGUILayout.EndVertical();
			break;

			case "Air Jump":

			EditorGUILayout.BeginVertical(UIHelper.MainStyle);
			{
				EditorGUILayout.IntSlider(jumpAmount, 1, 10);

				if (myObject.jumpAmount > 1)
				{
					EditorGUILayout.BeginVertical(UIHelper.SubStyle1);
					{
						EditorGUILayout.PropertyField(airJumpForce);
						EditorGUILayout.PropertyField(resetVelocityOnAirJump);
					}
					EditorGUILayout.EndVertical();
				}
			}
			EditorGUILayout.EndVertical();
			break;

			case "Collision":
			EditorGUILayout.BeginVertical(UIHelper.MainStyle);
			{
				EditorGUILayout.PropertyField(useCollisionCheck);
				if (myObject.useCollisionCheck)
				{
					EditorGUILayout.BeginVertical(UIHelper.SubStyle1);
					{
						EditorGUILayout.PropertyField(collisionOffset);
						EditorGUILayout.PropertyField(collisionCheckRadius);
						EditorGUILayout.PropertyField(characterHeight);
					}
					EditorGUILayout.EndVertical();
				}
			}
			EditorGUILayout.EndVertical();
			break;

			case "FX":
			EditorGUILayout.BeginVertical(UIHelper.MainStyle);
			{
				EditorGUILayout.PropertyField(jumpFX);
				EditorGUILayout.PropertyField(airJumpFX);

				if (myObject.jumpFX != null || myObject.airJumpFX != null)
				{
					EditorGUILayout.BeginVertical(UIHelper.SubStyle1);
					{
						EditorGUILayout.PropertyField(FXOffset);
						EditorGUILayout.PropertyField(timeBeforeDestroyFX);
					}
					EditorGUILayout.EndVertical();
				}

			}
			EditorGUILayout.EndVertical();
			break;

			case "Animation":
			EditorGUILayout.BeginVertical(UIHelper.MainStyle);
			{
				EditorGUILayout.PropertyField(animator);

				if(myObject.animator != null)
				{
					EditorGUILayout.BeginVertical(UIHelper.SubStyle1);
					{
						EditorGUILayout.PropertyField(jumpTriggerName);
						EditorGUILayout.PropertyField(airJumpTriggerName);
					}
					EditorGUILayout.EndVertical();
				}
			}
			EditorGUILayout.EndVertical();
			break;
		}

		if (EditorGUI.EndChangeCheck())
		{
			soTarget.ApplyModifiedProperties();
		}
	}
}
