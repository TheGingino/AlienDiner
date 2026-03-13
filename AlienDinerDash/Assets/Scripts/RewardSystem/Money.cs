using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money : MonoBehaviour
{
    private CustomerSO customerSO;
    [SerializeField] private int moneyValue;

    private void Start()
    {
        Customer customer = FindObjectOfType<Customer>();
        DriveThroughCustomer driveThroughCustomer = FindObjectOfType<DriveThroughCustomer>();
        if (customer == null)
        {
            Debug.LogError("No Customer found in scene (FindObjectOfType<Customer>() returned null).");
            return;
        }

        customerSO = customer.CustomerSO;
        moneyValue = customerSO.customerMoney;
        
        customerSO = driveThroughCustomer.CustomerSO;

        Debug.Log("Added " + moneyValue + " money!");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            RewardSystem rewardSystem = FindObjectOfType<RewardSystem>();
            
            rewardSystem.AddMoney(moneyValue);
            rewardSystem.IncrementCustomerServed();
            Debug.Log("Added " + moneyValue + " money!");
            
            //Destroy(gameObject);        
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            RewardSystem rewardSystem = FindObjectOfType<RewardSystem>();
            rewardSystem.AddMoney(moneyValue);
            rewardSystem.IncrementCustomerServed();
            Destroy(gameObject);
        }
    }
}
