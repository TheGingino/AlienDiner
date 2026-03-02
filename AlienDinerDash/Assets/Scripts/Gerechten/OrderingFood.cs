using System;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class OrderingFood : MonoBehaviour
{
    private Customer _customer;
    private bool _hasOrdered = false;

    [SerializeField] private UnityEvent onFoodSOrdered;
    [SerializeField] private UnityEvent onFoodServed;
    [SerializeField] DishType[] _orderableDishes;


    private void Start()
    {
        _customer = GetComponentInParent<Customer>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && _customer != null)
        {
            OrderFood();
            //onFoodSOrdered.Invoke();
            Debug.Log("Food ordered for " + _customer.name);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            onFoodServed.Invoke();
            Debug.Log("Player entered ordering area for " + _customer.name);
        }
    }

    [ContextMenu("Order Food")]
    public void OrderFood()
    {
        if (_hasOrdered == true)
            return;
        
        Debug.Log("ORDER IN");

        if (_orderableDishes == null || _orderableDishes.Length == 0 || _customer == null)
            return;

        var index = Random.Range(0, _orderableDishes.Length);
        var chosenDish = _orderableDishes[index];

        _customer.SetDesiredDish(chosenDish);

        onFoodSOrdered.Invoke();

        Debug.Log("Dish ordered for " + _customer.name + ": " + chosenDish);
        _hasOrdered = true;
    }

}
