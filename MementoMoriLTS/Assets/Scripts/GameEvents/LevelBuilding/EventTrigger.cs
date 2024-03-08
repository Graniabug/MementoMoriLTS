using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class EventTrigger : MonoBehaviour
{
    private Collider triggerCollider;
    public UnityEvent OnTrigger;

    private void Start()
    {
        triggerCollider = this.GetComponent<Collider>();
        triggerCollider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        OnTrigger.Invoke();
    }
}
