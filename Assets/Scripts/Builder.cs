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
    [HideInInspector] private PlayerController m_player;
    public AudioSource audio_building;

    void Start()
    {
        m_player = GetComponent<PlayerController>();
    }

    private void Update()
    {
        m_highlighter.transform.position = GetGridPos(PlacementGrid.instance.GetCenter(m_player.transform.position) + (m_player.m_direction * PlacementGrid.instance.itemWidth));
        m_highlighter.GetComponent<MeshRenderer>().material.SetInt("_Available", PlacementGrid.instance.GetObject(m_highlighter.transform.position) == null ? 1 : 0);
    }

    public GameObject BuildOnGrid(Vector3 pos)
    {
        var g_pos = GetGridPos(pos);
        if (PlacementGrid.instance.GetObject(g_pos) == null)
        {
            var go = Instantiate(m_towerPrefab, g_pos, Quaternion.identity);
            PlacementGrid.instance.FillItem(g_pos, go);
            audio_building.Play();
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
}
