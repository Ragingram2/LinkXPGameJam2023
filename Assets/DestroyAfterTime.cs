using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    public float time = 5;
    IEnumerator WaitForDeath(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }

    private void Start()
    {
        StartCoroutine(WaitForDeath(time));
    }
}
