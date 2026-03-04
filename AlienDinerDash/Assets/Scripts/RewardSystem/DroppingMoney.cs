using UnityEngine;

public class DroppingMoney : MonoBehaviour
{
    [SerializeField] private RewardSystem rewardSystem;
    [SerializeField] private int moneyToDrop;
    
    private Customer.CustomerStates currentState = Customer.CustomerStates.LEAVING;
    
    [SerializeField] private Money moneyPrefab;

    private void Start()
    {
        rewardSystem = FindObjectOfType<RewardSystem>();
    }

    public void DropMoney()
    {
        Instantiate(moneyPrefab, transform.position, Quaternion.identity);
        //Debug.Log("Dropped " + moneyToDrop + " money!");
    }
}
