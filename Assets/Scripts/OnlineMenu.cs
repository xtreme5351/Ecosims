using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnlineMenu : MonoBehaviour
{
    // Menu object connections
    public GameObject onlineMenu;
    public GameObject changeMenu;
    
    // Method to spawn the online menu by setting it to active
    public void OnlineNormal()
    {
        changeMenu.SetActive(false);
        onlineMenu.SetActive(true);
    }
    
    // Method to spawn the change details menu by setting it to active
    public void ChangeMenu()
    {
        onlineMenu.SetActive(false);
        changeMenu.SetActive(true);
    }

    // Simple method to load the main menu upon exit
    public void ExitOnline()
    {
        SceneManager.LoadScene(0);
    }
}


