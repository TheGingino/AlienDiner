using System;
using System.Collections;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Customer : MonoBehaviour
{
    [Header("Customer Settings")] [SerializeField]
    private CustomerSO customerSO;

    public CustomerSO CustomerSO => customerSO;
    
    [SerializeField] private UnityEvent hasFinishedEating;


    public enum CustomerStates
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
    private int _nextWaypointIndex;

    [SerializeField] private float speed = 2f;
    [SerializeField] private float reachDistance = 0.1f;

    [Header("Visual Testing")] [SerializeField]
    private Slider customerTimerSlider;
    private float _sliderTime;
    
    [Header ("Order Visuals")]
    [SerializeField] private Image orderImage;
    [SerializeField] private DishSpriteEntry[] dishSprites;
    
    [SerializeField] private Animator _animator;
    private GameObject[] customerWaypoints;
    private void Start()
    {
        _waypointToLeave = FindObjectOfType<WaypointToLeave>();
        customerTimerSlider = FindObjectOfType<Slider>();
        orderImage.enabled = false;
        
        _animator = GetComponent<Animator>();
        if (!_animator)
        {
            Debug.LogError("Animator component not assigned in the inspector.");
        }
        
        customerWaypoints = _waypointToLeave.insideWaypointToLeave;
        
        customerTimerSlider.maxValue = customerSO.customerTimer + customerSO.customerFoodTimer;
        customerTimerSlider.value = customerSO.customerTimer + customerSO.customerFoodTimer;
        //Debug.Log(customerTimerSlider.value);
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
        _sliderTime = customerTimerSlider.value;
        switch (customerSO.customerType)
        {
            case CustomerType.ANNOYING:
                yield return StartCoroutine(AnnoyingCustomer(_sliderTime));
                Debug.Log("Annoying customer finished waiting." + _sliderTime);
                break;
            case CustomerType.AVERAGE:
                yield return StartCoroutine(NormalCustomer(_sliderTime));
                Debug.Log("Average customer finished waiting." + _sliderTime);
                break;
            case CustomerType.PATIENT:
                yield return StartCoroutine(PatientCustomer(_sliderTime));
                Debug.Log("Patient customer finished waiting." + _sliderTime);
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
                _animator.SetBool("Sit", true);
                currentState = CustomerStates.SERVED;
                Debug.Log("Customer received food!");
                break;
            }
            waitTime -= Time.deltaTime;
            yield return null;
        }
        
        if (!hasBeenServed && customerTimerSlider.value <= 0)
        {
            if (customerSO.customerType == CustomerType.ANNOYING)
            {
                Debug.Log("Customer got tired of waiting and left!");
                currentState = CustomerStates.LEAVING;
                customerWaypoints = _waypointToLeave.waypointToLeave;
                LeaveRestaurant();
            }
        }
    }
    
    public bool IsWaitingFor(DishType dish)
    {
        return currentState == CustomerStates.HUNGRY && desiredDish == dish;
    }

    public void ServeFood()
    {
        if (currentState == CustomerStates.HUNGRY)
        {
            hasBeenServed = true;
            currentState = CustomerStates.SERVED;
            orderImage.enabled = false;
            Debug.Log("Customer received food!");
            StartCoroutine(EatThenLeave());
        }
    }

    [ContextMenu("Testing the ability to leave the restaurant")]
    private void LeaveRestaurant()
    {
        Debug.Log("Customer is leaving");
                        
        DroppingMoney droppingMoney = GetComponent<DroppingMoney>();
        droppingMoney.DropMoney();
        StartCoroutine(MoveToExit());
    }

    private IEnumerator MoveToExit()
    {
        _animator.SetBool("Walk", true);
        while (_nextWaypointIndex < customerWaypoints.Length)
        {
            transform.position = Vector3.MoveTowards(transform.position,
                customerWaypoints[_nextWaypointIndex].transform.position,
                Time.deltaTime * speed);

            if (Vector3.Distance(transform.position,
                    customerWaypoints[_nextWaypointIndex].transform.position) <= reachDistance)
            {
                _nextWaypointIndex += 1;
            }
            hasFinishedEating.Invoke();
            
            yield return null;
        }
        _animator.SetBool("Walk", false);
        Destroy(gameObject);
    }
    
    IEnumerator EatThenLeave()
    {
        yield return new WaitForSeconds(5f);

        currentState = CustomerStates.LEAVING;
        customerWaypoints = _waypointToLeave.waypointToLeave;

        LeaveRestaurant();
    }
    

    private void DecreaseSliderValue()
    {
        if (customerTimerSlider.value >= 0) 
        {
            customerTimerSlider.value -= Time.deltaTime;
        }
        //Debug.Log("Customer timer: " + customerTimerSlider.value);
    }
    
    private DishType desiredDish;

    public void SetDesiredDish(DishType dish)
    {
        desiredDish = dish;

        if (orderImage != null)
        {
            orderImage.sprite = GetSpriteForDish(dish);
            orderImage.enabled = true;
        }

        Debug.Log($"Customer {name} wants: {desiredDish}");
    }
    
    private Sprite GetSpriteForDish(DishType dish)
    {
        foreach (var entry in dishSprites)
        {
            if (entry.dishType == dish)
                return entry.sprite;
        }

        return null;
    }
    
    [System.Serializable]
    public class DishSpriteEntry
    {
        public DishType dishType;
        public Sprite sprite;
    }
}