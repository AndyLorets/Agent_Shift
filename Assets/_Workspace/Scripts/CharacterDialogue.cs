using UnityEngine;

[System.Serializable]
public struct CharacterDialogue 
{
    [TextArea(4, 4)] public string text; 
    public AudioClip clip;

    public static bool speaking; 
}
