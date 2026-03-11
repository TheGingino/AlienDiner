using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriveThroughCustomer : MonoBehaviour
{
    private CustomerSO customerSO;

    [Header("Waypoint to leave")]
    private WaypointToLeave _waypointToLeave;
    private int _nextWaypointIndex;

    [SerializeField] OrderingFood _orderingFood;

    [SerializeField] private bool hasBeenServed;

    private DishType _desiredDish; // NEW


    private void Start()
    {
        _orderingFood.OrderFood();
    }

    // NEW
    public void SetDesiredDish(DishType dish)
    {
        _desiredDish = dish;
    }

    // NEW
    public bool IsWaitingFor(DishType dish)
    {
        return !hasBeenServed && _desiredDish == dish;
    }

    // NEW
    public void ServeFood()
    {
        if (hasBeenServed)
            return;

        hasBeenServed = true;

        Debug.Log("Drive-through order served");

        StartCoroutine(LeaveDriveThrough());
    }


    private IEnumerator LeaveDriveThrough()
    {
        while (_nextWaypointIndex < _waypointToLeave.driveThroughWaypointToLeave.Length)
        {
            Vector3 targetPosition =
                _waypointToLeave.driveThroughWaypointToLeave[_nextWaypointIndex].transform.position;

            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                Time.deltaTime * 5f
            );

            if (Vector3.Distance(
                    transform.position,
                    _waypointToLeave.driveThroughWaypointToLeave[_nextWaypointIndex].transform.position 
                ) <= 0.1f)
            {
                _nextWaypointIndex += 1;
            }

            yield return null;
        }

        Destroy(gameObject);
    }
}