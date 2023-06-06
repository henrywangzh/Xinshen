using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public int FirstLevel, UI;
    public GameObject MainMenu, CreditMenu, SettingsMenu, PauseMenu, PauseButton;
    [SerializeField] private CanvasGroup bgText;
    [SerializeField] private GameObject textObject;
    [SerializeField] private GameObject buttons;
    [SerializeField] private CanvasGroup buttonAlpha;
    private TMP_Text XinshenText;
    private bool fadeIn;
    private float timePerCharacter = 0.03f;
    private float timer;
    private int ind;
    private string textToWrite = "Xinshen";
    private bool fadeInButtons;
    
    public void StartGame()
    {
        SceneManager.LoadScene("FirstLevel");
        
    }

    public void Start()
    {
        bgText.alpha = 0;
        fadeIn = true;
        fadeInButtons = false;
        XinshenText = textObject.GetComponent<TMP_Text>();
        XinshenText.text = textToWrite.Substring(0, ind);
        buttons.SetActive(false);
        buttonAlpha.alpha = 0;

    }

    public void FixedUpdate()
    {
        if (fadeIn)
        {
            if (bgText.alpha < 1)
            {
                bgText.alpha += Time.deltaTime/4.3f;
            }

            if (bgText.alpha >= 1)
            {
                timer -= Time.deltaTime;
                if (timer <= 0f && ind < textToWrite.Length)
                {
                    timer += timePerCharacter;
                    ind++;
                    XinshenText.text = textToWrite.Substring(0, ind);
                }
            }

            if (ind >= textToWrite.Length)
            {
                buttons.SetActive(true);
            }
            
        }
        else
        {
            ind = 0;
            bgText.alpha = 0;
        }

        if (buttons.activeSelf)
        {
            if (buttonAlpha.alpha < 1)
            {
                buttonAlpha.alpha += Time.deltaTime/5f;
            }
        }
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
        fadeIn = true;
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