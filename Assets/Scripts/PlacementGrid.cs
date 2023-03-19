using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.VolumeComponent;
using static UnityEngine.UI.Image;

public struct GridItem
{
    public GameObject owner;
    public bool enabled;

    public GridItem(GameObject _owner, bool _enabled) { owner = _owner; enabled = _enabled; }
}

public class PlacementGrid : MonoBehaviour
{
    public Vector2Int size;
    public float itemWidth;
    public bool hexagonal;

    private float yScaleHex;

    public LayerMask groundMask;

    private GridItem[,] grid;
    private Vector2 origin;
    private Vector2 totalSize;

    public bool showGrid;

    public static PlacementGrid instance = null;

    public ExcludeBuild excludeBuild;

    private void Start()
    {
        RecalculateGrid();
    }

    public void RecalculateGrid()
    {
        Debug.Log("Recalculating grid");

        yScaleHex = 0.5f / Mathf.Tan(Mathf.Deg2Rad * 30f);
        grid = new GridItem[size.x, size.y];

        Vector3 position = transform.position;
        totalSize = new Vector2(itemWidth * size.x, size.y * (hexagonal ? itemWidth * yScaleHex : itemWidth));
        origin = new Vector2(position.x, position.z) - totalSize * 0.5f;

        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                Vector3 pos = GetCenter(new Vector2Int(x, y)) + Vector3.up * 50f;
                RaycastHit m_hit;
                grid[x, y] = new GridItem(null, Physics.Raycast(new Ray(pos, Vector3.down), out m_hit, 100, groundMask));
            }
        }

        FillRange(excludeBuild.transform.localToWorldMatrix, excludeBuild.bounds, excludeBuild.gameObject);

        instance = this;
    }

    public Vector2Int GetIndex(Vector3 pos)
    {
        return GetIndex(new Vector2(pos.x, pos.z));
    }

    public Vector2Int GetIndex(Vector2 pos)
    {
        Vector2 relativePos = pos - origin;
        Vector2Int index = new Vector2Int();
        index.y = Mathf.RoundToInt(relativePos.y / itemWidth);

        if (hexagonal)
        {
            index.y = Mathf.RoundToInt(relativePos.y / (itemWidth * yScaleHex));
            relativePos.x -= 0.5f * itemWidth * (index.y % 2);
        }

        index.x = Mathf.RoundToInt(relativePos.x / itemWidth);

        return index;
    }

    public GameObject GetObject(Vector3 pos)
    {
        return GetObject(GetIndex(pos));
    }

    public GameObject GetObject(Vector2 pos)
    {
        return GetObject(GetIndex(pos));
    }

    public GameObject GetObject(Vector2Int index)
    {
        if (index.x >= 0 && index.x < size.x && index.y >= 0 && index.y < size.y)
        {
            if (grid[index.x, index.y].enabled)
            {
                return grid[index.x, index.y].owner;
            }
        }

        return null;
    }

    public Vector3 GetCenter(Vector3 pos)
    {
        return GetCenter(GetIndex(pos));
    }

    public Vector3 GetCenter(Vector2 pos)
    {
        return GetCenter(GetIndex(pos));
    }

    public Vector3 GetCenter(Vector2Int index)
    {
        if (hexagonal)
        {
            return new Vector3(origin.x + index.x * itemWidth + 0.5f * itemWidth * (index.y % 2), transform.position.y, origin.y + index.y * itemWidth * yScaleHex);
        }
        return new Vector3(origin.x + index.x * itemWidth, transform.position.y, origin.y + index.y * itemWidth);
    }

    public void FillItem(Vector3 pos, GameObject gameObject)
    {
        FillItem(GetIndex(pos), gameObject);
    }

    public void FillItem(Vector2 pos, GameObject gameObject)
    {
        FillItem(GetIndex(pos), gameObject);
    }

    public void FillItem(Vector2Int index, GameObject gameObject)
    {
        if (index.x >= 0 && index.x < size.x && index.y >= 0 && index.y < size.y)
        {
            if (grid[index.x, index.y].enabled)
            {
                grid[index.x, index.y].owner = gameObject;
            }
        }
    }

    public void FillRange(Matrix4x4 matrix, Bounds bounds, GameObject gameObject)
    {
        Vector3 matOrigin = matrix.GetPosition();
        Vector2 min = new Vector2(bounds.min.x, bounds.min.z);
        Vector2 max = new Vector2(bounds.max.x, bounds.max.z);

        Vector2 cur = min;

        float stepWidth = itemWidth * 0.5f;

        Vector2Int steps = new Vector2Int(
            Mathf.CeilToInt((max.x - min.x) / stepWidth),
            Mathf.CeilToInt((max.y - min.y) / stepWidth));

        for (int x = 0; x < steps.x; x++)
        {
            for (int y = 0; y < steps.y; y++)
            {
                Vector3 transformedCur = matrix.MultiplyPoint(new Vector3(cur.x, matOrigin.y, cur.y));

                FillItem(new Vector2(transformedCur.x, transformedCur.z), gameObject);
                cur.y += stepWidth;
            }
            cur.x += stepWidth;
            cur.y = min.y;
        }
    }

    private void OnDrawGizmos()
    {
        if (!showGrid) return;

        Vector2Int underMouse = new Vector2Int(-1, -1);

        Ray ray = Camera.current.ScreenPointToRay(Input.mousePosition);
        RaycastHit m_hit;
        if (Physics.Raycast(ray, out m_hit, 100))
        {
            underMouse = GetIndex(m_hit.point);
        }

        float height = transform.position.y;

        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                if (!grid[x, y].enabled)
                    continue;

                Vector2 pos = origin;
                pos.x += x * itemWidth;
                pos.y += y * itemWidth;

                if (hexagonal)
                {
                    pos.x += 0.5f * itemWidth * (y % 2);
                    pos.y = origin.y + y * itemWidth * yScaleHex;
                }

                Gizmos.color = grid[x, y].owner == null ? new Color(0f, 1f, 0f, 0.25f) : new Color(1f, 0f, 0f, 0.25f);

                if(x == underMouse.x && y == underMouse.y)
                {
                    Gizmos.color = Color.blue;
                }

                Gizmos.DrawSphere(new Vector3(pos.x, height, pos.y), itemWidth * 0.5f);
            }
        }
    }
}
