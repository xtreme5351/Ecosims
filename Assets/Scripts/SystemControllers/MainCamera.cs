using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SystemControllers
{
    public class MainCamera : MonoBehaviour
    {
        private readonly float movementSpeed = 0.5f;
        private readonly float flySpeed = 0.1f;
        //private void Start()
        //{
        //    cam = GetComponent<Camera>();
        //}

        void Update()
        {
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
