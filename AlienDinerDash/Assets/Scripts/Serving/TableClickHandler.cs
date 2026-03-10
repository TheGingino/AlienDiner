using UnityEngine;

public class TableClickHandler : MonoBehaviour
{
    [SerializeField] PlayerMovement _playerMovement;
    [SerializeField] PlayerInteraction _player;
    [SerializeField] Transform _servePoint;

    void OnMouseDown()
    {
        if (_player.IsBusy)
            return;

        if (!_player.IsHoldingDish)
            return;

        _playerMovement.MoveToTable(_servePoint, transform);
    }
}