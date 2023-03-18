using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float m_speed = 10;
    private Vector2 m_movement;

    [Header("Build settings")]
    [SerializeField] private GameObject m_towerPrefab;

    void Start()
    {

    }

    void FixedUpdate()
    {
        transform.position += new Vector3(m_movement.x, 0, m_movement.y) * m_speed * 10f * Time.deltaTime * Time.deltaTime;
        Debug.DrawRay(transform.position, m_movement.normalized);
    }

    void OnFire(InputValue val)
    {
        if (val.isPressed)
        {
            Instantiate(m_towerPrefab, transform.forward + transform.position, Quaternion.identity);
        }
    }

    void OnMove(InputValue val)
    {
        m_movement = Vector2.zero;
        var input = val.Get<Vector2>();
        if (input.magnitude > 0)
        {
            var v = (Quaternion.Euler(0, 45, 0) * new Vector3(input.x, 0, input.y));
            m_movement = new Vector2(v.x, v.z);
        }
    }
}

