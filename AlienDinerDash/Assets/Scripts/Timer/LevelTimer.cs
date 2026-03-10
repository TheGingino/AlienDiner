using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelTimer : MonoBehaviour
{
    [SerializeField] private Image clockImage;
    [SerializeField] private TMP_Text countdownText;
    [SerializeField] private float startTime = 180f;
    
    private float currentTime;
    private bool isRunning = false;
    
    public event Action OnTimerEnd;
    
    private void Start()
    {
        Debug.Log("LevelTimer Start() called");
        currentTime = startTime;
        UpdateUI();
        StartTimer();
        Debug.Log($"Timer started. IsRunning: {isRunning}, CurrentTime: {currentTime}");
    }
    
    private void Update()
    {
        if (!isRunning) return;

        currentTime -= Time.deltaTime;
        Debug.Log($"Current Time: {MathF.Floor(currentTime)}"); // Add this line

        if (currentTime <= 0)
        {
            currentTime = 0;
            isRunning = false;
            OnTimerEnd?.Invoke();
        }

        UpdateUI();
    }
    
    private void UpdateUI()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        countdownText.text = $"{minutes:00}:{seconds:00}";
        
        clockImage.fillAmount = currentTime / startTime;
        if (Input.anyKeyDown)
        {
            ApplyPenalty();
        }
    }
    
    public void ApplyPenalty()
    {
        currentTime -=  currentTime * 0.1f;
        if (currentTime < 0)
            currentTime = 0;
        
        UpdateUI();
    }
    
    public void StartTimer()
    {
        isRunning = true;
    }
    
    public void StopTimer()
    {
        isRunning = false;
    }
}