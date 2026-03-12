using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    [SerializeField] private Customer[] customerSO;
    [SerializeField] private DriveThroughCustomer driveThroughCustomerSO;

    [SerializeField] private float spawnInterval = 5f;
    private float spawnTimer;
    
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform driveThroughSpawnPoint;
    [SerializeField] private AudioClip _spawnSFX;
    [SerializeField] private AudioSource _sfxSource;
    
    private void Start()
    {
        _sfxSource = GetComponent<AudioSource>();
        if (customerSO == null || customerSO.Length == 0)
        {
            customerSO = GetComponentsInChildren<Customer>();
        }
    }

    private void Update()
    {
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnInterval)
        {
            SpawnCustomer(); 
            SpawnDriveThrough(); 
            spawnTimer = 0f; 
        }
    }

    private void SpawnCustomer()
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
