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
    public static int m_enemyCount = 0;
    private int m_maxEnemies = 100;
    private bool m_spawn = true;

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
            Debug.Log($"Rest Time: {(int)m_restTime / 60}:{m_restTime % 60:00}");
            m_restTime += Time.deltaTime;
            if (m_restTime > m_restTimeLength)
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

    void UpdateWave()
    {
        if (m_spawn)
        {
            var enemySet = m_enemySets[0];
            foreach (var pos in m_spawnPositions)
            {
                var rand = Random.Range(0, enemySet.m_enemies.Count);
                var go = Instantiate(m_enemy, pos, Quaternion.identity);
                go.GetComponent<Enemy>().initialize(enemySet.m_enemies[rand], pos);
                m_enemyCount++;
            }
            StartCoroutine(SpawnCycle(m_spawnRate));
        }

        m_waveTime += Time.deltaTime;
        if (m_waveTime >= m_waveTimeLength || m_enemyCount <= 0)
        {
            m_waveTime = 0;
            m_spawn = false;
            EndWave();
        }

        Debug.Log($"Wave Time: {(int)m_waveTime / 60}:{m_waveTime % 60:00}");
    }

    void EndWave()
    {
        m_waveRuning = false;
        InitNextWave();
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
        m_spawn = false;
        yield return new WaitForSeconds(seconds);
        m_spawn = true;
    }
}
