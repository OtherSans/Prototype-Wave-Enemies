using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] private UnityEvent onDie;
    [SerializeField] private UnityEvent onDamage;
    public int healthPoints;

    public void ApplyDamage(int damageValue)
    {
        healthPoints -= damageValue;
        onDamage?.Invoke();
        if (healthPoints <= 0)
          {
                    onDie?.Invoke();
          }
    }
    
}
