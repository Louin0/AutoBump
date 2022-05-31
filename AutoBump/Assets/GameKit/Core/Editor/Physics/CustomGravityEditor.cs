using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CustomGravity))]
public class CustomGravityEditor : Editor
{
	[Tooltip("Used to check public variables from the target class")]
	int toolBarTab;
	string currentTab;

	CustomGravity myObject;
	SerializedObject soTarget;

	private SerializedProperty baseGravityForce;
	private SerializedProperty secondaryGravityForce;
	private SerializedProperty maxVelocity;

	private SerializedProperty invertOnInput;
	private SerializedProperty inputName;
	private SerializedProperty instantGravityChangeOnInput;
	private SerializedProperty invertJumpingDirection;

	private SerializedProperty onlyWhenGrounded;
	private SerializedProperty collisionCheckDistance;

	private SerializedProperty invertAnimationType;

	private SerializedProperty transformToInvert;
	private SerializedProperty invertedRotation;
	private SerializedProperty normalRotation;

	private SerializedProperty animator;
	private SerializedProperty gravityChangeBoolName;

	private void OnEnable ()
	{
		myObject = (CustomGravity)target;
		soTarget = new SerializedObject(target);

		////

		baseGravityForce = soTarget.FindProperty("baseGravityForce");
		secondaryGravityForce = soTarget.FindProperty("secondaryGravityForce");
		maxVelocity = soTarget.FindProperty("maxVelocity");

		invertOnInput = soTarget.FindProperty("invertOnInput");
		inputName = soTarget.FindProperty("inputName");
		instantGravityChangeOnInput = soTarget.FindProperty("instantGravityChangeOnInput");
		invertJumpingDirection = soTarget.FindProperty("invertJumpingDirection");

		onlyWhenGrounded = soTarget.FindProperty("onlyWhenGrounded");
		collisionCheckDistance = soTarget.FindProperty("collisionCheckDistance");

		invertAnimationType = soTarget.FindProperty("invertAnimationType");

		transformToInvert = soTarget.FindProperty("transformToInvert");
		invertedRotation = soTarget.FindProperty("invertedRotation");
		normalRotation = soTarget.FindProperty("normalRotation");

		animator = soTarget.FindProperty("animator");
		gravityChangeBoolName = soTarget.FindProperty("gravityChangeBoolName");
	}

	public override void OnInspectorGUI ()
	{   //Initializing Custom GUI Styles
		UIHelper.InitializeStyles();


		soTarget.Update();
		EditorGUI.BeginChangeCheck();

		EditorGUILayout.BeginVertical(UIHelper.MainStyle);
		{
			EditorGUILayout.BeginHorizontal(UIHelper.SubStyle1);
			{
				toolBarTab = GUILayout.Toolbar(toolBarTab, new string[] { "Gravity Inversion", "Forces", "Ground Check", "Animation" }, GUILayout.MinHeight(25));
			}
			EditorGUILayout.EndHorizontal();


			switch (toolBarTab)
			{
				case 0:
				currentTab = "Gravity Inversion";
				break;

				case 1:
				currentTab = "Forces";
				break;

				case 2:
				currentTab = "Ground Check";
				break;

				case 3:
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
				case "Forces":
				EditorGUILayout.BeginVertical(UIHelper.SubStyle1);
				{
					EditorGUILayout.PropertyField(baseGravityForce);
					if (myObject.invertOnInput)
						EditorGUILayout.PropertyField(secondaryGravityForce);
					EditorGUILayout.PropertyField(maxVelocity);
				}
				EditorGUILayout.EndVertical();
				break;

				case "Gravity Inversion":
				EditorGUILayout.BeginVertical(UIHelper.SubStyle1);
				{
					EditorGUILayout.PropertyField(invertOnInput);

					if (myObject.invertOnInput)
					{
						EditorGUILayout.BeginVertical(UIHelper.SubStyle2);
						{
							EditorGUILayout.PropertyField(inputName);
							EditorGUILayout.PropertyField(instantGravityChangeOnInput);
							EditorGUILayout.PropertyField(invertJumpingDirection);
						}
						EditorGUILayout.EndVertical();
					}
				}
				EditorGUILayout.EndVertical();
				break;

				case "Ground Check":
				EditorGUILayout.BeginVertical(UIHelper.SubStyle1);
				{
					EditorGUILayout.PropertyField(onlyWhenGrounded);

					if (myObject.onlyWhenGrounded)
					{
						EditorGUILayout.BeginVertical(UIHelper.SubStyle2);
						{
							EditorGUILayout.PropertyField(collisionCheckDistance);
						}
						EditorGUILayout.EndVertical();
					}
				}
				EditorGUILayout.EndVertical();
				break;

				case "Animation":

				EditorGUILayout.BeginVertical(UIHelper.SubStyle1);
				{
					EditorGUILayout.PropertyField(invertAnimationType);
					if (myObject.invertAnimationType == CustomGravity.InvertAnimationType.Rotation)
					{
						EditorGUILayout.BeginVertical(UIHelper.SubStyle2);
						{
							EditorGUILayout.PropertyField(transformToInvert);
							EditorGUILayout.PropertyField(invertedRotation);
							EditorGUILayout.PropertyField(normalRotation);
						}
						EditorGUILayout.EndVertical();
					}
					else if (myObject.invertAnimationType == CustomGravity.InvertAnimationType.Animation)
					{
						EditorGUILayout.BeginVertical(UIHelper.SubStyle2);
						{
							EditorGUILayout.PropertyField(animator);
							if (myObject.animator != null)
							{
								EditorGUILayout.PropertyField(gravityChangeBoolName);
							}
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

			EditorGUILayout.Space();

			// Can be used to display contextual error messages
			#region DebugMessages

			if (myObject.invertAnimationType == CustomGravity.InvertAnimationType.Animation)
			{
				if (myObject.animator == null)
				{
					EditorGUILayout.BeginVertical(UIHelper.WarningStyle);
					{
						EditorGUILayout.LabelField("No Animator found, please add one !", EditorStyles.boldLabel);
					}
					EditorGUILayout.EndVertical();
				}
				if (myObject.gravityChangeBoolName == "")
				{
					EditorGUILayout.BeginVertical(UIHelper.WarningStyle);
					{
						EditorGUILayout.LabelField("Parameter not set for animation !", EditorStyles.boldLabel);
					}
					EditorGUILayout.EndVertical();
				}
			}

			#endregion
		}
		EditorGUILayout.EndVertical();
	}
}