using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{

    [SerializeField] int damage = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        IDamageable d = other.gameObject.GetComponent<IDamageable>();
        if (d != null)
        {
            d.DealDamage(damage);
        }
    }

}
