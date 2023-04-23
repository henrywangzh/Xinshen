using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public int FirstLevel, UI;
    public GameObject MainMenu, CreditMenu, SettingsMenu, PauseMenu, PauseButton;

    public void StartGame()
    {
        SceneManager.LoadScene("FirstLevel");

    }

    public void Settings()
    {
        MainMenu.SetActive(false);
        CreditMenu.SetActive(false);
        SettingsMenu.SetActive(true);
    }

    public void Credit()
    {
        MainMenu.SetActive(false);
        CreditMenu.SetActive(true);
        SettingsMenu.SetActive(false);
    }

    public void Back()
    {
        MainMenu.SetActive(true);
        CreditMenu.SetActive(false);
        SettingsMenu.SetActive(false);
        //pauseMenu.SetActive(false);
        //PauseButton.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
        SceneManager.LoadScene("UI");
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        PauseMenu.SetActive(true);
        PauseButton.SetActive(false);
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        PauseMenu.SetActive(false);
        PauseButton.SetActive(true);
    }
}