using UnityEngine;

public class StationClickHandler : MonoBehaviour
{
    [SerializeField] PlayerInteraction _player;
    [SerializeField] PlayerMovement _playerMovement;

    void OnMouseDown()
    {
        InteractableObject station = GetComponent<InteractableObject>();

        if (_player.IsBusy)
            return;
        Debug.Log("Holding dish? " + _player.IsHoldingDish);

        // NEW CHECK
        if (_player.IsHoldingDish &&
            station.Type == InteractableObject.StationType.CookingStation)
        {
            Debug.Log("Hands full — cannot cook.");
            return;
        }
        
        // Empty hands → cannot use trashcan
        if (!_player.IsHoldingDish &&
            station.Type == InteractableObject.StationType.TrashCan)
        {
            Debug.Log("Nothing to throw away.");
            return;
        }

       
        
        _playerMovement.MoveToInteraction(station);
        

       
    }
}