using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitFlashController : MonoBehaviour
{
    private SkinnedMeshRenderer[] enemyMesh = new SkinnedMeshRenderer[] { };
    [SerializeField] private float delayTime;

    private void Start()
    {
        enemyMesh = GetComponentsInChildren<SkinnedMeshRenderer>();
    }
    public void DamageFlash()
    {
        for (int i = 0; i < enemyMesh.Length; i++)
        {
            enemyMesh[i].material.color = Color.black;
            Invoke("FlashStop", delayTime);
        }
    }

    private void FlashStop()
    {
        for(int i = 0; i < enemyMesh.Length; i++)
        {
            enemyMesh[i].material.color = Color.white;
        }
    }
}
