using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BuyButton : MonoBehaviour
{
    public int cost;
    public UI m_ui;

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
        m_ui.blob -= cost;
    }
}
