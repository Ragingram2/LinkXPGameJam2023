using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.Experimental.GraphView.GraphView;
using UnityEngine.UI;

public class TargetEnemy : MonoBehaviour
{
    [SerializeField] private LayerMask m_targetMask;
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
        if ((m_targetMask & (1 << other.gameObject.layer)) != 0)
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
        if(closest == null)
            return transform.position + transform.forward;

        var closestPos = closest.transform.position;
        closestPos.y = transform.position.y;
        return closest != null ? closestPos : transform.position + transform.forward;
    }
}
