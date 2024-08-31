using UnityEngine;
using System.Collections.Generic; 
public class EnemyManager : MonoBehaviour
{
    public List<Character> enemiesList { get; private set; } = new List<Character>();
    private void Awake()
    {
        ServiceLocator.RegisterService(this);
        Enemy[] enemies = GetComponentsInChildren<Enemy>();
        enemiesList.AddRange(enemies);
    }
    private void Start()
    {
        Construct(); 
    }
    private void OnDestroy()
    {
        Reconstruct(); 
    }
    private void Construct()
    {
        for (int i = 0; i < enemiesList.Count; i++)
        {
            enemiesList[i].onDead += RemoveEnemyFromList; 
        }
    }
    private void Reconstruct()
    {
        for (int i = 0; i < enemiesList.Count; i++)
        {
            enemiesList[i].onDead -= RemoveEnemyFromList;
        }
    }
    private void RemoveEnemyFromList(Character character)
    {
        Enemy enemy = character as Enemy;
        enemy.onDead -= RemoveEnemyFromList; 
        enemiesList.Remove(enemy); 
    }
}
