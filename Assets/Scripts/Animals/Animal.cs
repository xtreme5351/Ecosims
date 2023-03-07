using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using SystemControllers;
using Random = UnityEngine.Random;

namespace AnimalBehaviors
{
    public class Animal : MonoBehaviour
    {
        private GameObject self;
        private float walkingSpeed = 0.9f;
        private float runningSpeed = 200f;
        public float mood;
        private float rotationSpeed = 0.9f;
        private Vector3 currentEulerAngleVector;

        private float idleDetectRadius = 0;
        private Motion motionController;

        public string foodType = "Vegan";
        private GameObject[] food;
        //private float hungerSearchRad = 75;
        private readonly float hungerGainedPerTick = 0.05f;

        private GameObject water;
        private readonly float thirstGainedPerTick = 0.1f;
        //private float waterSearchRad = 200;

        private float exhaustionGainedPerTick;
        private float exhaustionWalking = 0f;
        private float exhaustionRunning = 0f;

        private readonly float sleepThreshold = 0.2f;
        private float deathPerTick = 0f;
        private Transform target;

        // Sphere Of Influence
        private Collider[] objectsInArea;
        private float searchRadius = 50f;
        private int experiencedTicks;

        private Dictionary<string, float> statDict;
        private Dictionary<string, GameObject[]> resourceMap;

        void Start()
        {
            mood = 1.0f;
            self = gameObject;
            motionController = new Motion(walkingSpeed, self, self.GetComponent<Rigidbody>());
            statDict = new Dictionary<string, float>();
            resourceMap = new Dictionary<string, GameObject[]>();
            water = GameObject.Find("Water");
            food = GameObject.FindGameObjectsWithTag(foodType);
            exhaustionGainedPerTick = (float) Math.Round(Mathf.Sqrt(hungerGainedPerTick + thirstGainedPerTick), 4);
            statDict.Add("food", 0f);
            statDict.Add("water", 0f);
            statDict.Add("exhaustion", 0f);
            statDict.Add("death", 0f);
            resourceMap.Add("food", food);
            resourceMap.Add("water", new GameObject[] { water });
            Chronos.OnTick += Life;
        }

        private void Life(object sender, Chronos.TickEventDispatcher tickedTime)
        {
            if (self.transform.position.y < 0)
            {
                self.transform.position = new Vector3(0, 0, 0);
            }
            experiencedTicks = tickedTime.currentTick;
            statDict["food"] += hungerGainedPerTick;
            statDict["water"] += thirstGainedPerTick;
            statDict["exhaustion"] += exhaustionGainedPerTick;
            statDict["death"] += 0;
            Debug.Log(experiencedTicks);
            mood = (300 - (statDict["food"] + statDict["water"] + statDict["exhaustion"])) / 300;
            if (mood <= 0.69f)
            {
                string lowestResourceName = statDict.FirstOrDefault(x => x.Value == statDict.Values.Max()).Key;
                //if (lowestResourceName == "exhaustion")
                //{
                //    Sleep();
                //}
                //else
                //{
                    StartDeathDecay();
                    GetLocalResource(resourceMap[lowestResourceName][0]);
                    MoveToTarget();
                //}
            }
            else
            {
                IdleState();
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (target != null && collision.gameObject.name == target.name)
            {
                EndDeathDecay();
                print("Collided with" + collision.gameObject.name);
            }
        }   

        private void GetLocalResource(GameObject desiredResource = null)
        {
            Physics.OverlapSphereNonAlloc(self.transform.position, searchRadius, objectsInArea);
            foreach (var obj in objectsInArea)
            {
                if (desiredResource != null && obj.name == desiredResource.name)
                {
                    target = obj.transform;
                }
            }
        }

        private void MoveToTarget()
        {
            transform.position = Vector3.Lerp(transform.position, target.position, 0.1f);
        }

        private void IdleState()
        {
            if (experiencedTicks % 2 == 0)
            {
                currentEulerAngleVector += new Vector3(0, 0, Random.Range(-360, 360)) * rotationSpeed;
                self.transform.Rotate(currentEulerAngleVector);
                self.transform.position += self.transform.right * 10f;
            }
        }

        private void Sleep()
        {
            Debug.Log("Sleeping");
        }

        private void StartDeathDecay()
        {
            deathPerTick = 0.2f;
        }

        private void EndDeathDecay()
        {
            deathPerTick = 0f;
        }
    }
}