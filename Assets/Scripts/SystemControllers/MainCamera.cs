using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SystemControllers
{
    public class MainCamera : NetworkBehaviour
    {
        private readonly float movementSpeed = 0.5f;
        private readonly float flySpeed = 0.1f;
        public GameObject self;
        private int _sceneNo; 
        
        // Instantiation method
        private void Start()
        {
            // Set the camera object to the center of the simulation world.
            self.transform.position = new Vector3(135, 20, 60);
            _sceneNo = SceneManager.GetActiveScene().buildIndex;
            if (!isLocalPlayer && _sceneNo != 2)
            {
                self.gameObject.SetActive(false);
            }
        }

        void Update()
        {
            if (!isLocalPlayer && _sceneNo != 2)
            {
                return;
            }
            KeyControls();
        }

        void KeyControls()
        {
            if (Input.GetKey(KeyCode.W))
            {
                transform.position += new Vector3(0, 0, 1) * movementSpeed;
            }
            if (Input.GetKey(KeyCode.A))
            {
                transform.position += new Vector3(-1, 0, 0) * movementSpeed;
            }
            if (Input.GetKey(KeyCode.S))
            {
                transform.position += new Vector3(0, 0, -1) * movementSpeed;
            }
            if (Input.GetKey(KeyCode.D))
            {
                transform.position += new Vector3(1, 0, 0) * movementSpeed;
            }
            if (Input.GetKey(KeyCode.Space))
            {
                transform.position += new Vector3(0, 1, 0) * flySpeed;
            }
            if (Input.GetKey(KeyCode.LeftShift))
            {
                transform.position += new Vector3(0, -1, 0) * flySpeed;
            }
        }
    }
}
