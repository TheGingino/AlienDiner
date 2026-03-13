using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] private Image clockImage;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private float startTime = 180f;

    private float _currentTime;
    private bool _isRunning = false;

    private static Timer _instance;
    
    public UnityEvent onLevelFinished;


    void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        _currentTime = startTime;
        _isRunning = true;
        UpdateDisplay();
    }

    public static void RegisterCustomer(Customer customer)
    {
        if (_instance != null)
        {
            customer.HasLeftAngry.AddListener(_instance.ApplyPenalty);
        }
    }

    void Update()
    {
        if (!_isRunning) return;

        _currentTime -= Time.deltaTime;

        if (_currentTime <= 0)
        {
            _currentTime = 0;
            _isRunning = false;
            OnTimerComplete();
        }

        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        if (timerText != null)
        {
            timerText.text = "" + Mathf.CeilToInt(_currentTime);
        }

        if (clockImage != null)
        {
            clockImage.fillAmount = _currentTime / startTime;
        }
    }

    public void ApplyPenalty()
    {
        _currentTime -= 10f;
        Debug.Log("Applied penalty! Current time: " + _currentTime);
        if (_currentTime < 0)
        {
            _currentTime = 0;
        }
        UpdateDisplay();
    }

    private void OnTimerComplete()
    {
        Debug.Log("Timer completed!");
        onLevelFinished.Invoke();
    }
}
