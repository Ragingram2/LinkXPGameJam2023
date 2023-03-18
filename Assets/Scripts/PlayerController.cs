using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float m_speed = 10;
    [SerializeField] private LayerMask m_hitMask;
    [HideInInspector] public Vector3 m_direction;
    public EntityAnimationController animationController;
    private Vector2 m_movement;
    private Builder m_builder;
    private Camera m_mainCamera;
    private void Start()
    {
        m_builder = GetComponent<Builder>();
        m_mainCamera = Camera.main;
    }

    void FixedUpdate()
    {
        transform.position += m_speed * Time.deltaTime * new Vector3(m_movement.x, 0, m_movement.y);

        Ray ray = m_mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit m_hit;
        if (Physics.Raycast(ray, out m_hit, 100, m_hitMask))
        {
            m_direction = m_builder.GetGridPos(m_hit.point - transform.position).normalized;
        }
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
        animationController.SetVelocity(m_movement);
    }

    //private void OnDrawGizmos()
    //{
    //    if (m_builder == null)
    //        return;

    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireCube(m_builder.GetGridPos(transform.position + (m_direction * .3f)), Vector3.one * .3f);
    //}
}

