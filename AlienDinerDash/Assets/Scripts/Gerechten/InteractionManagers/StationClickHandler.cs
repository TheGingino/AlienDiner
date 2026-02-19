using UnityEngine;

public class StationClickHandler : MonoBehaviour
{
    [SerializeField] PlayerInteraction _player;
    [SerializeField] PlayerMovement _playerMovement;

    void OnMouseDown()
    {
        if (_player.IsBusy) return;
        
        _playerMovement.MoveToInteraction(GetComponent<InteractableObject>());
    }
}