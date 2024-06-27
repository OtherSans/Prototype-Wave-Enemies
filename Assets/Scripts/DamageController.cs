using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageController : MonoBehaviour
{
    public int healthDamage;

    [SerializeField] private float destroyTime;

    public void TakeDamage(GameObject target)
    {
        var healthComp = target.GetComponent<HealthComponent>();
        var hitComp = target.GetComponent<CharacterHitController>();
        if (healthComp != null)
        {
            if(!hitComp.isInvincible)
            {
                healthComp.ApplyDamage(healthDamage);
            }
                
        }
    }
    public void DestroyObj()
    {
        Destroy(gameObject, destroyTime);
    }
}
