using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CameraShakeOnInput))]
public class CameraShakeOnInputEditor : Editor
{
	[Tooltip("Used to check public variables from the target class")]
	private CameraShakeOnInput myObject;
	private SerializedObject soTarget;

	private SerializedProperty shakeDuration;
	private SerializedProperty intensity;

	private SerializedProperty inputName;

	private SerializedProperty useCooldown;
	private SerializedProperty cooldown;

	private SerializedProperty onlyOnce;
	private SerializedProperty targetToShake;


	private void OnEnable ()
	{
		myObject = (CameraShakeOnInput)target;
		soTarget = new SerializedObject(target);

		targetToShake = soTarget.FindProperty("targetToShake");

		shakeDuration = soTarget.FindProperty("shakeDuration");
		intensity = soTarget.FindProperty("intensity");

		inputName = soTarget.FindProperty("inputName");
		useCooldown = soTarget.FindProperty("useCooldown");
		cooldown = soTarget.FindProperty("cooldown");

		onlyOnce = soTarget.FindProperty("onlyOnce");
	}

	public override void OnInspectorGUI ()
	{
		UIHelper.InitializeStyles();

		EditorGUILayout.BeginVertical(UIHelper.MainStyle);
		{

			EditorGUILayout.BeginVertical(UIHelper.SubStyle1);
			{
				EditorGUILayout.PropertyField(targetToShake);
				EditorGUILayout.PropertyField(onlyOnce);
			}
			EditorGUILayout.EndVertical();

			EditorGUILayout.BeginVertical(UIHelper.SubStyle1);
			{
				EditorGUILayout.PropertyField(inputName);
			}
			EditorGUILayout.EndVertical();

			EditorGUILayout.BeginVertical(UIHelper.SubStyle1);
			{
				EditorGUILayout.PropertyField(shakeDuration);
				EditorGUILayout.PropertyField(intensity);

			}
			EditorGUILayout.EndVertical();

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


		if (EditorGUI.EndChangeCheck())
		{
			soTarget.ApplyModifiedProperties();
		}
	}
}
