using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy")]
public class EnemyData : ScriptableObject
{

    public new string name;
    public string description;
    public Sprite model;

    public int health;
    public int speed;
    public int damageDeal;
    public float attackTime;

    public bool canFly;


}
