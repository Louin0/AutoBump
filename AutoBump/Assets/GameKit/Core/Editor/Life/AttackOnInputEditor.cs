using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AttackOnInput))]
public class AttackOnInputEditor : Editor
{
	[Tooltip("Used to check public variables from the target class")]
	int toolBarTab;
	string currentTab;

	AttackOnInput myObject;
	SerializedObject soTarget;

	private SerializedProperty inputName;
	private SerializedProperty attackLayerMask;
	private SerializedProperty attackAngle;
	private SerializedProperty attackRange;
	private SerializedProperty damageDelay;
	private SerializedProperty attackCooldown;

	private SerializedProperty attackDamage;
	private SerializedProperty attackKnockback;
	private SerializedProperty attackUpwardsKnockback;
	private SerializedProperty hitFX;

	private SerializedProperty attackTriggerParameterName;
	private SerializedProperty animator;

	private void OnEnable ()
	{
		myObject = (AttackOnInput)target;
		soTarget = new SerializedObject(target);

		////

		inputName = soTarget.FindProperty("inputName");
		attackLayerMask = soTarget.FindProperty("attackLayerMask");
		attackAngle = soTarget.FindProperty("attackAngle");
		attackRange = soTarget.FindProperty("attackRange");
		damageDelay = soTarget.FindProperty("damageDelay");
		attackCooldown = soTarget.FindProperty("attackCooldown");

		attackDamage = soTarget.FindProperty("attackDamage");
		attackKnockback = soTarget.FindProperty("attackKnockback");
		attackUpwardsKnockback = soTarget.FindProperty("attackUpwardsKnockback");
		hitFX = soTarget.FindProperty("hitFX");

		attackTriggerParameterName = soTarget.FindProperty("attackTriggerParameterName");
		animator = soTarget.FindProperty("animator");

	}


	public override void OnInspectorGUI ()
	{
		//Initializing Custom GUI Styles
		UIHelper.InitializeStyles();

		soTarget.Update();
		EditorGUI.BeginChangeCheck();

		EditorGUILayout.BeginVertical(UIHelper.MainStyle);
		{
			if(myObject.displayDebugInfo)
			{
				if (myObject.attackCooldown > 0f)
				{
					if (myObject.timer > 0f)
					{
						EditorGUI.ProgressBar(new Rect(20, 20, EditorGUIUtility.currentViewWidth - 40, 20), 1f - (myObject.timer / myObject.attackCooldown), "Cooldown");
					}
					else
					{
						EditorGUI.ProgressBar(new Rect(20, 20, EditorGUIUtility.currentViewWidth - 40, 20), 1f - (myObject.timer / myObject.attackCooldown), "Can attack now !");
					}
				}

				EditorGUILayout.Space(30f);
			}

			EditorGUILayout.BeginHorizontal(UIHelper.SubStyle1);
			{
				toolBarTab = GUILayout.Toolbar(toolBarTab, new string[] { "General", "Hit", "Animation", "FX"}, GUILayout.MinHeight(25));

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
				currentTab = "General";
				break;

				case 1:
				currentTab = "Hit";
				break;

				case 2:
				currentTab = "Animation";
				break;

				case 3:
				currentTab = "FX";
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
				case "General":
				EditorGUILayout.BeginVertical(UIHelper.MainStyle);
				{
					EditorGUILayout.PropertyField(inputName);
					EditorGUILayout.PropertyField(attackLayerMask);
					EditorGUILayout.PropertyField(attackAngle);
					EditorGUILayout.PropertyField(attackRange);
					EditorGUILayout.PropertyField(damageDelay);
					EditorGUILayout.PropertyField(attackCooldown);
				}
				EditorGUILayout.EndVertical();
				break;

				case "Hit":
				EditorGUILayout.BeginVertical(UIHelper.MainStyle);
				{
					EditorGUILayout.PropertyField(attackDamage);
					EditorGUILayout.PropertyField(attackKnockback);
					EditorGUILayout.PropertyField(attackUpwardsKnockback);
				}
				EditorGUILayout.EndVertical();
				break;

				case "Animation":
				EditorGUILayout.BeginVertical(UIHelper.MainStyle);
				{
					EditorGUILayout.PropertyField(animator);

					if (myObject.animator != null)
					{
						EditorGUILayout.PropertyField(attackTriggerParameterName);
					}
				}
				EditorGUILayout.EndVertical();
				break;

				case "FX":
				EditorGUILayout.BeginVertical(UIHelper.MainStyle);
				{
					EditorGUILayout.PropertyField(hitFX);
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

			switch (toolBarTab)
			{
				case 0:

				break;

				case 1:

				break;

				case 2:

				break;
			}
			#endregion
		}
		EditorGUILayout.EndVertical();
	}

	private void OnSceneGUI ()
	{
		if(myObject.displayDebugInfo)
		{
			UIHelper.DrawArc(myObject.attackAngle, myObject.attackRange, myObject.transform);
		}
	}
}
