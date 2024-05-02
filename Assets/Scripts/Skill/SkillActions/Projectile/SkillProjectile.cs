using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillProjectile : MonoBehaviour
{
    [SerializeField] private float speed;

    private Vector3 direction;
    private Vector3 destination;

    private float distanceLeft;

    private Action onReachDestinationCallback;

    private void Update()
    {
        NextMovementStep();
    }

    public void AskMoveToDestination(Vector2 movementDestination, Action callback)
    {
        destination = movementDestination;
        destination.z = transform.position.z;

        direction = (destination - transform.position).normalized;

        distanceLeft = Vector3.Distance(destination, transform.position);

        onReachDestinationCallback = callback;

        enabled = true;
    }

    private void OnReachDestination()
    {
        onReachDestinationCallback?.Invoke();

        StopMovement();
    }

    private void StopMovement()
    {
        enabled = false;
        Destroy(gameObject);
    }

    private void NextMovementStep()
    {
        transform.position += direction * Time.deltaTime * speed;

        if(Vector3.Distance(transform.position, destination) > distanceLeft)
        {
            OnReachDestination();
        }

        distanceLeft = Vector3.Distance(transform.position, destination);
    }
}
