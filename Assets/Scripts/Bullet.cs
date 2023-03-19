using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject owner;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Enemy>(out Enemy enemyComponent))
        {
            enemyComponent.EnemyTakeDamage(1, owner);
        }

        Destroy(gameObject);
    }

    void ParentAliveCheck()
    {
        if (owner == null)
        {
            Destroy(gameObject);
        }
    }

    private void LateUpdate()
    {
        ParentAliveCheck();
    }
}
