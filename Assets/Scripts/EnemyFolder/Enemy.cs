using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyData enemyData;

    public string enemyName;
    public string enemyDescription;

    int enemyCurrentHealth;
    int enemyMaxHealth;

    int enemySpeed;

    int enemyDamageDeal;

    float enemyAttackTime;

    bool enemyCanFly;

    public Rigidbody body;

    public Transform target;

    //Enemy target
    //GameObject target;

    void Start()
    {
        enemyName = enemyData.name;
        enemyDescription = enemyData.description;

        enemyMaxHealth = enemyData.health;
        enemyCurrentHealth = enemyMaxHealth;

        enemySpeed = enemyData.speed;

        enemyDamageDeal = enemyData.damageDeal;

        enemyAttackTime = enemyData.attackTime;

        enemyCanFly = enemyData.canFly;

        DebugLog();
    }

    void Update()
    {
        if (target != null)
        {
            transform.LookAt(target.position);
            transform.Translate(Vector3.forward * enemySpeed * Time.deltaTime);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        target = null;
    }

    void DebugLog()
    {
        Debug.Log( $"Enemy type {enemyName}: " + enemyDamageDeal);
    }

    void SwitchEnemyTarget()
    {
        //target = FindObjectOfType < house or wall or whatever > ();
        //if wall is in front, switch the attack target to wall, else go base
    }

}
