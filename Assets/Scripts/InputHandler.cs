using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public delegate void InputFwd(InputValue val);

public class InputHandler : MonoBehaviour
{
    [SerializeField] private PlayerInput m_input;

    void Start()
    {
        m_input.onActionTriggered += OnActionTriggered;
    }

    void OnActionTriggered(InputAction.CallbackContext context)
    {
        BroadcastMessage($"On{context.action.name}", context.action, SendMessageOptions.DontRequireReceiver);
    }
}
