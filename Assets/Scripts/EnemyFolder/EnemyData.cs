using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;


[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy")]
public class EnemyData : ScriptableObject
{
    public new string name;
    public string description;
    public AnimatorController controller;

    public int maxHealth;
    public float speed;
    public int damageDeal;
    public float attackTime;
    public float attackRadius;
}

//Make radius multiply here instead of Enemy.cs