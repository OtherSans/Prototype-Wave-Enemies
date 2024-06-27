using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    private int getHealth;

    [SerializeField] private Image healthBar;

    public void ChangingHealth(GameObject target)
    {
        var health = target.GetComponent<HealthComponent>();
        getHealth = health.healthPoints;
        healthBar.fillAmount = getHealth / 100f;
    }
}
