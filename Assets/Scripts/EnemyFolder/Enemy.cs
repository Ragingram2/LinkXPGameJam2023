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

    bool canAttack;

    

    public Rigidbody body;
    GameObject target;
    Transform currentTarget;
    Transform finalTarget;
    public NavMeshAgent agent;

    public BoxCollider targetBoxCollider; //should be farm or wall collider
    public SphereCollider enemySphereCollider;

    public GameObject turd;
    public int amountOfTurds;

    public EntityAnimationController entityAnimationController;
    
    public void Initialize(EnemyData data, Vector3 pos)
    {
        enemyData = data;

        target = GameObject.Find("Core");
        currentTarget = target.transform;
        finalTarget = target.transform;

        agent = GetComponent<NavMeshAgent>();
        //agent.Warp(pos);
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

        entityAnimationController.GetComponent<Animator>().runtimeAnimatorController = enemyData.controller;

        DebugLog();
    }

    void Update()
    {
        EnemyMove();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name.Equals("Core"))
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        Tower temp;
        if (collision.gameObject.TryGetComponent<Tower>(out temp))
        {
            temp.TakeDamage(enemyDamageDeal);
        }
    }

    void OnTriggerStay(Collider collider)
    {
        Tower temp;
        if (collider.gameObject.TryGetComponent<Tower>(out temp))
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

    public void EnemyTakeDamage(int _damageAmmount, GameObject bulletOwner)
    {
        if (currentTarget.tag != "Tower")
        {
            SwitchEnemyTarget(bulletOwner.transform);
        }
        enemyCurrentHealth -= _damageAmmount;
        Debug.Log(enemyCurrentHealth);

        if (enemyCurrentHealth <= 0)
        {
            bulletOwner.GetComponent<Tower>().m_targets.Remove(gameObject);
            for (int i = 0; i < amountOfTurds; i++)
            {
                Instantiate(turd, transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
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

        Vector3 movement = agent.velocity;
        entityAnimationController.SetVelocity(new Vector2(movement.x, movement.z));
    }

    protected virtual void EnemyAttack()
    {
        Tower temp = currentTarget.gameObject.GetComponent<Tower>();
        
        if (canAttack)
        {
            temp.TakeDamage(enemyDamageDeal);
            StartCoroutine(EnemyAttackCooldown(enemyAttackTime));
        }
    }

    protected IEnumerator EnemyAttackCooldown(float seconds)
    {
        canAttack = false;
        yield return new WaitForSeconds(seconds);
        canAttack = true;
    }

    private void OnDestroy()
    {
        EnemySpawner.m_alliveCount--;
        
        
    }
}

//2. Enemy attack - time between attacks, type of attack etc.
//3. Switch enemy target - if attacked, change target to it, if there's no target, change it back to farm
