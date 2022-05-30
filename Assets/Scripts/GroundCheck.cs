using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;




public class GroundCheck : MonoBehaviour
{

    [SerializeField] OnGroundedEvent OnGrounded;
    [SerializeField] LayerMask groundCheckLayers;


    private void Awake()
    {
        if (OnGrounded == null)
            OnGrounded = new OnGroundedEvent();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"On Grouncheck Enter layer={other.gameObject.layer} value={groundCheckLayers.value} {(1 << other.gameObject.layer)}");

        if (((1 << other.gameObject.layer) & groundCheckLayers.value) > 0)
            //EventManager.TriggerEvent("OnGrounded", true);
        OnGrounded.Invoke(true);
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log($"On Grouncheck Exit layer={other.gameObject.layer} value={groundCheckLayers.value} {(1 << other.gameObject.layer)}");
        if (((1 << other.gameObject.layer) & groundCheckLayers.value) > 0)
            //EventManager.TriggerEvent("OnGrounded", false);
        OnGrounded.Invoke(false);
    }



}

[System.Serializable]
public class OnGroundedEvent : UnityEvent<bool>
{
}