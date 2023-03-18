using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.IMGUI.Controls;
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
    Transform finalTarget;
    public NavMeshAgent agent;
    public CapsuleCollider capsuleCollider;

    public BoxCollider targetBoxCollider; //should be farm or wall collider
    public SphereCollider enemySphereCollider;

    float timerLenght = 5.0f;
    float timeGoing;
    bool startedCountdown = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        enemySphereCollider = GetComponent<SphereCollider>();

        targetBoxCollider = target.GetComponent<BoxCollider>();


        enemyName = enemyData.name;
        enemyDescription = enemyData.description;

        enemyMaxHealth = enemyData.health;
        enemyCurrentHealth = enemyMaxHealth;

        enemySpeed = enemyData.speed;
        agent.speed = enemySpeed; 

        enemyDamageDeal = enemyData.damageDeal;

        enemyAttackTime = enemyData.attackTime;

        enemyRadiusMultiply = enemyData.radiusMultiply;

        finalTarget = target;

        DebugLog();
    }

    void Update()
    {
        EnemyMove();
    }

    private void LateUpdate()
    {
        if (startedCountdown)
        {
            timeGoing += Time.deltaTime;
            Debug.Log(timeGoing);
            if (timeGoing > timerLenght)
            {
                timeGoing = 0;
                startedCountdown = false;
                SwitchEnemyTarget(finalTarget);
                Debug.Log("Timer expired");
                //agent.speed = enemySpeed;
            }
        }
    }

    

    void OnTriggerEnter(Collider collider)
    {
        
        if (collider.gameObject.name == "AttackTarget")
        {
            SwitchEnemyTarget(collider.gameObject.transform);
            startedCountdown = true;
            Debug.Log(startedCountdown);
            Debug.Log($"Ran into this thing: {collider.gameObject.name}");
        }

        if (collider.gameObject.name == target.name)
        {           
            agent.speed = 0f;
            EnemyAttack();
            Debug.Log($"Enemy type {enemyName} speed is: " + agent.speed);
            Debug.Log($"Collider name: {collider.name}");
        }
    }

    // Dictionary that makes lists based on priority numbers
  

    void DebugLog()
    {
        Debug.Log( $"Enemy type {enemyName}: " + enemyDamageDeal);
    }

    public void SwitchEnemyTarget(Transform pTarget)
    {
        Debug.Log($"Should've switched target to {pTarget.name}");
        target = pTarget;
        agent.speed = enemySpeed;


        //target = FindObjectOfType < house or wall or whatever > ();
        //priority: highest damage
        //Needs sphere collider for triggers
    }


    protected virtual void EnemyMove()
    {
        if (target != null)
        {

            var pos = target.position;
            pos.y = 0;
            agent.SetDestination(pos);
        }

        //^This will be replaced by farm house and the farm house radius
    }
    protected virtual void EnemyAttack()
    {
        //If sphere collider is trigger colliding with wall or base - attack
        Debug.Log("Attacked target");
    }    
}


//If the enemy is colliding(it's box collision) with anything that's a tower, it deals damage