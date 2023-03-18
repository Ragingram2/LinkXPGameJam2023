using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public EnemyData enemyData;

    public string enemyName;
    public string enemyDescription;

    int enemyCurrentHealth;
    int enemyMaxHealth;

    float enemySpeed;
    public float radiusMultiply;

    int enemyDamageDeal;

    float enemyAttackTime;

    int enemyRadiusMultiply;

    //public Rigidbody body;
    public Transform target;
    public NavMeshAgent agent;
    public CapsuleCollider capsuleCollider;
    

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

        enemyRadiusMultiply = enemyData.radiusMultiply;

        DebugLog();
    }

    void Update()
    {
        EnemyMove();
    }


    void OnTriggerEnter(Collider other)
    {
        //agent.SetDestination(transform.position);
        Debug.Log($"Enemy type {enemyName} is going here: " + transform.position);
    }

    void DebugLog()
    {
        Debug.Log( $"Enemy type {enemyName}: " + enemyDamageDeal);
    }

    void EnemyMove()
    {
        if (target != null)
        {
            agent.speed = enemySpeed;
            var pos = target.position;
            pos.y = 0;
            agent.SetDestination(pos);
        }

        if (agent.remainingDistance < capsuleCollider.radius * enemyRadiusMultiply)
        {
            agent.speed = 0f;
        }
        //^This will be replaced by farm house and the farm house radius
    }


    void SwitchEnemyTarget()
    {
        //target = FindObjectOfType < house or wall or whatever > ();
        //if wall is in front, switch the attack target to wall, else go base
    }

}
