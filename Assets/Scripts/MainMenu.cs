using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject normalMenu;
    public GameObject forgotMenu;
    public GameObject newUserMenu;
    
    public void PlayLocalGame()
    {
        SceneManager.LoadScene(2);
    }

    public void QuitGame()
    {
        Debug.Log("Exited");
        Application.Quit();
    }

    public void LoginScreen()
    {
        SceneManager.LoadScene(1);
    }

    public void Back()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void ForgotDetails()
    {
        normalMenu.SetActive(false);
        newUserMenu.SetActive(false);
        forgotMenu.SetActive(true);
    }

    public void NewUser()
    {
        normalMenu.SetActive(false);
        forgotMenu.SetActive(false);
        newUserMenu.SetActive(true);
    }

    public void NormalMenu()
    {
        forgotMenu.SetActive(false);
        newUserMenu.SetActive(false);
        normalMenu.SetActive(true);
    }
}
