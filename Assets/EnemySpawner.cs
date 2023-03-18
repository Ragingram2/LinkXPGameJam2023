using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawning Settings")]
    [SerializeField] private List<EnemySet> m_enemySets;
    [SerializeField] private Vector2 m_spawnLocAmountRange = new Vector2(5, 10);
    [SerializeField] private float m_spawnRadius = 10;
    private List<Vector3> m_spawnPositions = new List<Vector3>();

    private bool m_waveRuning = false;
    private int m_enemyCount = 0;

    private void Start()
    {
        InitNextWave();
    }

    private void Update()
    {
        if(m_waveRuning)
        {
            UpdateWave();
        }
    }

    void InitNextWave()
    {
        m_spawnPositions.Clear();
        var num = Random.Range(m_spawnLocAmountRange.x, m_spawnLocAmountRange.y);
        for (int i = 0; i < num; i++)
        {
            var rand = Random.insideUnitCircle;
            Vector3 pos = new Vector3(rand.x, 0, rand.y);

            m_spawnPositions.Add(pos.normalized * m_spawnRadius);
        }
    }

    void OnStartWave(InputAction val)
    {
        if(val.IsPressed())
        {

        }
    }
    void UpdateWave()
    {

    }

    void EndWave()
    {
        m_waveRuning = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, m_spawnRadius);
        Gizmos.color = Color.red;
        foreach (var pos in m_spawnPositions)
        {
            Gizmos.DrawSphere(pos, 1);
        }
    }
}
