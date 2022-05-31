using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CameraShakeOnDestroy))]
public class CameraShakeOnDestroyEditor : Editor
{
	[Tooltip("Used to check public variables from the target class")]
	private CameraShakeOnDestroy myObject;
	private SerializedObject soTarget;

	private SerializedProperty shakeDuration;
	private SerializedProperty intensity;
	private SerializedProperty targetToShake;
	private SerializedProperty sceneManager;

	private void OnEnable ()
	{
		myObject = (CameraShakeOnDestroy)target;
		soTarget = new SerializedObject(target);

		shakeDuration = soTarget.FindProperty("shakeDuration");
		intensity = soTarget.FindProperty("intensity");
		targetToShake = soTarget.FindProperty("targetToShake");
		sceneManager = soTarget.FindProperty("sceneManager");
	}

	public override void OnInspectorGUI ()
	{
		UIHelper.InitializeStyles();

		EditorGUILayout.BeginVertical(UIHelper.MainStyle);
		{
			EditorGUILayout.BeginVertical(UIHelper.SubStyle1);
			{
				EditorGUILayout.PropertyField(targetToShake);
				EditorGUILayout.PropertyField(sceneManager);
			}
			EditorGUILayout.EndVertical();

			EditorGUILayout.BeginVertical(UIHelper.SubStyle1);
			{
				EditorGUILayout.PropertyField(shakeDuration);
				EditorGUILayout.PropertyField(intensity);
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
