using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Money : MonoBehaviour
{
    void Update()
    {
        GetComponent<TMP_Text>().text = "" + PlayerController.money;
    }
}
