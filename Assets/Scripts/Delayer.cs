using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Delayer : MonoBehaviour
{
    private float elapsedTime = 0f;

    [SerializeField] float delay = .1f;
    [SerializeField] bool disableOnTrigger = true;
    [SerializeField] UnityEngine.Events.UnityEvent action;

    private void OnEnable()
    {
        Reset();
    }

    private void OnDisable()
    {
        Reset();
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= delay)
        {
            action.Invoke();
            gameObject.SetActive(!disableOnTrigger);
        }
    }

    public void Reset()
    {
        elapsedTime = 0f;
    }

}