using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SystemControllers
{
    public class Hermes : MonoBehaviour
    {
        private static bool s_GameIsPaused;
        public GameObject pauseMenuUI;
        public GameObject mainMenu;

        public void Start()
        {
            s_GameIsPaused = false;
        }

        void Update()
        {
            // If the key pressed is the escape key, pause the game
            // This condition is checked every frame by this object
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                // Boolean logic to toggle the main menu on and off based on the game state.
                mainMenu.SetActive(s_GameIsPaused);
                s_GameIsPaused = !s_GameIsPaused;
                pauseMenuUI.SetActive(s_GameIsPaused);
            }
        }

        public void QuitSession()
        {
            // Load the home page to exit the session
            SceneManager.LoadScene(0);
        }

        public void PauseGame()
        {
            mainMenu.SetActive(false);
            pauseMenuUI.SetActive(true);
            s_GameIsPaused = true;
        }
    }
   
}



