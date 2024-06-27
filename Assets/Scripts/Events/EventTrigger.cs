using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class EventTrigger : MonoBehaviour
{
    [SerializeField] private UnityEvent action;

    [SerializeField] private new string tag;

    private void OnTriggerEnter(Collider other)
    {
            if (other.gameObject.CompareTag(tag))
            {
                action?.Invoke();
            }
            else if (other.gameObject.tag == "Enemy")
            {
                Destroy(other.gameObject);
            }
    }
}
