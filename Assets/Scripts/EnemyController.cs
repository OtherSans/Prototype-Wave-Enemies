using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float damageForce;

    private Rigidbody rb;
    private GameObject player;
    private bool isPlayerInRange = false;

    private Vector3 newVel = new Vector3();
    private Vector3 newPos = new Vector3();
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player");
    }
    private void Update()
    {
        newPos = player.transform.position - transform.position;
        float angle = Mathf.Atan2(newPos.x, newPos.z) * Mathf.Rad2Deg;
        rb.rotation = Quaternion.Euler(0, angle, 0);
        newPos.Normalize();
        newVel = newPos;
    }
    private void MoveChar(Vector3 direction)
    {
        rb.MovePosition(transform.position + (direction * speed * Time.fixedDeltaTime));
    }
    private void FixedUpdate()
    {
        
        if (isPlayerInRange)
        {
            MoveChar(newVel);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }

    public void DamageHitForce()
    {
        //rb.AddForce(new Vector3(damageForce, rb.transform.position.y, rb.transform.position.z), ForceMode.Impulse);
        rb.velocity = new Vector3(damageForce, rb.transform.position.y, rb.transform.position.z);
    }
}
