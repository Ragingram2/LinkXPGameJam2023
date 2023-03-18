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
    [HideInInspector] public Vector3 m_direction;
    public EntityAnimationController animationController;
    private Vector2 m_movement;
    private Builder m_builder;

    private void Start()
    {
        m_builder = GetComponent<Builder>();
    }

    void FixedUpdate()
    {
        transform.position += m_speed * Time.deltaTime * new Vector3(m_movement.x, 0, m_movement.y);
    }

    void OnFire(InputAction val)
    {
        if (val.IsPressed())
        {
            m_builder.BuildOnGrid(transform.position, m_direction);
        }
    }

    void OnMove(InputAction val)
    {
        m_movement = val.ReadValue<Vector2>();
        if (m_movement.magnitude > 0)
        {
            m_direction = new Vector3(m_movement.x, 0, m_movement.y);
        }
        animationController.SetVelocity(m_movement);
    }
}

