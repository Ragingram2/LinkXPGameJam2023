using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Builder : MonoBehaviour
{
    [SerializeField] private bool m_debugDraw = false;
    [Header("Build settings")]
    [SerializeField] private GameObject m_towerPrefab;
    [SerializeField] private GameObject m_highlighter;
    [SerializeField] private float m_cellsize = 10;
    [HideInInspector] private PlayerController m_player;
    private Dictionary<Vector3, GameObject> m_activeTowers = new Dictionary<Vector3, GameObject>();

    void Start()
    {
        m_player = GetComponent<PlayerController>();
    }

    private void Update()
    {
        m_highlighter.transform.localScale = Vector3.one * m_cellsize;
        m_highlighter.transform.position = GetGridPos(transform.position + (m_player.m_direction * m_cellsize));
    }

    public GameObject BuildOnGrid(Vector3 pos)
    {
        var g_pos = GetGridPos(pos);
        if (!m_activeTowers.ContainsKey(g_pos))
        {
            var go = Instantiate(m_towerPrefab, g_pos, Quaternion.identity);
            m_activeTowers.Add(g_pos, go);
            return go;
        }
        return null;
    }

    public GameObject BuildOnGrid(Vector3 pos, Vector3 direction)
    {
        var g_pos = GetGridPos(pos + (direction * m_cellsize));
        if (!m_activeTowers.ContainsKey(g_pos))
        {
            var go = Instantiate(m_towerPrefab, g_pos, Quaternion.identity);
            m_activeTowers.Add(g_pos, go);
            return go;
        }
        return null;
    }

    public GameObject GetTower(Vector3 pos)
    {
        var g_pos = GetGridPos(pos);
        return GetTower(g_pos);
    }

    public GameObject GetTower(Vector2 key)
    {
        if (m_activeTowers.ContainsKey(key))
        {
            return m_activeTowers[key];
        }
        return null;
    }

    public void RemoveTower(Vector2 key)
    {
        if (m_activeTowers.ContainsKey(key))
        {
            m_activeTowers.Remove(key);
        }
    }

    public Vector3 GetGridPos(Vector3 pos)
    {
        float x = Mathf.Round(pos.x / m_cellsize) * m_cellsize;
        float y = Mathf.Round(pos.z / m_cellsize) * m_cellsize;
        return new Vector3(x, 0, y);
    }

    private void OnDrawGizmos()
    {
        if (m_debugDraw)
        {
            Gizmos.color = Color.blue;
            if (m_player != null)
            {
                var pos = GetGridPos(transform.position + (m_player.m_direction * m_cellsize));
                Handles.Label(pos, $"{pos}");
                Gizmos.DrawWireCube(pos, Vector3.one * m_cellsize);
            }

            Gizmos.color = Color.red;
            foreach (var pos in m_activeTowers.Keys)
            {
                Handles.Label(pos, $"{pos}");
                Gizmos.DrawWireCube(pos, Vector3.one * m_cellsize);
            }
        }
    }
}
