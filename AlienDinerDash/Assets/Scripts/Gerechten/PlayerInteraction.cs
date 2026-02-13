using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    bool _isBusy;

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

        yield return new WaitForSeconds(station.InteractionDuration);

        station.OnInteractionComplete();

        UnlockPlayer();

        _isBusy = false;
    }

    void LockPlayer()
    {
        Debug.Log("Player Locked");
        // disable movement input later
    }

    void UnlockPlayer()
    {
        Debug.Log("Player Unlocked");
    }

}
