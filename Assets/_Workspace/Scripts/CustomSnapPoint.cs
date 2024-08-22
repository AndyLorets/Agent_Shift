using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomSnapPoint : MonoBehaviour
{
    public enum ConnectionType
    {
        Wall, Floor
    }
    public ConnectionType Type;

    private void OnDrawGizmos()
    {
        switch (Type)
        {
            case ConnectionType.Floor: 
                Gizmos.color = Color.blue;
                break;
            case ConnectionType.Wall:
                Gizmos.color = Color.gray;
                break;
        }
        Gizmos.DrawSphere(transform.position, .1f);
    }
}
