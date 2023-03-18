using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float m_speed = 10;

    private Vector2 m_movement;

    void Start()
    {

    }

    void FixedUpdate()
    {
        transform.position += new Vector3(m_movement.x, 0, m_movement.y) * m_speed * 10f * Time.deltaTime * Time.deltaTime;
    }

    void OnMove(InputValue val)
    {
        m_movement = val.Get<Vector2>().normalized;
    }
}
