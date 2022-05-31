using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Life))]
public class LifeEditor : Editor
{
	Life myObject;
	SerializedObject soTarget;

	private SerializedProperty maxLife;

	private SerializedProperty invincibilityDuration;

	private SerializedProperty animator;
	private SerializedProperty hitParameterName;

	private void OnEnable ()
	{
		myObject = (Life)target;
		soTarget = new SerializedObject(target);

		////

		maxLife = soTarget.FindProperty("maxLife");
		invincibilityDuration = soTarget.FindProperty("invincibilityDuration");

		animator = soTarget.FindProperty("animator");
		hitParameterName = soTarget.FindProperty("hitParameterName");
	}

	public override void OnInspectorGUI ()
	{
		UIHelper.InitializeStyles();
		
		soTarget.Update();
		EditorGUI.BeginChangeCheck();

		if(!Application.isPlaying)
		{
			myObject.CurrentLife = myObject.startLife;
		}

		EditorGUILayout.BeginHorizontal(UIHelper.MainStyle);
		{
			Rect space = GUILayoutUtility.GetRect(GUIContent.none, UIHelper.MainStyle, GUILayout.Height(20), GUILayout.Width(EditorGUIUtility.currentViewWidth));
			EditorGUI.ProgressBar(space, (float)myObject.CurrentLife / (float)myObject.maxLife, "Current Life");
		}
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.BeginVertical(UIHelper.MainStyle);
		{
			EditorGUILayout.BeginHorizontal(UIHelper.SubStyle1);
			{
				EditorGUILayout.PropertyField(maxLife);
				EditorGUILayout.LabelField("Start Life ", GUILayout.MaxWidth(80));
				myObject.startLife = EditorGUILayout.IntSlider(myObject.startLife, 0, myObject.maxLife);
			}

			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.BeginHorizontal(UIHelper.SubStyle1);
			{
				EditorGUILayout.PropertyField(invincibilityDuration);
			}
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginVertical(UIHelper.SubStyle1);
			{
				EditorGUILayout.PropertyField(animator);

				if (myObject.animator != null)
				{
					EditorGUILayout.BeginVertical(UIHelper.SubStyle2);
					{
						EditorGUILayout.PropertyField(hitParameterName);
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
