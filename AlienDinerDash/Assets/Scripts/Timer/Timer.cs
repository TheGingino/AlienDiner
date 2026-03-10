
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] private Image clockImage;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private float startTime = 180f;

    private float currentTime;
    private bool isRunning = false;

    private static Timer _instance;

    void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        currentTime = startTime;
        isRunning = true;
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
        if (!isRunning) return;

        currentTime -= Time.deltaTime;

        if (currentTime <= 0)
        {
            currentTime = 0;
            isRunning = false;
            OnTimerComplete();
        }

        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        if (timerText != null)
        {
            timerText.text = "" + Mathf.CeilToInt(currentTime);
        }

        if (clockImage != null)
        {
            clockImage.fillAmount = currentTime / startTime;
        }
    }

    public void ApplyPenalty()
    {
        currentTime -= 10f;
        Debug.Log("Applied penalty! Current time: " + currentTime);
        if (currentTime < 0)
        {
            currentTime = 0;
        }
        UpdateDisplay();
    }

    private void OnTimerComplete()
    {
        Debug.Log("Timer has ended!");
    }
}
