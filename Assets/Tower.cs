using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private TowerData m_data;
    [SerializeField] private LayerMask m_targetMask;
    [SerializeField] private GameObject m_bullet;

    [HideInInspector] public List<GameObject> m_targets = new List<GameObject>();
    private GameObject m_curretTarget;

    void Start()
    {

    }

    void Update()
    {
        if (m_curretTarget == null)
            TowerTarget();
        else
            TowerAttack();
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

    public virtual void TowerAttack()
    {
        var vel = m_curretTarget.GetComponent<Rigidbody>().velocity;
        var pos = m_curretTarget.transform.position +vel;
        Debug.DrawLine(transform.position, pos);
        transform.LookAt(new Vector3(pos.x, 0, pos.y));
    }

    public virtual void TowerTarget()
    {
        var obj = GetClosestEnemyPos();
        m_curretTarget = obj.go;
    }

    (Vector3 pos, GameObject go) GetClosestEnemyPos()
    {
        float maxDistance = Mathf.Infinity;
        GameObject closest = null;
        foreach (var go in m_targets)
        {
            var dist = Vector3.Distance(transform.position, go.transform.position);
            if (dist < maxDistance)
            {
                maxDistance = dist;
                closest = go;
            }
        }
        if (closest == null)
            return (transform.position + transform.forward, null);

        var closestPos = closest.transform.position;
        closestPos.y = transform.position.y;
        return closest != null ? (closestPos, closest) : (transform.position + transform.forward, null);
    }
}
