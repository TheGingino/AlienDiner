using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private AudioSource clickSFX;
    public void PlayGame()
    {
        clickSFX.Play();
        SceneManager.LoadScene("Main");
    }
    
    public void QuitGame()
    {
        clickSFX.Play();
        Application.Quit();
    }
}
