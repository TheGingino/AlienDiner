using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class DishPrefabEntry
{
    public DishType dishType;
    public GameObject prefab;
}

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] Transform _handPoint;
    [SerializeField] List<DishPrefabEntry> _dishPrefabs;

    bool _isBusy;
    DishType _heldDish = DishType.None;
    GameObject _heldDishObject;
    public bool IsHoldingDish => _heldDish != DishType.None;



    public bool IsBusy => _isBusy;

    public void StartInteraction(InteractableObject station)
    {
        if (_isBusy) return;
        
        StartCoroutine(InteractionRoutine(station));
    }

    IEnumerator InteractionRoutine(InteractableObject station)
    {
        _isBusy = true;

        LockPlayer();
        FaceStation(station);
        station.ShowProgress();

        float timer = 0f;

        while (timer < station.InteractionDuration)
        {
            timer += Time.deltaTime;

            float normalized = Mathf.Clamp01(timer / station.InteractionDuration);
            station.UpdateProgress(normalized);

            yield return null;
        }

        station.HideProgress();
        station.OnInteractionComplete();
        
        DishType result = station.ProcessDish(_heldDish);

        Debug.Log("Station returned dish: " + result);

        
        if (station.Type == InteractableObject.StationType.TrashCan)
        {
            ClearDish();
        }
        else if (result != DishType.None)
        {
            GiveDish(result);
        }

        UnlockPlayer();
        _isBusy = false;
    }

    void LockPlayer()
    {
        Debug.Log("Player Locked");
        GetComponent<PlayerMovement>().LockPlayerMovement(false);

    }

    void UnlockPlayer()
    {
        Debug.Log("Player Unlocked");
        GetComponent<PlayerMovement>().LockPlayerMovement(true);

    }

    public void GiveDish(DishType dish)
    {
        _heldDish = dish;

        Debug.Log("Player now holds: " + dish);

        if (_heldDishObject != null)
            Destroy(_heldDishObject);

        GameObject prefab = GetPrefabForDish(dish);

        if (prefab != null)
        {
            _heldDishObject = Instantiate(prefab, _handPoint);
            _heldDishObject.transform.localPosition = Vector3.zero;
            _heldDishObject.transform.localRotation = Quaternion.identity;
        }
    }
    
    void ClearDish()
    {
        _heldDish = DishType.None;

        if (_heldDishObject != null)
        {
            Destroy(_heldDishObject);
            _heldDishObject = null;
        }
    }
    
    GameObject GetPrefabForDish(DishType dish)
    {
        foreach (var entry in _dishPrefabs)
        {
            if (entry.dishType == dish)
                return entry.prefab;
        }

        return null;
    }
    
    void FaceStation(InteractableObject station)
    {
        Vector3 direction = station.transform.position - transform.position;
        direction.y = 0f; 

        if (direction.sqrMagnitude <= 0.001f)
            return;

        transform.rotation = Quaternion.LookRotation(direction);
    }
    
    public void TryServeCustomersAtTable(Transform tableTransform)
    {
        if (!IsHoldingDish)
            return;

        Collider[] hits = Physics.OverlapSphere(tableTransform.position, 2f);

        foreach (Collider hit in hits)
        {
            Customer customer = hit.GetComponent<Customer>();

            if (customer != null && customer.IsWaitingFor(_heldDish))
            {
                customer.ServeFood();
                ClearDish();
                GetComponent<PlayerMovement>().LockPlayerMovement(true);
                return;
            }

            DriveThroughCustomer driveCustomer = hit.GetComponent<DriveThroughCustomer>();

            if (driveCustomer != null && driveCustomer.IsWaitingFor(_heldDish))
            {
                driveCustomer.ServeFood();
                ClearDish();
                GetComponent<PlayerMovement>().LockPlayerMovement(true);
                return;
            }
            
            GetComponent<PlayerMovement>().LockPlayerMovement(true);
        }
    }
}
