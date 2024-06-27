using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Events;

public class EventParticleCollision : MonoBehaviour
{
    public List<ParticleCollisionEvent> collisionEvents;
    public ParticleSystem part;

    [SerializeField] private EnterEvent action;
    [SerializeField] private new string tag;
    private void Start()
    {
        part = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
    }
private void OnParticleCollision(GameObject other)
    {
        int num = part.GetCollisionEvents(other, collisionEvents);
        Rigidbody rb = other.GetComponent<Rigidbody>();
        int i = 0;

        while (i < num)
        {
            if (rb)
            {
                Vector3 pos = collisionEvents[i].intersection;
                Vector3 force = collisionEvents[i].velocity * 10;
                rb.AddForce(force);
            }
            i++;
        }
        //if (other.gameObject.CompareTag(tag))
        //{
        //    action.Invoke(other.gameObject);
        //}
    }

}
[Serializable]
public class EventEnter : UnityEvent<GameObject>
{

}
