using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackType
{
    SlowDown,
    Poison
}
public class MortarShot : MonoBehaviour
{
    public GameObject m_explosion;
    public AttackType m_attackType;
    public LayerMask m_hitmask;
    public float m_blastRadius = 5f;
    public float m_slowDownTime = 2f;
    public int m_damage = 10;

    private void OnCollisionEnter(Collision collision)
    {
        var layername = LayerMask.LayerToName(collision.collider.gameObject.layer);
        if (layername.Equals("Ground") || layername.Equals("Target"))
        {
            AOEAttack();
            var go = Instantiate(m_explosion, transform.position, Quaternion.identity);
            go.transform.localScale = Vector3.one * (m_blastRadius + m_blastRadius);
            Destroy(gameObject);
        }
    }

    void AOEAttack()
    {
        var colls = Physics.OverlapSphere(transform.position, m_blastRadius, m_hitmask);
        foreach (var coll in colls)
        {
            Enemy enemy;
            if (coll.gameObject.TryGetComponent<Enemy>(out enemy))
            {
                switch (m_attackType)
                {
                    case AttackType.Poison:
                        enemy.EnemyTakeDamage(m_damage, gameObject);
                        break;
                    case AttackType.SlowDown:
                        StartCoroutine(enemy.SlowdownEffect(m_slowDownTime));
                        break;
                }
            }
        }
    }
}
