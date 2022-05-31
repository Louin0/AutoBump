using UnityEngine;
using UnityEditor;

//Change all Dummyclass references into the class name
[CustomEditor(typeof(EventBase))]
public class EventBaseEditor : Editor
{
    [Tooltip("Used to check public variables from the target class")]
    private EventBase myObject;
    private SerializedObject soTarget;

    private SerializedProperty loop;
    private SerializedProperty triggeredEvents;

    private void OnEnable ()
    {
        myObject = (EventBase)target;
        soTarget = new SerializedObject(target);

        loop = soTarget.FindProperty("loop");
        triggeredEvents = soTarget.FindProperty("triggeredEvents");
    }

    public override void OnInspectorGUI ()
    {
        UIHelper.InitializeStyles();

        soTarget.Update();
        EditorGUI.BeginChangeCheck();

        EditorGUILayout.BeginVertical(UIHelper.MainStyle);
        {
            EditorGUILayout.HelpBox("This script is mostly used as a base to trigger Events, you might want to use the other Event scripts instead.", MessageType.Warning, true);
            EditorGUILayout.PropertyField(loop);
            
            EditorGUILayout.BeginHorizontal(UIHelper.SubStyle1);
            {
                EditorGUILayout.PropertyField(triggeredEvents);
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