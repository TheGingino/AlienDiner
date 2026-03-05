using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Customer", menuName = "ScriptableObjects/CustomerSO", order = 1)]
public class CustomerSO : ScriptableObject
{
    public CustomerType customerType;
    public GameObject customerPrefab;
    public float customerTimer;
    public float customerFoodTimer;
    
    public int customerMoney;
    
    private void OnEnable()
    {
        switch (customerType)
        {
            case CustomerType.AVERAGE:
                customerTimer = 30f;
                customerMoney = 10;
                break;
            case CustomerType.ANNOYING:
                customerTimer = 20f;
                customerMoney = 15;
                break;
            case CustomerType.PATIENT:
                customerTimer = 40f;
                customerMoney = 25;
                break;
            case CustomerType.DRIVETHROUGH:
                customerTimer = 25f;
                customerMoney = 12;
                break;
        }

        if (customerFoodTimer <= 0f)
        {
            // default food timer to the visible customer timer unless explicitly configured
            customerFoodTimer = customerTimer;
        }
    }
}

//Customer type enum
public enum CustomerType
{
    AVERAGE,
    ANNOYING,
    PATIENT,
    DRIVETHROUGH
}
