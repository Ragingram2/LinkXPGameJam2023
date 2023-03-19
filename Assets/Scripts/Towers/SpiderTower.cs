using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderTower : Tower
{
    float launchSpeed;
    Vector3 launchVel;
    void OnValidate()
    {
        float x = m_data.m_range + 0.25001f;
        float y = -transform.position.y;
        launchSpeed = Mathf.Sqrt((y + Mathf.Sqrt(x * x + y * y)));
    }

    public override void TowerAttack()
    {
        var targetPos = m_curretTarget.transform.position;
        transform.LookAt(m_curretTarget.transform);
        Debug.DrawLine(transform.position, targetPos);

        RaycastHit hit;
        if (Physics.Raycast(m_gunTip.position, transform.forward, out hit, Mathf.Infinity, m_targetMask))
        {
            var name = LayerMask.LayerToName(hit.transform.gameObject.layer);
            if (!name.Equals("Target"))
            {
                return;
            }
        }

        Vector2 dir;
        dir.x = targetPos.x - m_gunTip.position.x;
        dir.y = targetPos.z - m_gunTip.position.z;
        float x = dir.magnitude;
        float y = -m_gunTip.position.y;
        dir /= x;

        float g = 9.81f;
        float s = 10;
        float s2 = s * s;

        float r = s2 * s2 - g * (g * x * x + 2f * y * s2);
        float tanTheta = (s2 + Mathf.Sqrt(r)) / (g * x);
        float cosTheta = Mathf.Cos(Mathf.Atan(tanTheta));
        float sinTheta = cosTheta * tanTheta;

        launchVel = new Vector3(s * cosTheta * dir.x, s * sinTheta, s * cosTheta * dir.y) * launchSpeed * 1000f;
        Debug.Log(launchVel);
        if (m_canAttack)
        {
            var go = Instantiate(m_bullet, m_gunTip.transform.position + transform.forward, Quaternion.identity);
            go.GetComponent<Rigidbody>().AddForce(launchVel, ForceMode.Impulse);
            StartCoroutine(ShotCooldown(m_data.m_fireRate));
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, launchVel);
    }
}
