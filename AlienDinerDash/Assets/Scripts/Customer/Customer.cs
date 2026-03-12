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

    private CustomerSeating _customerSeating;
    public CustomerSO CustomerSO => customerSO;
    
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

    [Header("Waypoint to leave")] 
    private WaypointToLeave _waypointToLeave;
    private int _nextWaypointIndex;
    private GameObject[] customerWaypoints;
    
    [SerializeField] private float speed = 2f;
    [SerializeField] private float reachDistance = 0.1f;

    [Header("Visual Testing")] [SerializeField]
    private Slider customerTimerSlider;
    private float _sliderTime;
    
    [Header ("Order Visuals")]
    [SerializeField] private Image orderImage;
    [SerializeField] private DishSpriteEntry[] dishSprites;
    [SerializeField] private Transform foodSpawnPoint;
    private GameObject _spawnedFood;
    
    [Header("Events")]    
    [SerializeField] private UnityEvent hasFinishedEating;
    [SerializeField] private UnityEvent hasLeftAngry;
    public UnityEvent HasLeftAngry => hasLeftAngry;
    
    [SerializeField] private Animator _animator;

    [SerializeField] private AudioClip[] _customerSFX;
    [SerializeField] private AudioSource sfxSource;

    private void Start()
    {
        _waypointToLeave = FindObjectOfType<WaypointToLeave>();
        customerTimerSlider = FindObjectOfType<Slider>();
        _customerSeating = GetComponent<CustomerSeating>();
        orderImage.enabled = false;
        
        _animator = GetComponent<Animator>();
        customerWaypoints = _waypointToLeave.insideWaypointToLeave;
        
        customerTimerSlider.maxValue = 10;
        customerTimerSlider.value = 10;
        
        Timer.RegisterCustomer(this);
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
                //Debug.Log("Annoying customer finished waiting." + _sliderTime);
                break;
            case CustomerType.AVERAGE:
                yield return StartCoroutine(NormalCustomer(_sliderTime));
                //Debug.Log("Average customer finished waiting." + _sliderTime);
                break;
            case CustomerType.PATIENT:
                yield return StartCoroutine(PatientCustomer(_sliderTime));
                //Debug.Log("Patient customer finished waiting." + _sliderTime);
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
                hasLeftAngry.Invoke();
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
            _animator.SetBool("Eat", true);
            GameObject foodPrefab = GetPrefabForDish(desiredDish);
            if (foodPrefab != null)
            {
                _spawnedFood = Instantiate(foodPrefab, foodSpawnPoint.position, Quaternion.identity);
            }
            _animator.SetBool("Sit", false);
            Debug.Log("Customer received food!");
            StartCoroutine(EatThenLeave());
        }
    }

    [ContextMenu("Testing the ability to leave the restaurant")]
    private void LeaveRestaurant()
    {
        if (_customerSeating != null)
            _customerSeating.LeaveSeat();
        
        StartCoroutine(MoveToExit());
    }

    private IEnumerator MoveToExit()
    {
        _animator.SetBool("Eat", false);
        sfxSource.clip = _customerSFX[0];

        sfxSource.loop = true;
        sfxSource.Play();

        _animator.SetBool("Walk", true);
        while (_nextWaypointIndex < customerWaypoints.Length)
        {
            transform.position = Vector3.MoveTowards(transform.position,
                customerWaypoints[_nextWaypointIndex].transform.position,
                Time.deltaTime * speed);
            transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.LookRotation(customerWaypoints[_nextWaypointIndex].transform.position - transform.position),
                Time.deltaTime * speed);

            if (Vector3.Distance(transform.position,
                    customerWaypoints[_nextWaypointIndex].transform.position) <= reachDistance)
            {
                _nextWaypointIndex += 1;
            }
            
            yield return null;
        }
        _animator.SetBool("Walk", false);
        Destroy(gameObject);
    }
    
    IEnumerator EatThenLeave()
    {

        sfxSource.clip = _customerSFX[3];

        yield return new WaitForSeconds(5f);
        if (_spawnedFood != null)
            Destroy(_spawnedFood);
        currentState = CustomerStates.LEAVING;
        customerWaypoints = _waypointToLeave.waypointToLeave;

        GetMoney();
        LeaveRestaurant();
    }
    
    private void DecreaseSliderValue()
    {
        if (_customerSeating != null && _customerSeating.IsSeated)
            return;
        
        if (customerTimerSlider.value >= 0) 
        {
            customerTimerSlider.value -= Time.deltaTime;
        }
        //Debug.Log("Customer timer: " + customerTimerSlider.value);
    }
    
    private DishType desiredDish;

    [ContextMenu("Set desired dish for testing")]
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
    
    private GameObject GetPrefabForDish(DishType dish)
    {
        foreach (var entry in dishSprites)
        {
            if (entry.dishType == dish)
                return entry.foodPrefab;
        }

        return null;
    }
    
    [System.Serializable]
    public class DishSpriteEntry
    {
        public DishType dishType;
        public Sprite sprite;
        public GameObject foodPrefab;
    }
    
    private void GetMoney()
    {
        int moneyValue = customerSO.customerMoney;
        Debug.Log(moneyValue + " money value from customerSO.");
        RewardSystem rewardSystem = FindObjectOfType<RewardSystem>();
        
        rewardSystem.AddMoney(moneyValue);
        rewardSystem.IncrementCustomerServed();
    }
}