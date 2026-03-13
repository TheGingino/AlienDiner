using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialScreen : MonoBehaviour
{
    void Start()
    {
        Time.timeScale = 0f; // Pause the game
    }
    
    public void CloseTutorial()
    {
        Time.timeScale = 1f; // Resume the game
        gameObject.SetActive(false); // Hide the tutorial screen
    }

}
