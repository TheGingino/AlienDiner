using UnityEngine;
using UnityEngine.UI;

public class InteractableObject : MonoBehaviour
{
    [SerializeField] float _interactionDuration = 2f;
    [SerializeField] float _heightOffsetY = 1.5f;
    [SerializeField] GameObject _progressBarPrefab;
    [SerializeField]  Transform _interactionWaypoint;
    [SerializeField] private ParticleSystem[] _interactionParticles;
    [SerializeField] StationType _stationType;
    [SerializeField] DishType _dishType;
    [SerializeField] AudioSource _prepareSFX;
    
    GameObject _currentProgressBar;
    Image _progressImage;
    Renderer _renderer;

    public float InteractionDuration => _interactionDuration;
    public Transform InteractionWaypoint => _interactionWaypoint;
   
    public StationType Type => _stationType;
    
    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
       StopParticles();
        
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
       
       _prepareSFX.Play();
       PlayParticles();
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
        StopParticles();
    }

    public void OnInteractionComplete()
    {
        Debug.Log("Interaction complete " + name);
    }
    
    public enum StationType
    {
        CookingStation,
        TrashCan
    }

    public DishType ProcessDish(DishType inputDish)
    {
        switch (_stationType)
        {
            case StationType.CookingStation:
                return _dishType;
            
            case StationType.TrashCan:
                return DishType.None;
            
            default:
                return DishType.None;
        }
    }
    
    
    void PlayParticles()
    {
        if (_interactionParticles == null) return;

        foreach (var particle in _interactionParticles)
        {
            if (particle != null)
                particle.Play();
        }
    }
    
    void StopParticles()
    {
        if (_interactionParticles == null) return;

        foreach (var particle in _interactionParticles)
        {
            if (particle != null)
                particle.Stop();
        }
    }
}