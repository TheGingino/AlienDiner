using UnityEngine;

public class TableClickHandler : MonoBehaviour
{
    private PlayerInteraction _player;
    private PlayerMovement _playerMovement;
    [SerializeField] Transform _servePoint;

    private void Awake()
    {
        _player = FindObjectOfType<PlayerInteraction>();
        _playerMovement = FindObjectOfType<PlayerMovement>();
    }
    
    void OnMouseDown()
    {
        if (_player.IsBusy)
            return;

        if (!_player.IsHoldingDish)
            return;

        _playerMovement.MoveToTable(_servePoint, transform);
    }
}