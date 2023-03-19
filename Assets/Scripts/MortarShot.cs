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

    private void OnCollisionEnter(Collision collision)
    {
        var layername = LayerMask.LayerToName(collision.collider.gameObject.layer);
        if (layername.Equals("Ground") || layername.Equals("Target"))
        {
            AOEAttack();
            Instantiate(m_explosion, transform.position, Quaternion.identity);
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
                        //do poison stuff
                        break;
                    case AttackType.SlowDown:
                        //do slow stuff
                        break;
                }
            }
        }
    }
}
