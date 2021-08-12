using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GoalEvent : MonoBehaviour
{
    public UnityEvent OnPlayerGoal;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ShootFireworks();
        }
    }

    private void ShootFireworks()
    {
        OnPlayerGoal.Invoke();
    }
}
