using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour, IDamageable
{

    public int health;
    [SerializeField] UnityEvent onDie;
    [SerializeField] OnDamageEvent onDamage;
    [SerializeField] OnHealthChangeEvent onHealthChange;


    private void Start()
    {
        if (onHealthChange != null)
            onHealthChange.Invoke(health);
    }

    public void DealDamage(int damage)
    {
        health--;
        //health = Math.Max(health, 0);
        onDamage.Invoke(damage);
        onHealthChange.Invoke(health);
        if (health == 0)
        {
            onDie.Invoke();
        }
    }

    public void Heal(int healing)
    {
        health += healing;
        onHealthChange.Invoke(health);
    }

    public void SetHealth(int value)
    {
        health = value;
        onHealthChange.Invoke(health);
    }

}

public interface IDamageable
{
    public void DealDamage(int damage);
    public void Heal(int healing);
    public void SetHealth(int value);
}

[System.Serializable]
public class OnDamageEvent : UnityEvent<int>
{ }
[System.Serializable]
public class OnHealthChangeEvent : UnityEvent<int>
{ }