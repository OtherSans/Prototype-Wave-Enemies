using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventInteractions : MonoBehaviour
{
    [SerializeField] private UnityEvent action;
    public void Interact()
    {
        action?.Invoke();
    }
}
