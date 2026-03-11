using System;
using TMPro;
using UnityEngine;

public class RewardSystem : MonoBehaviour
{
    [SerializeField] private int money;
    [SerializeField] private int customerServed;
    
    private TextMeshProUGUI moneyText;
    private TextMeshProUGUI customerServedText;
    
    private void Start()
    {
        money = 0;
        customerServed = 0;
    }

    private void Update()
    {
        UpdateUI();
    }

    public void AddMoney(int amount)
    {
        money += amount;
        Debug.Log("Money: " + money);
        UpdateUI();
    }

    private void UpdateUI()
    {
        moneyText = GameObject.Find("MoneyCounter").GetComponent<TextMeshProUGUI>();
        customerServedText = GameObject.Find("CustomerServedText").GetComponent<TextMeshProUGUI>();
        
        moneyText.text = "Money: " + money;
        customerServedText.text = "Customers Served: " + customerServed;
        
        //Debug.Log("Money: " + money + " | Customers Served: " + customerServed);
    }
    
     public void IncrementCustomerServed()
    {
        customerServed++;
        Debug.Log("Customers Served: " + customerServed);
        UpdateUI();
    }
}
