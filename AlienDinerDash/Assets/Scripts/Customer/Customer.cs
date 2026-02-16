using System;
using System.Collections;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class Customer : MonoBehaviour
{
    [Header("Customer Settings")] [SerializeField]
    private CustomerSO customerSO;

    private enum CustomerStates
    {
        HUNGRY,
        SERVED,
        LEAVING
    };

    private CustomerStates currentState = CustomerStates.HUNGRY;

    private bool _hasCoroutineStarted = false;
    [SerializeField] private bool hasBeenServed = false;
    //[SerializeField] private int itemsOrdered = 0;

    [Header("Waypoint to leave")] private WaypointToLeave _waypointToLeave;
    private int nextWaypointIndex;

    [SerializeField] private float speed = 5f;
    [SerializeField] private float reachDistance = 0.1f;

    [Header("Visual Testing")] [SerializeField]
    private Slider customerTimerSlider;

    private float _sliderTime;


    private void Start()
    {
        _waypointToLeave = FindObjectOfType<WaypointToLeave>();
        customerTimerSlider = FindObjectOfType<Slider>();

        customerTimerSlider.value = customerSO.customerFoodTimer;
    }

    private void Update()
    {
        if (currentState == CustomerStates.HUNGRY && !_hasCoroutineStarted)
        {
            StartCoroutine(CustomerState());
            _hasCoroutineStarted = true;
        }
    }

    private IEnumerator CustomerState()
    {
        float waitTime = customerSO.customerTimer;
        Debug.Log(waitTime);
        switch (customerSO.customerType)
        {
            case CustomerType.ANNOYING:
                yield return StartCoroutine(AnnoyingCustomer(waitTime));
                Debug.Log("Annoying customer finished waiting." + waitTime);
                break;
            case CustomerType.AVERAGE:
                yield return StartCoroutine(NormalCustomer(waitTime));
                Debug.Log("Average customer finished waiting." + waitTime);
                break;
            case CustomerType.PATIENT:
                yield return StartCoroutine(PatientCustomer(waitTime));
                Debug.Log("Patient customer finished waiting." + waitTime);
                break;
        }
    }

    
    private IEnumerator AnnoyingCustomer(float waitTime)
    {
        yield return StartCoroutine(CustomerBehavior(waitTime));
    }
    private IEnumerator PatientCustomer(float waitTime)
    {
        yield return StartCoroutine(CustomerBehavior(waitTime));
    }
    private IEnumerator NormalCustomer(float waitTime)
    {
        yield return StartCoroutine(CustomerBehavior(waitTime));
    }
    
    private IEnumerator CustomerBehavior(float waitTime)
    {
        while (waitTime > 0)
        {
            DecreaseSliderValue();
            if (hasBeenServed)
            {
                currentState = CustomerStates.SERVED;
                Debug.Log("Customer received food!");
                break;
            }
            waitTime -= Time.deltaTime;
            yield return null;
        }

        CustomerType customerType = CustomerType.ANNOYING;
        
        if (!hasBeenServed && customerTimerSlider.value <= 0 && customerType == CustomerType.ANNOYING)
        {
            Debug.Log("Customer got tired of waiting and left!");
            currentState = CustomerStates.LEAVING;
            LeaveRestaurant();
        }

    }

    public void ServeFood()
    {
        if (currentState == CustomerStates.HUNGRY)
        {
            hasBeenServed = true;
            currentState = CustomerStates.SERVED;
            Debug.Log("Customer received food!");
        }
    }

    [ContextMenu("Testing the ability to leave the restaurant")]
    private void LeaveRestaurant()
    {
        Debug.Log("Customer is leaving");
        StartCoroutine(MoveToExit());
    }

    private IEnumerator MoveToExit()
    {
        while (nextWaypointIndex < _waypointToLeave.waypointToLeave.Length)
        {
            transform.position = Vector3.MoveTowards(transform.position,
                _waypointToLeave.waypointToLeave[nextWaypointIndex].transform.position,
                Time.deltaTime * speed);

            if (Vector3.Distance(transform.position,
                    _waypointToLeave.waypointToLeave[nextWaypointIndex].transform.position) <= reachDistance)
            {
                nextWaypointIndex += 1;
            }

            yield return null;
        }
        Destroy(gameObject);
    }

    private void DecreaseSliderValue()
    {
        if (customerTimerSlider.value >= 0) 
        {
            customerTimerSlider.value -= Time.deltaTime;
        }
        //Debug.Log("Customer timer: " + customerTimerSlider.value);
    }
}