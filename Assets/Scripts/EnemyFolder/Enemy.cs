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

    int enemyDamageDeal;

    float enemyAttackTime;
    float enemyAttackRadius;

    //public Rigidbody body;
    GameObject target;
    Transform currentTarget;
    Transform finalTarget;
    public NavMeshAgent agent;
    public CapsuleCollider capsuleCollider;

    public BoxCollider targetBoxCollider; //should be farm or wall collider
    public SphereCollider enemySphereCollider;

    //float timerLenght = 5.0f;
    //float timeGoing;
    //bool startedCountdown = false;

    float timerThing = 0f;

    void Start()
    {
        target = GameObject.Find("TestTarget");
        currentTarget = target.transform;
        finalTarget = target.transform;

        agent = GetComponent<NavMeshAgent>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        enemySphereCollider = GetComponent<SphereCollider>();

        targetBoxCollider = currentTarget.GetComponent<BoxCollider>();//when towers, capsule

        enemyName = enemyData.name;
        enemyDescription = enemyData.description;

        enemyMaxHealth = enemyData.maxHealth;
        enemyCurrentHealth = enemyMaxHealth;

        enemySpeed = enemyData.speed;
        agent.speed = enemySpeed; 

        enemyDamageDeal = enemyData.damageDeal;

        enemyAttackTime = enemyData.attackTime;

        enemyAttackRadius = enemyData.attackRadius;
        enemySphereCollider.radius = enemyAttackRadius;

        DebugLog();
    }

    void Update()
    {
        EnemyMove();
    }

    private void LateUpdate()
    {
        //if (startedCountdown)
        //{
        //    timeGoing += Time.deltaTime;
        //    if (timeGoing > timerLenght)
        //    {
        //        timeGoing = 0;
        //        startedCountdown = false;
        //        SwitchEnemyTarget(finalTarget);
        //        Debug.Log("Timer expired");
        //        //agent.speed = enemySpeed;
        //    }
        //}
    }

    private void OnCollisionStay(Collision collision)
    {
        Tower temp;
        if (collision.gameObject.TryGetComponent<Tower>(out temp))
        {
            //deal damage to tower with enemyDamageDeal variable
        }
    }

    void OnTriggerStay(Collider collider)
    {
        
        EnemyAttack();
        timerThing += Time.deltaTime;
        transform.LookAt(collider.transform.position);
        agent.speed = 0f;
        if (timerThing > 5f)
        {
            timerThing = 0f;
            if (collider.gameObject.name == "AttackTarget")
            {
                
            }
            Debug.Log(collider.name);
        }
        GameObject thingy = collider.gameObject;
        Tower temp;
        if (collider.gameObject.TryGetComponent<Tower>(out temp))
        {
            transform.LookAt(collider.transform.position);
            agent.speed = 0f;
            EnemyAttack();
        }
        else if (collider.gameObject.name == "AttackTarget")
        {
            transform.LookAt(collider.transform.position);
            agent.speed = 0f;
            EnemyAttack();
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
        currentTarget = pTarget;
        if (currentTarget == null)
        {
            Debug.Log("There's no target");
            currentTarget = finalTarget;
        }
    }

    public void EnemyTakesDamage(int _damageAmmount)
    {
        GameObject temp = this.gameObject;

        SwitchEnemyTarget(currentTarget); //takes the turret the damage came from

        enemyCurrentHealth -= _damageAmmount;

        if (enemyCurrentHealth <= 0)
        {
            GameObject.Destroy(temp);
            Debug.Log("Enemy is dead");
        }
    }


    protected virtual void EnemyMove()
    {
        if (target != null)
        {
            var pos = currentTarget.position;
            pos.y = 0;
            agent.SetDestination(pos);
        }
        else
        {
            Debug.Log("There is no target");
            currentTarget = finalTarget;
        }

        //^This will be replaced by farm house and the farm house radius
    }
    protected virtual void EnemyAttack()
    {
        float timeGoing = 0f;
        timeGoing += Time.deltaTime;
        Debug.Log(timeGoing);
        if (timeGoing >= enemyAttackTime)
        {
            timeGoing = 0f;
            //deal damage to tower with enemyDamageDeal variable
            Debug.Log("Attacked");
        }
    }    
}

//2. Enemy attack - time between attacks, type of attack etc.
//3. Switch enemy target - if attacked, change target to it, if there's no target, change it back to farm
