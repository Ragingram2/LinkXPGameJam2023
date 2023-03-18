using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TargetEnemy : MonoBehaviour
{
    private List<GameObject> m_targets = new List<GameObject>();
    void Start()
    {

    }

    void Update()
    {
        transform.LookAt(GetClosestEnemyPos());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Equals("Enemy"))
        {
            m_targets.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (m_targets.Contains(other.gameObject))
        {
            m_targets.Remove(other.gameObject);
        }
    }

    Vector3 GetClosestEnemyPos()
    {
        float maxDistance = Mathf.Infinity;
        GameObject closest = null;
        foreach (var go in m_targets)
        {
            var dist = Vector3.Distance(transform.position, go.transform.position);
            if ( dist < maxDistance)
            {
                maxDistance = dist;
                closest = go;
            }
        }
        var closestPos = closest.transform.position;
        closestPos.y = transform.position.y;
        return closest != null ? closestPos : transform.forward;
    }
}
