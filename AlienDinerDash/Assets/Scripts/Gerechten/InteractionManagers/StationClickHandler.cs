using System;
using UnityEngine;

public class StationClickHandler : MonoBehaviour
{
    private PlayerInteraction _player;
    private PlayerMovement _playerMovement;

    private void Awake()
    {
        _player = FindObjectOfType<PlayerInteraction>();
        _playerMovement = FindObjectOfType<PlayerMovement>();
    }

    void OnMouseDown()
    {
        InteractableObject station = GetComponent<InteractableObject>();

        if (_player.IsBusy)
            return;
        Debug.Log("Holding dish? " + _player.IsHoldingDish);

      
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