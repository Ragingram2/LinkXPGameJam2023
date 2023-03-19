using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridAlign : MonoBehaviour
{
    public bool dummy;

    private void OnValidate()
    {
        if (PlacementGrid.instance == null) return;
        transform.position = PlacementGrid.instance.GetCenter(transform.parent.position);
    }
}
