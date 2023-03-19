using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntTower : Tower
{
    public override void TowerAttack()
    {
        var vel = m_curretTarget.GetComponent<Rigidbody>().velocity;
        var pos = m_curretTarget.transform.position + vel;
        var temp = m_curretTarget.transform.position;
        temp.y = 0;
        transform.LookAt(temp);

        RaycastHit hit;
        if (Physics.Raycast(m_gunTip.position, transform.forward, out hit, Mathf.Infinity, m_targetMask))
        {
            var name = LayerMask.LayerToName(hit.transform.gameObject.layer);
            if (!name.Equals("Enemy"))
            {
                return;
            }
        }

        if (m_canAttack)
        {
            var go = Instantiate(m_bullet, m_gunTip.position, Quaternion.identity);
            go.transform.LookAt(hit.transform);
            go.GetComponent<Rigidbody>().AddForce(transform.forward * 5f, ForceMode.Impulse);
            go.GetComponent<Bullet>().owner = gameObject;
            StartCoroutine(ShotCooldown(m_data.m_fireRate));
        }
    }
}
