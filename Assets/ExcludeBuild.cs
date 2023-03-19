using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ExcludeBuild : MonoBehaviour
{
    public Bounds bounds = new Bounds(Vector3.zero, Vector3.one);

    public static List<ExcludeBuild> exclusionZones = new List<ExcludeBuild>();

    private void OnEnable()
    {
        exclusionZones.Add(this);
    }

    private void OnDisable()
    {
        exclusionZones.Remove(this);
    }

    private void OnValidate()
    {
        if(PlacementGrid.instance != null)
            PlacementGrid.instance.RecalculateGrid();
    }

    private void OnDrawGizmos()
    {
        Matrix4x4 prevMat = Gizmos.matrix;
        Gizmos.color = new Color(1f, 0f, 0f, 0.25f);
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawCube(bounds.center, bounds.size);
        Gizmos.matrix = prevMat;
    }
}
