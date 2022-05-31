using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CameraShaker))]
public class CameraShakerEditor : Editor
{
	[Tooltip("Used to check public variables from the target class")]
	private CameraShaker myObject;
	private SerializedObject soTarget;

	private SerializedProperty shakeOnXAxis;
	private SerializedProperty shakeOnYAxis;
	private SerializedProperty shakeOnZAxis;

	private SerializedProperty intensityCurveX;
	private SerializedProperty intensityCurveY;
	private SerializedProperty intensityCurveZ;


	private void OnEnable ()
	{
		myObject = (CameraShaker)target;
		soTarget = new SerializedObject(target);

		shakeOnXAxis = soTarget.FindProperty("shakeOnXAxis");
		shakeOnYAxis = soTarget.FindProperty("shakeOnYAxis");
		shakeOnZAxis = soTarget.FindProperty("shakeOnZAxis");

		intensityCurveX = soTarget.FindProperty("intensityCurveX");
		intensityCurveY = soTarget.FindProperty("intensityCurveY");
		intensityCurveZ = soTarget.FindProperty("intensityCurveZ");
	}

	public override void OnInspectorGUI ()
	{
		UIHelper.InitializeStyles();

		EditorGUILayout.BeginHorizontal(UIHelper.MainStyle);
		{
			EditorGUILayout.PropertyField(shakeOnXAxis);

			if(myObject.shakeOnXAxis)
			{
				EditorGUILayout.PropertyField(intensityCurveX);
			}
		}
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal(UIHelper.MainStyle);
		{
			EditorGUILayout.PropertyField(shakeOnYAxis);

			if (myObject.shakeOnYAxis)
			{
				EditorGUILayout.PropertyField(intensityCurveY);
			}
		}
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal(UIHelper.MainStyle);
		{
			EditorGUILayout.PropertyField(shakeOnZAxis);

			if (myObject.shakeOnZAxis)
			{
				EditorGUILayout.PropertyField(intensityCurveZ);
			}
		}
		EditorGUILayout.EndHorizontal();


		if (EditorGUI.EndChangeCheck())
		{
			soTarget.ApplyModifiedProperties();
		}
	}

}