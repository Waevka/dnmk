#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DnmkStage))]
[CanEditMultipleObjects]
public class StageInspector : Editor {

    public static GUIContent moveButton     = new GUIContent("mv down" , "Move event one entry down.");
    public static GUIContent addButton      = new GUIContent("+", "Adds a new event.");
    public static GUIContent deleteButton   = new GUIContent("-", "Deletes this event from the list.");

    static SerializedProperty StageEventList;
    static SerializedProperty StageEventTime;

    void OnEnable()
    {
        StageEventList = serializedObject.FindProperty("dnmkEventList");
        StageEventTime = serializedObject.FindProperty("dnmkEventTime");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        if (StageEventList != null && StageEventTime != null)
        {
            GUILayout.Label("Total event count in stage: " + StageEventList.arraySize);
            DisplayEventData();
            if (GUILayout.Button(addButton))
            {
                StageEventList.arraySize += 1;
                StageEventTime.arraySize += 1;
            }
        }
        else
        {
            Debug.LogWarning("StageEventList or StageEventTime is incorrectly defined (probalby changed variable name)", this);
        }
        serializedObject.ApplyModifiedProperties();
    }

    private static void DisplayEventData()
    {
        for (int i = 0; i < StageEventList.arraySize; i++)
        {
            EditorGUIUtility.labelWidth = 50.0f;
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(StageEventList.GetArrayElementAtIndex(i), new GUIContent("Event"));
            EditorGUILayout.PropertyField(StageEventTime.GetArrayElementAtIndex(i), new GUIContent("Time"), GUILayout.Width(100.0f));
            DisplayButtons(i);
            EditorGUILayout.EndHorizontal();
        }
    }

    private static void DisplayButtons(int i)
    {
        if (GUILayout.Button(moveButton, EditorStyles.miniButtonLeft, GUILayout.Width(20f))) {
            StageEventList.MoveArrayElement(i, i+1);
        }
        if (GUILayout.Button(addButton, EditorStyles.miniButtonMid, GUILayout.Width(20f))) {
            StageEventList.InsertArrayElementAtIndex(i);
        }
        if (GUILayout.Button(deleteButton, EditorStyles.miniButtonRight, GUILayout.Width(20f))) {
            StageEventList.DeleteArrayElementAtIndex(i);
        }
    }
}
#endif
