using UnityEngine;
using UnityEngine.UI;

public class InteractableObject : MonoBehaviour
{
    [SerializeField] float _interactionDuration = 2f;
    [SerializeField] float _heightOffsetY = 1.5f;
    [SerializeField] GameObject _progressBarPrefab;
    [SerializeField] DishType _dishType;

    GameObject _currentProgressBar;
    Image _progressImage;
    Renderer _renderer;

    public float InteractionDuration => _interactionDuration;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }

    public void ShowProgress()
    {
        if (_progressBarPrefab == null) return;

        Vector3 spawnPosition = GetTopCenterPosition();

        _currentProgressBar = Instantiate(
            _progressBarPrefab,
            spawnPosition,
            Quaternion.identity
        );

        _progressImage = _currentProgressBar.GetComponentInChildren<Image>();
        _progressImage.fillAmount = 0f;
    }

    Vector3 GetTopCenterPosition()
    {
        if (_renderer == null)
            return transform.position + Vector3.up * _heightOffsetY;

        Bounds bounds = _renderer.bounds;

        return new Vector3(
            bounds.center.x,
            bounds.max.y + _heightOffsetY,
            bounds.center.z
        );
    }

    public void UpdateProgress(float normalizedValue)
    {
        if (_progressImage == null) return;

        _progressImage.fillAmount = normalizedValue;
    }

    public void HideProgress()
    {
        if (_currentProgressBar != null)
        {
            Destroy(_currentProgressBar);
            _progressImage = null;
        }
    }

    public void OnInteractionComplete()
    {
        Debug.Log("Interaction complete " + name);
    }
}