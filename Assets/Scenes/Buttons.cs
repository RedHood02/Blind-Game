using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    [SerializeField] GameObject pausePanel;
    [SerializeField] bool isPaused;

    public void Retry()
    {
        SceneManager.LoadScene(2);
    }

    public void Menu()
    {
        SceneManager.LoadScene(0);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void Pause()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && !isPaused)
        {
            pausePanel.SetActive(true);
            Time.timeScale = 0;
            isPaused = true;
        }
        else if(Input.GetKeyDown(KeyCode.Escape) && isPaused)
        {
            pausePanel.SetActive(false);
            Time.timeScale = 1;
            isPaused = false;
        }
    }

    public void Unpause()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1;
        isPaused = false;
    }

    public void Exit()
    {
        Application.Quit();
    }
}
