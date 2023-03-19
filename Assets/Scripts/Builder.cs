using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class Builder : MonoBehaviour
{
    [SerializeField] private bool m_debugDraw = false;
    [Header("Build settings")]
    [SerializeField] private GameObject m_towerPrefab;
    [SerializeField] private GameObject m_highlighter;
    [SerializeField] private float m_cellsize = 10;
    [HideInInspector] private PlayerController m_player;

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
        if (PlacementGrid.instance.GetObject(pos) == null)
        {
            var go = Instantiate(m_towerPrefab, g_pos, Quaternion.identity);
            PlacementGrid.instance.FillItem(g_pos, go);
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
        return PlacementGrid.instance.GetObject(key);
    }

    public void RemoveTower(Vector2 key)
    {
        PlacementGrid.instance.FillItem(key, null);
    }

    public Vector3 GetGridPos(Vector3 pos)
    {
        return PlacementGrid.instance.GetCenter(pos);
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

            //Gizmos.color = Color.red;
            //foreach (var pos in m_activeTowers.Keys)
            //{
            //    Handles.Label(pos, $"{pos}");
            //    Gizmos.DrawWireCube(pos, Vector3.one * m_cellsize);
            //}

            Gizmos.color = Color.green;
            for (int x = -5; x < 5; x++)
            {
                for (int y = -5; y < 5; y++)
                {
                    var pos = GetGridPos(transform.position + (new Vector3(x, 0, y) * m_cellsize));
                    Handles.Label(pos, $"{pos}");
                    Gizmos.DrawWireCube(pos, Vector3.one * m_cellsize);
                }
            }
        }
    }
}
