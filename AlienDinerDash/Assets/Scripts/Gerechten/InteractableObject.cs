using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    [SerializeField] float _interactionDuration = 2f;

    public float InteractionDuration => _interactionDuration;

    public void OnInteractionComplete()
    {
        Debug.Log("Interaction complete" + name);
    }
    
}
