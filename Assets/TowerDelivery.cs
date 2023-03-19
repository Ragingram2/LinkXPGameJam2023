using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerDelivery : MonoBehaviour
{
    public int cost = 0;
    public GameObject m_tower;
    public Builder m_player;

    public void OnClick()
    {
        m_player.m_towerPrefab = m_tower;
    }
    
}
