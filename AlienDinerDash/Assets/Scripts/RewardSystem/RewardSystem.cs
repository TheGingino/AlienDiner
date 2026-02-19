using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardSystem : MonoBehaviour
{
    [SerializeField] private int money;
    
    public void AddMoney(int amount)
    {
        money += amount;
        Debug.Log("Money: " + money);
    }
}
