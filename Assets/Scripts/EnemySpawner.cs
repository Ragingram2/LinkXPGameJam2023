using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawning Settings")]
    [SerializeField] private GameObject m_enemy;
    private float m_waveTime = 0.0f;
    [SerializeField] private float m_restTimeLength = 180.0f;
    private float m_restTime = 0.0f;
    [SerializeField] private float m_spawnRadius = 10;
    private List<Vector3> m_spawnPositions = new List<Vector3>();
    [SerializeField] private List<Wave> m_waves;
    private Wave m_currentWave;

    public static int m_waveCount = 0;
    private bool m_waveRuning = false;
    public static int m_enemyCount = 0;
    public static int m_alliveCount = 0;

    private bool m_spawn = true;
    private bool m_allEnemiesSpawned = false;

    public static string m_displayTime;

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
            m_displayTime = $"{(int)m_restTime / 60}:{m_restTime % 60:00}";

            m_restTime += Time.deltaTime;
            if (m_restTime > m_restTimeLength)
            {
                m_waveRuning = true;
                m_restTime = 0.0f;
                m_waveCount++;
            }
        }
    }

    void InitNextWave()
    {
        var count = m_waveCount;
        if (count >= m_waves.Count)
        {
            count = m_waves.Count - 1;
        }
        m_currentWave = m_waves[count];

        m_spawnPositions.Clear();
        var num = Random.Range(m_currentWave.spawnLocAmountRange.x, m_currentWave.spawnLocAmountRange.y);
        for (int i = 0; i < num; i++)
        {
            var rand = Random.insideUnitCircle;
            Vector3 pos = new Vector3(rand.x, 0, rand.y);
            m_spawnPositions.Add(pos.normalized * m_spawnRadius);
        }
    }

    void UpdateWave()
    {
        if (m_spawn && !m_allEnemiesSpawned)
        {
            var enemySet = m_currentWave.enemySet;
            foreach (var pos in m_spawnPositions)
            {
                if (m_enemyCount > m_currentWave.maxEnemies)
                    break;
                var rand = Random.Range(0, enemySet.m_enemies.Count);
                Enemy enemy = Instantiate(m_enemy, pos, Quaternion.identity).GetComponent<Enemy>();
                enemy.Initialize(enemySet.m_enemies[rand], pos);
                m_enemyCount++;
                m_alliveCount++;
            }

            if (m_enemyCount >= m_currentWave.maxEnemies)
            {
                m_allEnemiesSpawned = true;
                m_enemyCount = 0;
            }
            else
            {
                StartCoroutine(SpawnCycle(m_currentWave.spawnRate));
            }
        }

        m_waveTime += Time.deltaTime;
        if (m_waveTime >= m_currentWave.waveTimeLength || (m_alliveCount <= 0 && m_allEnemiesSpawned))
        {
            m_waveTime = 0;
            EndWave();
        }

        m_displayTime = $" {(int)m_waveTime / 60}:{m_waveTime % 60:00}";
    }

    void EndWave()
    {
        m_waveRuning = false;
        m_allEnemiesSpawned = false;
        m_enemyCount = 0;
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
