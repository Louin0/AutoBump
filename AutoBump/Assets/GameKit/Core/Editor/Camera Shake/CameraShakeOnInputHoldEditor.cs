using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CameraShakeOnInputHold))]
public class CameraShakeOnInputHoldEditor : Editor
{
	[Tooltip("Used to check public variables from the target class")]
	private CameraShakeOnInputHold myObject;
	private SerializedObject soTarget;

	private SerializedProperty shakerTransform;
	private SerializedProperty shakeInputName;
	private SerializedProperty intensity;
	private SerializedProperty frequency;
	private SerializedProperty smoothTime;

	private void OnEnable ()
	{
		myObject = (CameraShakeOnInputHold)target;
		soTarget = new SerializedObject(target);

		shakerTransform = soTarget.FindProperty("shakerTransform");
		shakeInputName = soTarget.FindProperty("shakeInputName");
		intensity = soTarget.FindProperty("intensity");
		frequency = soTarget.FindProperty("frequency");
		smoothTime = soTarget.FindProperty("smoothTime");

	}

	public override void OnInspectorGUI ()
	{
		UIHelper.InitializeStyles();

		EditorGUILayout.BeginVertical(UIHelper.MainStyle);
		{
			EditorGUILayout.BeginVertical(UIHelper.SubStyle1);
			{
				EditorGUILayout.PropertyField(shakerTransform);
			}
			EditorGUILayout.EndVertical();

			EditorGUILayout.BeginVertical(UIHelper.SubStyle1);
			{
				EditorGUILayout.PropertyField(shakeInputName);
			}
			EditorGUILayout.EndVertical();

			EditorGUILayout.BeginVertical(UIHelper.SubStyle1);
			{
				EditorGUILayout.PropertyField(intensity);
				EditorGUILayout.PropertyField(frequency);
				EditorGUILayout.PropertyField(smoothTime);

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
