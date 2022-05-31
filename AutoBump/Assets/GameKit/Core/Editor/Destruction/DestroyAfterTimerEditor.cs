using UnityEditor;
using UnityEngine;

//Change all Dummyclass references into the class name
[CustomEditor(typeof(DestroyAfterTimer))]
public class DestroyAfterTimerEditor : Editor
{
    [Tooltip("Used to check public variables from the target class")]
    private DestroyAfterTimer myObject;
    private SerializedObject soTarget;

    private SerializedProperty lifeTime;

    private void OnEnable ()
    {
        myObject = (DestroyAfterTimer)target;
        soTarget = new SerializedObject(target);

        lifeTime = soTarget.FindProperty("lifeTime");
    }

    public override void OnInspectorGUI()
    {
        UIHelper.InitializeStyles();

        soTarget.Update();
        EditorGUI.BeginChangeCheck();

        EditorGUILayout.BeginHorizontal(UIHelper.MainStyle);
        {
            EditorGUILayout.PropertyField(lifeTime);
        }
        EditorGUILayout.EndHorizontal();
        
        if (EditorGUI.EndChangeCheck())
        {
            soTarget.ApplyModifiedProperties();
        }
    }
}