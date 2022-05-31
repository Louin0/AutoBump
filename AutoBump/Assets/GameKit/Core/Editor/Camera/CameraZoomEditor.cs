using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(CameraZoom))]
public class CameraZoomEditor : Editor
{
	[Tooltip("Used to check public variables from the target class")]
	CameraZoom myObject;
	SerializedObject soTarget;

	private SerializedProperty zoomCurve;

	private SerializedProperty sensitivity;
	private SerializedProperty smoothSpeed;

	private SerializedProperty zoomInputName;
	private SerializedProperty usedCamera;


	private void OnEnable ()
	{
		myObject = (CameraZoom)target;
		soTarget = new SerializedObject(target);

		zoomCurve = soTarget.FindProperty("zoomCurve");

		sensitivity = soTarget.FindProperty("sensitivity");
		smoothSpeed = soTarget.FindProperty("smoothSpeed");
		zoomInputName = soTarget.FindProperty("zoomInputName");

		usedCamera = soTarget.FindProperty("usedCamera");
	}

	public override void OnInspectorGUI ()
	{
		UIHelper.InitializeStyles();

		soTarget.Update();
		EditorGUI.BeginChangeCheck();

		EditorGUILayout.BeginVertical(UIHelper.MainStyle);
		{
			EditorGUILayout.PropertyField(usedCamera);

			if(myObject.usedCamera != null)
			{
				EditorGUILayout.BeginVertical(UIHelper.SubStyle1);
				{
					EditorGUILayout.PropertyField(zoomInputName);
				}
				EditorGUILayout.EndVertical();

				EditorGUILayout.BeginVertical(UIHelper.SubStyle1);
				{
					EditorGUILayout.PropertyField(zoomCurve);
				}
				EditorGUILayout.EndVertical();

				EditorGUILayout.BeginHorizontal(UIHelper.SubStyle1);
				{
					EditorGUILayout.LabelField("Zoom FOV : ", GUILayout.MaxWidth(100f));
					myObject.minZoomFOV = EditorGUILayout.FloatField(myObject.minZoomFOV, GUILayout.MaxWidth(50f));
					EditorGUILayout.MinMaxSlider(ref myObject.minZoomFOV, ref myObject.maxZoomFOV, 5f, 120f);
					myObject.maxZoomFOV = EditorGUILayout.FloatField(myObject.maxZoomFOV, GUILayout.MaxWidth(50f));
				}
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal(UIHelper.SubStyle1);
				{
					EditorGUILayout.PropertyField(sensitivity);
					EditorGUILayout.PropertyField(smoothSpeed);
				}
				EditorGUILayout.EndHorizontal();
			}
		}
		EditorGUILayout.EndVertical();

		if (EditorGUI.EndChangeCheck())
		{
			soTarget.ApplyModifiedProperties();
		}
	}
}
