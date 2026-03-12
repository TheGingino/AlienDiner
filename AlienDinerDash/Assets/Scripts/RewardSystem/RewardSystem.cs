using System;
using TMPro;
using UnityEngine;

public class RewardSystem : MonoBehaviour
{
    [SerializeField] private int _money;
    [SerializeField] private int _customerServed;
    
    public int money => _money;
    public int customerServed => _customerServed;
    
    private TextMeshProUGUI moneyText;
    private TextMeshProUGUI customerServedText;
    
    
    private void Start()
    {
        _money = 0;
        _customerServed = 0;
    }

    private void Update()
    {
        UpdateUI();
    }

    public void AddMoney(int amount)
    {
        _money += amount;
        Debug.Log("Money: " + _money);
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
        _customerServed++;
        Debug.Log("Customers Served: " + customerServed);
        UpdateUI();
    }
}
