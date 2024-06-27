using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnController : MonoBehaviour
{
    [SerializeField] private GameObject[] enemy = new GameObject[] { };
    [SerializeField] private Transform spawner;

    public void Spawn()
    {
        
        for(int i = 0; i < enemy.Length; i++)
        {
            var pos = new Vector3(Random.Range(-11f, 11f), spawner.position.y, spawner.position.z);
            var enemies = Instantiate(enemy[i], pos, Quaternion.identity);
        }
        
    }

}
