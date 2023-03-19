using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Wave", menuName = "Wave")]
public class Wave : ScriptableObject
{
    public EnemySet enemySet;
    public float spawnRate = 1.0f;
    public float waveTimeLength = 180.0f;
    public Vector2 spawnLocAmountRange = new Vector2(5, 10);
    public int maxEnemies = 100;
}
