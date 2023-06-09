using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy")]
public class EnemyData : ScriptableObject
{
    public new string name;
    public string description;
    public RuntimeAnimatorController controller;

    public int maxHealth;
    public float speed;
    public int damageDeal;
    public float attackTime;
    public float attackRadius;
}