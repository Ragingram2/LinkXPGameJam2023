using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawning Settings")]
    [SerializeField] private GameObject m_enemy;
    [SerializeField] private List<EnemySet> m_enemySets;
    [SerializeField] private float m_spawnRate = 1.0f;
    [SerializeField] private float m_waveTimeLength = 180.0f;
     private float m_waveTime = 0.0f;
    [SerializeField] private float m_restTimeLength = 180.0f;
    private float m_restTime = 0.0f;
    [SerializeField] private Vector2 m_spawnLocAmountRange = new Vector2(5, 10);
    [SerializeField] private float m_spawnRadius = 10;
    private List<Vector3> m_spawnPositions = new List<Vector3>();

    private bool m_waveRuning = false;
    private int m_enemyCount = 0;
    private bool m_spawn = false;

    private void Start()
    {
        InitNextWave();
    }

    private void Update()
    {
        if (m_waveRuning)
        {
            UpdateWave();
        }
        else
        {
            m_restTime += Time.deltaTime;
            if(m_restTime > m_restTimeLength)
            {
                m_waveRuning = true;
                m_restTime = 0.0f;
            }
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
        if (val.IsPressed() && !m_waveRuning)
        {
            m_waveRuning = true;
        }
    }

    void UpdateWave()
    {
        m_waveTime += Time.deltaTime;
        if(m_waveTime >= m_waveTimeLength)
        {
            m_waveTime = 0;
            m_spawn = false;
            EndWave();
        }

        if (m_spawn)
        {
            var enemySet = m_enemySets[0];
            foreach (var pos in m_spawnPositions)
            {
                var rand = Random.Range(0, enemySet.m_enemies.Count);
                var go = Instantiate(m_enemy, pos, Quaternion.identity);
                go.GetComponent<Enemy>().initialize(enemySet.m_enemies[rand]);
                m_enemyCount++;
            }
            StartCoroutine(SpawnCycle(m_spawnRate));
        }
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
    IEnumerator SpawnCycle(float seconds)
    {
        m_spawn= false;
        yield return new WaitForSeconds(seconds);
        m_spawn = true;
    }
}
