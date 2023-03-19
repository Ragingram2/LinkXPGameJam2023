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

        if(waveChanged == true)
        {

        }


    }
}
