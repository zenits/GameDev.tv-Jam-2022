using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Exit : MonoBehaviour
{
    public UnityEvent OnExit;

    bool hasExited = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && !hasExited)
        {
            hasExited = true;
            OnExit.Invoke();
        }
    }

}
