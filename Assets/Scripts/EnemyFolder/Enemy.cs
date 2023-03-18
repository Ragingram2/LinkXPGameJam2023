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
    GameObject target;
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
        target = GameObject.Find("TestTarget");
        finalTarget = target.transform;

        agent = GetComponent<NavMeshAgent>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        enemySphereCollider = GetComponent<SphereCollider>();

        targetBoxCollider = finalTarget.GetComponent<BoxCollider>();

        enemyName = enemyData.name;
        enemyDescription = enemyData.description;

        enemyMaxHealth = enemyData.health;
        enemyCurrentHealth = enemyMaxHealth;

        enemySpeed = enemyData.speed;
        agent.speed = enemySpeed; 

        enemyDamageDeal = enemyData.damageDeal;

        enemyAttackTime = enemyData.attackTime;

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

    private void OnCollisionStay(Collision collision)
    {
        Tower temp;
        if (collision.gameObject.TryGetComponent<Tower>(out temp))
        {
            //call tower take damage
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

    void DebugLog()
    {
        Debug.Log( $"Enemy type {enemyName}: " + enemyDamageDeal);
    }


    //If attacked by tower, it targets it, and when it gets within range, it attacks
    public void SwitchEnemyTarget(Transform pTarget)
    {
        Debug.Log($"Should've switched target to {pTarget.name}");
        finalTarget = pTarget;
        agent.speed = enemySpeed;


        //target = FindObjectOfType < house or wall or whatever > ();
        //priority: highest damage
        //Needs sphere collider for triggers
    }

    public void EnemyTakesDamage(int _damageAmmount)
    {
        enemyCurrentHealth -= _damageAmmount;
    }


    protected virtual void EnemyMove()
    {
        if (target != null)
        {

            var pos = finalTarget.position;
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


//1. If the enemy is colliding(it's box collision) with anything that's a tower, it deals damage
//2. Enemy attack - time between attacks, type of attack etc.
//3. Switch enemy target - if attacked, change target to it, if there's no target, change it back to farm
//4. Enemy move is using navmesh - setdestinantion