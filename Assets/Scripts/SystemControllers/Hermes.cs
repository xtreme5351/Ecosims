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
        
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                mainMenu.SetActive(s_GameIsPaused);
                s_GameIsPaused = !s_GameIsPaused;
                pauseMenuUI.SetActive(s_GameIsPaused);
            }
        }

        public void QuitSession()
        {
            SceneManager.LoadScene(0);
        }
    }
   
}

