using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnimalBehaviors
{
    public class Motion
    {
        private float heading;
        private float speed;
        private GameObject animalBody;
        private Rigidbody animal;

        private void Move(Vector3 movementVector)
        {
            this.animal.MovePosition(this.animal.transform.position + movementVector * Time.deltaTime * this.speed);
        }

        public void Idle()
        {
            this.Move(new Vector3(0, 0, 0));
        }

        public Motion(float movementSpeed, GameObject controller, Rigidbody animalInput)
        {
            this.speed = movementSpeed;
            this.animalBody = controller;
            this.animal = animalInput;
        }
    }
}

