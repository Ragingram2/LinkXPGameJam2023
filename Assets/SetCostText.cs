using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SetCostText : MonoBehaviour
{
    public TowerData data;

    void Update()
    {
        GetComponent<TMP_Text>().text = "" + data.cost;
    }
}
