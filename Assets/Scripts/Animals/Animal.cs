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

        private float _idleDetectRadius = 0;
        private Motion _motionController;

        public string foodType = "Vegan";
        private GameObject[] _food;
        //private float hungerSearchRad = 75;
        private readonly float _hungerGainedPerTick = 0.05f;

        private GameObject _water;
        private readonly float _thirstGainedPerTick = 0.1f;
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

        private int _age;

        void Start()
        {
            mood = 1.0f;
            self = gameObject;
            _motionController = new Motion(walkingSpeed, self, self.GetComponent<Rigidbody>());
            statDict = new Dictionary<string, float>();
            resourceMap = new Dictionary<string, GameObject[]>();
            _water = GameObject.Find("Water");
            _food = GameObject.FindGameObjectsWithTag(foodType);
            exhaustionGainedPerTick = (float) Math.Round(Mathf.Sqrt(_hungerGainedPerTick + _thirstGainedPerTick), 4);
            statDict.Add("food", 0f);
            statDict.Add("water", 0f);
            statDict.Add("exhaustion", 0f);
            statDict.Add("death", 0f);
            resourceMap.Add("food", _food);
            resourceMap.Add("water", new GameObject[] { _water });
            Chronos.OnTick += Life;
            _age = 0;
        }

        private void Life(object sender, Chronos.TickEventDispatcher tickedTime)
        {
            if (self.transform.position.y < 0)
            {
                self.transform.position = new Vector3(0, 0, 0);
            }

            if (_age > 150 || statDict["death"] >= 100.0f)
            {
                Destroy(self);
            }
            
            experiencedTicks = tickedTime.CurrentTick;
            statDict["food"] += _hungerGainedPerTick;
            statDict["water"] += _thirstGainedPerTick;
            statDict["exhaustion"] += exhaustionGainedPerTick;
            statDict["death"] += 0f;
            _age += experiencedTicks;
            // Debug.Log(experiencedTicks);
            // Mood is the average of the three stats
            mood = (300 - (statDict["food"] + statDict["water"] + statDict["exhaustion"])) / 300;
            // Arbitrary threshold
            if (mood <= 0.70f)
            {
                // Get the factor that is causing the low mood
                var lowestResourceName = statDict.FirstOrDefault(x => x.Value == statDict.Values.Max()).Key;
                if (lowestResourceName == "exhaustion")
                {
                    // Sleep if exhausted
                    Sleep(); 
                }
                else
                {
                    // Start dying due to lack of food or water
                    // Resolve the lowest resource first, which is often water
                    StartDeathDecay();
                    GetLocalResource(resourceMap[lowestResourceName][0]);
                    MoveToTarget();
                }
            }
            else
            {
                // If everything else is fine, then simply idle. 
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
            currentEulerAngleVector += new Vector3(0, 0, Random.Range(-360, 360)) * rotationSpeed;
            self.transform.Rotate(currentEulerAngleVector);
            self.transform.position += self.transform.right * 10f;
        }

        // Method called when the animal is exhausted.
        private void Sleep()
        {
            // Since no other movement method is called, the animal just "sleeps" stationary
            statDict["exhaustion"] -= 1.0f;
            // Lower metabolic rate
            statDict["food"] += 0.001f;
            statDict["water"] += 0.001f;
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