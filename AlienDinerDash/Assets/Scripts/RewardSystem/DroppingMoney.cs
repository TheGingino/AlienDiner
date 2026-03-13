using UnityEngine;

public class DroppingMoney : MonoBehaviour
{
    [SerializeField] private RewardSystem rewardSystem;
    [SerializeField] private Money moneyPrefab;

    private int moneyToDrop;
    
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
