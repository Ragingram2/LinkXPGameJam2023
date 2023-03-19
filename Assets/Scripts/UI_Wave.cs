using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MoneyAdd : MonoBehaviour
{
    public int wave = 0;
    [SerializeField] public TMP_Text waveText;
    [SerializeField] public TMP_Text currencyText;
    bool waveChanged = false;

    private void Update()
    {
        wave = EnemySpawner.m_waveCount;
        waveText.text = "Wave:" + wave;

    }
}
