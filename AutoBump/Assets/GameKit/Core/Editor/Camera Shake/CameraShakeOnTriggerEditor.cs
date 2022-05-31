using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CameraShakeOnTrigger))]
public class CameraShakeOnTriggerEditor : Editor
{
	[Tooltip("Used to check public variables from the target class")]
	private CameraShakeOnTrigger myObject;
	private SerializedObject soTarget;

	private SerializedProperty targetToShake;
	private SerializedProperty useTag;
	private SerializedProperty intensity;
	private SerializedProperty shakeDuration;
	private SerializedProperty triggerOnce;

	private void OnEnable ()
	{
		myObject = (CameraShakeOnTrigger)target;
		soTarget = new SerializedObject(target);

		targetToShake = soTarget.FindProperty("targetToShake");
		useTag = soTarget.FindProperty("useTag");

		intensity = soTarget.FindProperty("intensity");
		shakeDuration = soTarget.FindProperty("shakeDuration");
		triggerOnce = soTarget.FindProperty("triggerOnce");

	}

	public override void OnInspectorGUI ()
	{
		UIHelper.InitializeStyles();

		EditorGUILayout.BeginVertical(UIHelper.MainStyle);
		{
			EditorGUILayout.BeginVertical(UIHelper.SubStyle1);
			{
				EditorGUILayout.PropertyField(targetToShake);
			}
			EditorGUILayout.EndVertical();

			EditorGUILayout.BeginVertical(UIHelper.SubStyle1);
			{
				EditorGUILayout.PropertyField(triggerOnce);
				EditorGUILayout.BeginHorizontal(UIHelper.SubStyle2);
				{
					EditorGUILayout.PropertyField(useTag);
					if(myObject.useTag)
					{
						myObject.tagName = EditorGUILayout.TagField(myObject.tagName);
					}
				}
				EditorGUILayout.EndHorizontal();
			}
			EditorGUILayout.EndVertical();

			EditorGUILayout.BeginVertical(UIHelper.SubStyle1);
			{
				EditorGUILayout.PropertyField(intensity);
				EditorGUILayout.PropertyField(shakeDuration);

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
