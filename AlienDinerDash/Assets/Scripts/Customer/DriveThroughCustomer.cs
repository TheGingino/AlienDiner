using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriveThroughCustomer : MonoBehaviour
{
    [Header("Customer Settings")] [SerializeField]
    private CustomerSO customerSO;
    public CustomerSO CustomerSO => customerSO;


    [Header("Waypoint to leave")]
    private WaypointToLeave _waypointToLeave;
    private int _nextWaypointIndex;

    [SerializeField] OrderingFood _orderingFood;

    [SerializeField] private bool hasBeenServed;

    [SerializeField] private AudioSource flyingSFX;

    private DishType _desiredDish;

    private void Awake()
    {
        _waypointToLeave = FindObjectOfType<WaypointToLeave>();
    }

    private void Start()
    {
        _orderingFood.OrderFood();
    }

   
    public void SetDesiredDish(DishType dish)
    {
        _desiredDish = dish;
    }

  
    public bool IsWaitingFor(DishType dish)
    {
        return !hasBeenServed && _desiredDish == dish;
    }

    
    public void ServeFood()
    {
        if (hasBeenServed)
            return;

        hasBeenServed = true;

        flyingSFX.Play();

        Debug.Log("Drive-through order served");
        
        DriveGetMoney();
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
    
    private void DriveGetMoney()
    {
        int moneyValue = customerSO.customerMoney;
        Debug.Log(moneyValue + " money value from customerSO.");
        RewardSystem rewardSystem = FindObjectOfType<RewardSystem>();
        
        if (rewardSystem == null)
        {
            Debug.LogError("RewardSystem not found in scene!");
            return;
        }
        
        rewardSystem.AddMoney(moneyValue);
        rewardSystem.IncrementCustomerServed();
    }
}