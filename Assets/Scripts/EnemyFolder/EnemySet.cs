using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New EnemySet", menuName = "EnemySet")]
public class EnemySet : ScriptableObject
{
    public List<EnemyData> m_enemies;
}
