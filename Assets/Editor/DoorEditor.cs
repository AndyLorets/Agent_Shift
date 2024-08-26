using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Door))]
public class DoorEditor : Editor
{
    SerializedProperty interactSpriteProp;
    SerializedProperty playerIconProp;
    SerializedProperty dialogueProp;
    SerializedProperty interactHandlerProp;
    SerializedProperty unlockTypeProp;
    SerializedProperty doorCodeProp;
    SerializedProperty doorUnlockerProp;
    SerializedProperty taskName;

    private void OnEnable()
    {
        interactSpriteProp = serializedObject.FindProperty("_interactSprite");
        playerIconProp = serializedObject.FindProperty("_playerIcon");
        dialogueProp = serializedObject.FindProperty("_dialogue");
        interactHandlerProp = serializedObject.FindProperty("_interactHandler");
        unlockTypeProp = serializedObject.FindProperty("_unlockType");
        doorCodeProp = serializedObject.FindProperty("_doorCode");
        doorUnlockerProp = serializedObject.FindProperty("_doorUnlocker");
        taskName = serializedObject.FindProperty("_taskName");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // Отображаем все поля, кроме зависящих от UnlockType
        EditorGUILayout.PropertyField(interactSpriteProp);
        EditorGUILayout.PropertyField(playerIconProp);
        EditorGUILayout.PropertyField(dialogueProp);
        EditorGUILayout.PropertyField(interactHandlerProp);
        EditorGUILayout.PropertyField(unlockTypeProp);

        // Отображаем поля, если UnlockType равен Code
        if ((Door.UnlockType)unlockTypeProp.enumValueIndex == Door.UnlockType.Code)
        {
            EditorGUILayout.PropertyField(doorCodeProp);
            EditorGUILayout.PropertyField(doorUnlockerProp);
        }
        if ((Door.UnlockType)unlockTypeProp.enumValueIndex == Door.UnlockType.Task)
        {
            EditorGUILayout.PropertyField(taskName);
        }
        serializedObject.ApplyModifiedProperties();
    }
}
