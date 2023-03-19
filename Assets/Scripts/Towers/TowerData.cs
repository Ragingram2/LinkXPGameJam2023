using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tower", menuName = "Tower")]
public class TowerData : ScriptableObject
{
    public int m_health = 100;
    public float m_fireRate = 1.0f;
    public float m_damage = 10.0f;
    public float m_range = 30.0f;
}
