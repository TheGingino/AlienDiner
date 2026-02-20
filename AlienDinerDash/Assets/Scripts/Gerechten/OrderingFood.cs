using System;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class OrderingFood : MonoBehaviour
{
    private Customer _customer;
    private DishType[] _dishType;

    [SerializeField] private UnityEvent onFoodServed;

    private void Start()
    {
        _customer = GetComponentInParent<Customer>();
        _dishType = Enum.GetValues(typeof(DishType)) as DishType[];
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && _customer != null)
        {
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
    private void OrderFood()
    {
        if (_dishType == null || _dishType.Length == 0 || _customer == null) return;

        var index = Random.Range(0, _dishType.Length);
        var chosenDish = _dishType[index];
        _customer.SetDesiredDish(chosenDish);

        Debug.Log("Dish ordered for " + _customer.name + ": " + chosenDish);
    }
}
