using System.Collections.Generic;
using UnityEngine;

public class EnemyLureSpeakerManager : MonoBehaviour
{
    public List<EnemyLureSpeaker> lureSpeakerList { get; private set; } = new List<EnemyLureSpeaker>();
    private void Awake()
    {
        ServiceLocator.RegisterService(this);
        EnemyLureSpeaker[] enemyLureSpeaker = GetComponentsInChildren<EnemyLureSpeaker>();
        lureSpeakerList.AddRange(enemyLureSpeaker);
    }
}
