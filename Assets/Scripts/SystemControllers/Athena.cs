using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace SystemControllers
{
    public class Athena : NetworkBehaviour
    {
        private GameObject _pauseMenu;
        public GameObject mainMenu;
        private GameObject _onlineMenu;
        private GameObject _changeMenu;
        public GameObject simulation;
        private GameObject _backgroundUI;
        

        private NetworkManager _networkManager;

        private bool _isHost;
        private void Awake()
        {
            _onlineMenu = GameObject.Find("OnlineMenu");
            _pauseMenu = GameObject.Find("PauseMenu");
            _backgroundUI = GameObject.Find("Background");
            _changeMenu = GameObject.Find("ChangeMenu");

            _networkManager = GetComponent<NetworkManager>();
        }

        private void Update()
        {
            if (isClient || isServer)
            {
                _onlineMenu.SetActive(false);
                simulation.SetActive(true);
                mainMenu.SetActive(true);
                _backgroundUI.SetActive(false);
            }
        }

        public void HostServer()
        {
            if (!NetworkClient.active)
            {
                Debug.Log($"Server started at {_networkManager.networkAddress} via {Transport.active}");
                _networkManager.StartHost();
            }
        }

        public void ExitOnline()
        {
            if (isServer)
            {
                Debug.Log("Server stopped");
                _networkManager.StopHost();   
            }

            if (isClient)
            {
                Debug.Log("Client dc");
                _networkManager.StopClient();
            }
            SceneManager.LoadScene(2);
        }
    }
}
