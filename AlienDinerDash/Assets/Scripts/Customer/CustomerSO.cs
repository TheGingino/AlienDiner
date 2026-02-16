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
    
    private void Start()
    {
        switch (customerType)
        {
            case CustomerType.AVERAGE:
                customerTimer = 30;
                customerMoney = 10;
                break;
            case CustomerType.ANNOYING:
                customerTimer = 20;
                customerMoney = 15;
                break;
            case CustomerType.VIP:
                customerTimer = 40;
                customerMoney = 25;
                break;
            case CustomerType.DRIVETHROUGH:
                customerTimer = 25;
                customerMoney = 12;
                break;
        }
    }
}

//Customer type enum
public enum CustomerType
{
    AVERAGE,
    ANNOYING,
    VIP,
    DRIVETHROUGH
}
