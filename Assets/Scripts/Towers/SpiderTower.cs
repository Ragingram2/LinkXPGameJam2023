using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderTower : Tower
{
    float launchSpeed;
    void OnValidate()
    {
        float x = m_data.m_range + 0.25001f;
        float y = -transform.position.y;
        launchSpeed = Mathf.Sqrt(9.81f * (y + Mathf.Sqrt(x * x + y * y)));
    }


    public override void TowerAttack()
    {
        var targetPos = m_curretTarget.transform.position;
        transform.LookAt(m_curretTarget.transform);
        Debug.DrawLine(transform.position, targetPos);

        Vector2 dir;
        dir.x = targetPos.x - targetPos.x;
        dir.y = targetPos.z - targetPos.z;
        float x = dir.magnitude;
        float y = -m_curretTarget.transform.position.y;
        dir /= x;

        float g = 9.81f;
        float s = 10;
        float s2 = s * s;

        float r = s2 * s2 - g * (g * x * x + 2f * y * s2);
        float tanTheta = (s2 + Mathf.Sqrt(r)) / (g * x);
        float cosTheta = Mathf.Cos(Mathf.Atan(tanTheta));
        float sinTheta = cosTheta * tanTheta;

        var go = Instantiate(m_bullet,m_gunTip.transform.position,Quaternion.identity);
        go.GetComponent<Rigidbody>().AddForce(new Vector3(s * cosTheta * dir.x, s * sinTheta, s * cosTheta * dir.y));

    }
}
