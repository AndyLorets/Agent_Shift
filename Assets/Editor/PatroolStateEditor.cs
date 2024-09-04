using UnityEditor;
[CustomEditor(typeof(PatroolState))]
public class PatroolStateEditor : Editor
{
    SerializedProperty _patroolTransform;
    SerializedProperty _cubeSize;
    SerializedProperty _points;
    SerializedProperty _waitTime;
    SerializedProperty _patroolType;
    SerializedProperty _drawGizmos;

    private void OnEnable()
    {
        _patroolTransform = serializedObject.FindProperty("_patroolTransform");
        _cubeSize = serializedObject.FindProperty("_cubeSize");
        _points = serializedObject.FindProperty("_points");
        _waitTime = serializedObject.FindProperty("_waitTime");
        _patroolType = serializedObject.FindProperty("_patroolType");
        _drawGizmos = serializedObject.FindProperty("_drawGizmos");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // Отображаем все поля, кроме зависящих от Type
        EditorGUILayout.PropertyField(_patroolType);
        EditorGUILayout.PropertyField(_waitTime);
        EditorGUILayout.PropertyField(_drawGizmos);

        if ((PatroolState.PatroolType)_patroolType.enumValueIndex == PatroolState.PatroolType.Random)
        {
            EditorGUILayout.PropertyField(_patroolTransform);
            EditorGUILayout.PropertyField(_cubeSize);
        }
        if ((PatroolState.PatroolType)_patroolType.enumValueIndex == PatroolState.PatroolType.Points)
        {
            EditorGUILayout.PropertyField(_points);
        }
        serializedObject.ApplyModifiedProperties();
    }
}
