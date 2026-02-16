using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Customer : MonoBehaviour
{
    [Header("Customer Settings")]
    [SerializeField] private CustomerSO customerSO;
    private enum CustomerStates {HUNGRY, SERVED, FINISHED};
    private CustomerStates currentState = CustomerStates.HUNGRY;
    

    [Header("Waypoint to leave")]
    private WaypointToLeave _waypointToLeave;
    private int nextWaypointIndex;
    
    [SerializeField] private float speed = 5f;
    [SerializeField] private float reachDistance = 0.1f;
    
    [Header("Visual Testing")]
    [SerializeField] private Slider customerTimerSlider;
    [SerializeField] private Slider sliderTime;
    
    
    
    private void Start()
    {
        _waypointToLeave = FindObjectOfType<WaypointToLeave>();
        customerTimerSlider = FindObjectOfType<Slider>();
        
        customerTimerSlider.maxValue = customerSO.customerFoodTimer;
    }

    private void Update()
    {
        StartCoroutine(CustomerState());
        Debug.Log("Customer is waiting");
    }

    private IEnumerator CustomerState()
    {
        currentState = CustomerStates.HUNGRY;
        Debug.Log("Customer is hungry");
        
        yield return new WaitForSeconds(customerSO.customerFoodTimer);
        
        currentState = CustomerStates.SERVED;
        Debug.Log("Customer is served");
        DecreaseSliderValue();
        
        yield return new WaitForSeconds(customerSO.customerFoodTimer);
        
        currentState = CustomerStates.FINISHED;
        Debug.Log("Customer is finished");
        LeaveRestaurant();
    }
    
    public void ServeFood()
    {
        if (currentState == CustomerStates.HUNGRY)
        {
            currentState = CustomerStates.SERVED;
            Debug.Log("Customer received food!");
        }
    }
    
    [ContextMenu("Testing the ability to leave the restaurant")]
    private void LeaveRestaurant()
    { 
        Debug.Log("Customer is leaving");
        transform.position = Vector3.MoveTowards(transform.position, _waypointToLeave.waypointToLeave[nextWaypointIndex].transform.position, Time.deltaTime * speed);
        if (Vector3.Distance(transform.position, _waypointToLeave.waypointToLeave[nextWaypointIndex].transform.position) <= reachDistance)
        {
            nextWaypointIndex += 1;
        }
        Destroy(gameObject);
    }
    
    private void DecreaseSliderValue()
    {
        customerTimerSlider.value = customerSO.customerFoodTimer;
        if (customerTimerSlider.value > 0) customerTimerSlider.value -= Time.deltaTime;
        customerTimerSlider.value = customerSO.customerFoodTimer;

        Debug.Log("Customer timer: " + customerTimerSlider.value);
      
    }
}
