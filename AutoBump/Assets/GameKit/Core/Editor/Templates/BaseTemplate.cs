using UnityEngine;
using UnityEditor;

//Change all Dummyclass references into the class name
[CustomEditor(typeof(BaseDummyClass))]
public class BaseTemplate : Editor
{
	[Tooltip("Used to check public variables from the target class")]
	private BaseDummyClass myObject;
	private SerializedObject soTarget;

	private SerializedProperty exampleString1;

	private void OnEnable ()
	{
		myObject = (BaseDummyClass)target;
		soTarget = new SerializedObject(target);

		exampleString1 = soTarget.FindProperty("exampleString1");
	}

	public override void OnInspectorGUI ()
	{
		UIHelper.InitializeStyles();

		soTarget.Update();
		EditorGUI.BeginChangeCheck();

		EditorGUILayout.BeginVertical(UIHelper.MainStyle);
		{
			EditorGUILayout.PropertyField(exampleString1);

			EditorGUILayout.BeginHorizontal(UIHelper.SubStyle1);
			{
				EditorGUILayout.LabelField("Min Max Slider : ", GUILayout.MaxWidth(100f));
				myObject.examplefloat1 = EditorGUILayout.FloatField(myObject.examplefloat1, GUILayout.MaxWidth(50f));

				EditorGUILayout.MinMaxSlider(ref myObject.examplefloat1, ref myObject.examplefloat2, -10f, 10f);

				myObject.examplefloat2 = EditorGUILayout.FloatField(myObject.examplefloat2, GUILayout.MaxWidth(50f));
			}
			EditorGUILayout.EndHorizontal();
		}
		EditorGUILayout.EndVertical();


		if (EditorGUI.EndChangeCheck())
		{
			soTarget.ApplyModifiedProperties();
		}
	}
}