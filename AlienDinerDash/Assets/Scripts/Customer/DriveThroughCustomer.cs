using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriveThroughCustomer : MonoBehaviour
{
    private CustomerSO customerSO;
    
    [Header("Waypoint to leave")] private WaypointToLeave _waypointToLeave;
    private int _nextWaypointIndex;

    [SerializeField] private bool hasBeenServed;
    private IEnumerator CustomerState()
    {
        float waitTime = customerSO.customerFoodTimer;
        switch (customerSO.customerType)
        {
            case CustomerType.DRIVETHROUGH:
                yield return StartCoroutine(DriveThrough(waitTime));
                Debug.Log("Drive-through customer finished waiting." + waitTime);
                break;
        }
    }

    private IEnumerator DriveThrough(float waitTime)
    {
        yield return StartCoroutine(CustomerBehavior(waitTime));
    }

    private IEnumerator CustomerBehavior(float waitTime)
    {
        if (!hasBeenServed)
        {
            while (waitTime > 0)
            {
                waitTime -= Time.deltaTime;
                yield return null;
            }
            hasBeenServed = true;
            Debug.Log("Customer has been served.");
            StartCoroutine(LeaveDriveThrough());
        }
    }

    private IEnumerator LeaveDriveThrough()
    {
        while (_nextWaypointIndex < _waypointToLeave.driveThroughWaypointToLeave.Length)
        {
            Vector3 targetPosition = _waypointToLeave.driveThroughWaypointToLeave[_nextWaypointIndex].transform.position;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * 5f);
            if (Vector3.Distance(transform.position,
                    _waypointToLeave.waypointToLeave[_nextWaypointIndex].transform.position) <= 0.1f)
            {
                _nextWaypointIndex += 1;
            }
            yield return null;
        }
        Destroy(gameObject);
    }
}