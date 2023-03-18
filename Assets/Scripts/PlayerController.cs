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

    public EntityAnimationController animationController;

    private Vector2 m_movement;
    private Vector3 m_direction;

    [Header("Build settings")]
    [SerializeField] private GameObject m_towerPrefab;

    void FixedUpdate()
    {
        transform.position += m_speed * Time.deltaTime * new Vector3(m_movement.x, 0, m_movement.y);
    }

    void OnFire(InputAction val)
    {
        if (val.IsPressed())
        {
            Instantiate(m_towerPrefab, m_direction + transform.position, Quaternion.identity);
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position + m_direction, Vector3.one/2f);
    }
}

