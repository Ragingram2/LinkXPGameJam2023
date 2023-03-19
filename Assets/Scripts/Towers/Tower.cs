using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] protected TowerData m_data;
    [SerializeField] protected LayerMask m_targetMask;
    [SerializeField] protected GameObject m_bullet;
    [SerializeField] protected Transform m_gunTip;

    [HideInInspector] public List<GameObject> m_targets = new List<GameObject>();
    protected GameObject m_curretTarget;
    protected bool m_canAttack = true;

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
            Debug.Log($"Target Entered {other.name}");
            if (!m_targets.Contains(other.gameObject))
                m_targets.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (m_targets.Contains(other.gameObject))
        {
            Debug.Log($"Target Left {other.name}");
            m_targets.Remove(other.gameObject);
        }
    }

    public virtual void TowerAttack()
    {
        Debug.Log("Tower Attacking");
    }

    public virtual void TowerTarget()
    {
        var obj = GetClosestEnemyPos();
        m_curretTarget = obj.go;
    }

    public void TakeDamage(int num,GameObject owner)
    {
        m_data.m_health -= num;
        if (m_data.m_health <= 0)
        {
            owner.GetComponent<Enemy>().SwitchEnemyTarget(owner.GetComponent<Enemy>().finalTarget);
            owner.GetComponent<Enemy>().agent.speed = owner.GetComponent<Enemy>().enemyData.speed;
            owner.GetComponent<Enemy>().canSwitchTarget = true;
            Destroy(gameObject);
        }
    }

    protected (Vector3 pos, GameObject go) GetClosestEnemyPos()
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

    protected IEnumerator ShotCooldown(float seconds)
    {
        m_canAttack = false;
        yield return new WaitForSeconds(seconds);
        m_canAttack = true;
    }
}
