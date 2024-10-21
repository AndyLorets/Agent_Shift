using UnityEngine;

public static class GetColorFromHex
{
    public static Color GetColor(string hex)
    {
        Color newColor;
        if (ColorUtility.TryParseHtmlString(hex, out newColor))
        {
            return newColor;
        }
        else
        {
            Debug.LogWarning("Invalid hex string. Returning default color (white).");
            return Color.white; 
        }
    }
}
