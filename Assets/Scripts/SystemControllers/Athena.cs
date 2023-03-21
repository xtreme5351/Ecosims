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
        // Private GameObject declaration, these are just the different menus.
        private GameObject _pauseMenu;
        private GameObject _onlineMenu;
        private GameObject _changeMenu;
        private GameObject _backgroundUI;
        
        // Public GameObject declaration, these need to be forced external imports
        public GameObject mainMenu;
        public GameObject simulation;
        
        // Network Manager script import to query the status of the server
        private NetworkManager _networkManager;
        
        // This is the constructor method that is loaded when the script is loaded into memory.
        // It initialises all the private objects that are not externally imported/resolved.
        private void Awake()
        {
            // Find the GameObjects with their respective names and allocated them to their respective objects
            _onlineMenu = GameObject.Find("OnlineMenu");
            _pauseMenu = GameObject.Find("PauseMenu");
            _backgroundUI = GameObject.Find("Background");
            _changeMenu = GameObject.Find("ChangeMenu");
            
            //  Instantiate the network manager with its script.
            _networkManager = GetComponent<NetworkManager>();
        }
        
        // Method called every frame
        private void Update()
        {
            // If there is a connection detected, hide all the other menus apart from the main simulation UI.
            // A connection is detected through these inherited attributes from the NetworkBehaviour parent.
            if (isClient || isServer)
            {
                _onlineMenu.SetActive(false);
                simulation.SetActive(true);
                mainMenu.SetActive(true);
                _backgroundUI.SetActive(false);
            }
        }
    
        // Method that is called when the Host button is pressed
        public void HostServer()
        {
            // Check to see if there is an already active server, do nothing if there is.
            if (!NetworkClient.active)
            {
                // If there is no active server, create one using the KCP transport protocol
                Debug.Log($"Server started at {_networkManager.networkAddress} via {Transport.active}");
                _networkManager.StartHost();
            }
        }
        
        // Method called when the Join button is pressed.
        public void JoinServer()
        {
            // Simply joins the server as a client
            Debug.Log("Client connected");
            _networkManager.StartClient();
        }
        
        // Method that is called to exit the online. This is used inside the simulation UI, when the pause menu occurs.
        public void ExitOnline()
        {
            // Simple check to close whatever instance is running.
            // If the server is running, close the server. If the client is connected, close the connection.
            // This is to prevent any unnecessary clashes and to have a clean exit from the online simulation.
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
