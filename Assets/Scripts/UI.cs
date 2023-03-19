using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Runtime.CompilerServices;

public class MoneyAdd : MonoBehaviour
{
    public int wave = 0;
    public int blob = 0;
    [SerializeField] public TMP_Text waveText;
    [SerializeField] public TMP_Text blobText;
    bool waveChanged = false;

    private void Start()
    {
        
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            wave++;
            waveText.text = "Wave:" + wave;
            waveChanged = true;
        }

        if (Input.GetKeyDown(KeyCode.O) && wave > 0)
        {
            wave--;
            waveText.text = "Wave:" + wave;
        }
        addCurrency();
        
        }
        
        void addCurrency()
        {
        if (waveChanged == true)
        {
            blob += 10;
            blobText.text = "0" + blob;
            waveChanged = false;
        }

    }
}
