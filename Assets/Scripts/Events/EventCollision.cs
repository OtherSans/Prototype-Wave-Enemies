using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventCollision : MonoBehaviour
{
    
    [SerializeField] private EnterEvent action;
    [SerializeField] private new string tag;
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag(tag))
        {
            action.Invoke(collision.gameObject);
        }
    }

}
[Serializable]
public class EnterEvent : UnityEvent<GameObject>
{

}

