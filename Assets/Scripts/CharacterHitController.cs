using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Transactions;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterHitController : MonoBehaviour
{
    [SerializeField] private float invincibilityLength;
    [SerializeField] private SkinnedMeshRenderer[] playerMesh = new SkinnedMeshRenderer[] { };
    [SerializeField] private float knockForce;

    private float invincibilityCounter;
    private float flashCounter;
    private float flashLength = 0.1f;

    private CharacterController rb;

    [NonSerialized] public bool isInvincible = false;

    private void Awake()
    {
        rb = GetComponent<CharacterController>();
        
    }
    private void Update()
    {
        
        if (invincibilityCounter > 0)
        {
            invincibilityCounter -= Time.deltaTime;

            flashCounter -= Time.deltaTime;
            if (flashCounter <= 0)
            {
                foreach (var player in playerMesh)
                {
                    player.enabled = !player.enabled;
                    flashCounter = flashLength;
                }
            }
            if (invincibilityCounter <= 0)
            {
                foreach (var player in playerMesh)
                {
                    player.enabled = true;
                    isInvincible = false;
                }
            }
        }
    }
    public void GettingHit()
    {
        if (invincibilityCounter <= 0)
        {
            isInvincible = true;
            invincibilityCounter = invincibilityLength;
            foreach (var player in playerMesh)
            {
                player.enabled = false;
                flashCounter = flashLength;
            }
        }
    }
    public void Knockback()
    {
        transform.position += transform.forward * Time.deltaTime * knockForce;
    }
}
