using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelTimer : MonoBehaviour
{
    public Image clockImage;
    public TMP_Text countdownText;
    public float duration = 180f;

    private float elapsedTime;

    void Start()
    {
        elapsedTime = 0f;
        clockImage.fillAmount = 0f;
        countdownText.text = Mathf.CeilToInt(duration).ToString();
    }

    void Update()
    {
        if (elapsedTime >= duration)
            return;

        elapsedTime += Time.deltaTime;

        // Fill clock UP
        clockImage.fillAmount = elapsedTime / duration;

        // Countdown text
        float remaining = duration - elapsedTime;
        countdownText.text = Mathf.CeilToInt(remaining).ToString();

        if (elapsedTime >= duration)
        {
            countdownText.text = "0";
            OnTimerFinished();
        }
    }

    void OnTimerFinished()
    {
        Debug.Log("Timer finished!");
    }
}