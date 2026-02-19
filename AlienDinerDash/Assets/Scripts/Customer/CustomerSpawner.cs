using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    [SerializeField] private Customer[] customerSO;
    [SerializeField] private Customer driveThroughCustomerSO;

    [SerializeField] private float spawnInterval = 5f;
    private float spawnTimer;
    
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform driveThroughSpawnPoint;
    
    private void Start()
    {
        if (customerSO == null || customerSO.Length == 0)
        {
            customerSO = GetComponentsInChildren<Customer>();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            SpawnCustomer(); 
        }
        
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnInterval)
        {
            //SpawnCustomer(); 
            SpawnDriveThrough(); 
            spawnTimer = 0f; 
        }
    }

    public void SpawnCustomer()
    {
        if (customerSO == null || customerSO.Length == 0) return;
        
        var index = Random.Range(0, customerSO.Length);
        Debug.Log("Customer " + customerSO[index].name);
        Instantiate(customerSO[index].gameObject, spawnPoint.position, Quaternion.identity);
    }

    [SerializeField] private bool spawnDriveThrough = false;
    void SpawnDriveThrough()
    {
        if (spawnDriveThrough)
        {
            Instantiate(driveThroughCustomerSO.gameObject, driveThroughSpawnPoint.position, Quaternion.identity);
        }
    }
}
