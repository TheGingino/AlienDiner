using System;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class OrderingFood : MonoBehaviour
{
    private Customer _customer;
    private DriveThroughCustomer _driveThroughCustomer;
    private bool _hasOrdered = false;

    [SerializeField] private UnityEvent onFoodSOrdered;
    [SerializeField] private UnityEvent onFoodServed;
    [SerializeField] DishType[] _orderableDishes;


    private void Awake()
    {
        _customer = GetComponentInParent<Customer>();
        _driveThroughCustomer = GetComponent<DriveThroughCustomer>();

        Debug.Log("DriveThroughCustomer found: " + _driveThroughCustomer);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && _customer != null)
        {
            OrderFood();
            Debug.Log("Food ordered for " + _customer.name);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            onFoodServed.Invoke();

            if (_customer != null)
                Debug.Log("Player entered ordering area for " + _customer.name);
        }
    }

    [ContextMenu("Order Food")]
    public void OrderFood()
    {
        if (_hasOrdered)
            return;

        if (_orderableDishes == null || _orderableDishes.Length == 0)
            return;

        var index = Random.Range(0, _orderableDishes.Length);
        var chosenDish = _orderableDishes[index];

        if (_customer != null)
        {
            _customer.SetDesiredDish(chosenDish);
            Debug.Log("Dish ordered for " + _customer.name + ": " + chosenDish);
        }
        else if (_driveThroughCustomer != null)
        {
            _driveThroughCustomer.SetDesiredDish(chosenDish);
            Debug.Log("Drive-through ordered: " + chosenDish);
        }

        onFoodSOrdered.Invoke();
        _hasOrdered = true;
    }
}