using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BuyButton : MonoBehaviour
{
    public int cost;

    [SerializeField] public TMP_Text buttonText;

    private void OnValidate()
    {
        if (buttonText != null)
        {
            buttonText.text = cost.ToString();
        }
    }

    public void OnClicked()
    {
        UI.blob -= cost;
    }
}
