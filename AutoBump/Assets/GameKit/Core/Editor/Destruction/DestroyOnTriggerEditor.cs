using UnityEditor;
using UnityEngine;

//Change all Dummyclass references into the class name
[CustomEditor(typeof(DestroyOnTrigger))]
public class DestroyOnTriggerEditor : Editor
{
    [Tooltip("Used to check public variables from the target class")]
    private DestroyOnTrigger myObject;
    private SerializedObject soTarget;

    private SerializedProperty useTag;

    private void OnEnable ()
    {
        myObject = (DestroyOnTrigger)target;
        soTarget = new SerializedObject(target);

        useTag = soTarget.FindProperty("useTag");
    }

    public override void OnInspectorGUI()
    {
        UIHelper.InitializeStyles();

        soTarget.Update();
        EditorGUI.BeginChangeCheck();

        EditorGUILayout.BeginHorizontal(UIHelper.MainStyle);
        {
            EditorGUILayout.PropertyField(useTag);

            if (myObject.useTag)
            {
                myObject.tagName = EditorGUILayout.TagField(myObject.tagName);
            }
        }
        EditorGUILayout.EndHorizontal();
        
        if (EditorGUI.EndChangeCheck())
        {
            soTarget.ApplyModifiedProperties();
        }
    }
}
