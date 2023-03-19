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
    public AudioSource audio_walking;

    public int money;

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
            m_direction = (m_hit.point - transform.position).normalized * 0.8f;
        }

        if (m_movement.magnitude > 0)
        {
            if (!audio_walking.isPlaying)
            {
                audio_walking.Play();
                Debug.Log("Moving");
            }
        }
        else if (m_movement.magnitude == 0 && audio_walking.isPlaying)
        {
            audio_walking.Stop();
            Debug.Log("Stop Moving");
        }
    }

    void OnFire(InputAction val)
    {
        if (val.IsPressed())
        {
            m_builder.BuildOnGrid(PlacementGrid.instance.GetCenter(transform.position) + m_direction * PlacementGrid.instance.itemWidth);
        }
    }

    void OnMove(InputAction val)
    {
        m_movement = val.ReadValue<Vector2>();
        animationController.SetVelocity(m_movement);

    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Turd")
        {
            Destroy(collision.gameObject);
            money++;
        }
    }
}

