using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Runtime.CompilerServices;

public class UI : MonoBehaviour
{

    public int blob = 0;
    public int wave = 0;
    [SerializeField] public TMP_Text waveText;

    [SerializeField] public TMP_Text blobText;
    bool waveChanged = false;

    private void Start()
    {

    }
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.P))
        {
            Debug.Log("KeyPressed");
            wave++;
            waveText.text = "Wave:" + wave;
            //waveChanged = true;
            addCurrency();
        }

        if (Input.GetKeyUp(KeyCode.O) && wave > 0)
        {
            wave--;
            waveText.text = "Wave:" + wave;
            addCurrency();
        }
        //addCurrency();

    }

    void addCurrency()
    {
        if (/*waveChanged == true &&*/ wave < 6)
        {
            Debug.Log("Add 12");
            blob += 12;
            blobText.text = " " + blob;
            //waveChanged = false;
        }

        if (/*waveChanged == true && */wave >= 6)
        {
            Debug.Log("What ever");
            blob += (wave * 10) / 4;
            blobText.text = " " + blob;
            //waveChanged = false;
        }

    }
}
