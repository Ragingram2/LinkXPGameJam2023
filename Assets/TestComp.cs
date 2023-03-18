using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct state
{
    public bool up;
    public bool down;
    public bool left;
    public bool right;

    public bool running;
}

public class TestComp : MonoBehaviour
{
    public Animator animator;

    private state currentState = new state();

    public bool up;
    public bool down = true;
    public bool left;
    public bool right;

    public bool running;

    private bool vertical = true;
    private float speed = 0f;

    TestComp()
    {
        currentState.down = true;
    }

    private void OnValidate()
    {
        if(currentState.running == running) {
            if (currentState.up && (down || left || right))
            {
                up = false;
            }

            if (currentState.down && (up || left || right))
            {
                down = false;
            }
        
            if(currentState.left && (up || down || right))
            {
                left = false;
            }
        
            if(currentState.right && (up || down || left))
            {
                right = false;
            }
        }

        if(!up && !down && !left && !right)
        {
            up = currentState.up;
            down = currentState.down;
            left = currentState.left;
            right = currentState.right;
        }

        currentState.up = up;
        currentState.down = down;
        currentState.left = left;
        currentState.right = right;
        currentState.running = running;

        vertical = up || down;
        speed = running? ((up || right) ? 1f : -1f) : 0f;
    }

    void Update()
    {
        animator.SetBool("vertical", vertical);
        animator.SetFloat("speed", speed);
    }
}
