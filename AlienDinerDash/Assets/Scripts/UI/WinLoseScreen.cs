using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class WinLoseScreen : MonoBehaviour
{
    [SerializeField] private GameObject winLoseScreen;
    [SerializeField] private GameObject levelUI;
    [SerializeField] private int customersNeededToWin = 5; // Configurable win condition

    [SerializeField] private AudioClip[] winLoseSFX;
    [SerializeField] private AudioSource sfxSource;

    public void ShowWinScreen()
    {
        UpdateWinScreenStats();
        Time.timeScale = 0f;
    }

    public void CheckWinLoseCondition()
    {
        RewardSystem rewardSystem = FindObjectOfType<RewardSystem>();
        
        if (rewardSystem != null)
        {
            if (rewardSystem.customerServed >= customersNeededToWin)
            {
                sfxSource.clip = winLoseSFX[0];
                ShowWinScreen();
                sfxSource.Play();

            }
            else
            {
                sfxSource.clip = winLoseSFX[1];
                ShowLoseScreen(); 
                sfxSource.Play();
            }
        }
    }

    private void ShowLoseScreen()
    {
        UpdateWinScreenStats(); // Still show stats on lose screen
        Time.timeScale = 0f;
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("StartScreen");
    }

    public void ReloadLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main");
    }

    private void UpdateWinScreenStats()
    {
        RewardSystem rewardSystem = FindObjectOfType<RewardSystem>();

        if (rewardSystem != null)
        {
            TextMeshProUGUI statusText = GameObject.Find("StatusText")?.GetComponent<TextMeshProUGUI>();
            if (statusText != null)
            {
                if (rewardSystem.customerServed >= customersNeededToWin)
                {
                    statusText.text = "YOU WIN!";
                }
                else
                {
                    statusText.text = "YOU LOSE!";
                }
            }

            TextMeshProUGUI winMoneyText = GameObject.Find("MoneyText")?.GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI winCustomerText = GameObject.Find("CustomerText")?.GetComponent<TextMeshProUGUI>();

            if (winMoneyText != null)
            {
                winMoneyText.text = "Money: " + rewardSystem.money;
            }

            if (winCustomerText != null)
            {
                winCustomerText.text = "Customers Served: " + rewardSystem.customerServed + "/" + customersNeededToWin;
            }
        }

        if (winLoseScreen != null)
        {
            winLoseScreen.SetActive(true);
            levelUI.SetActive(false);
        }
    }
}