using UnityEngine;

public class StationClickHandler : MonoBehaviour
{
    [SerializeField] PlayerInteraction _player;

    void OnMouseDown()
    {
        if (_player.IsBusy) return;

        InteractableObject interactable = GetComponent<InteractableObject>();

        if (interactable != null)
        {
            _player.StartInteraction(interactable);
        }
    }
}