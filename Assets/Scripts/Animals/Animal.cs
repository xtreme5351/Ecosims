using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using SystemControllers;
using UnityEngine.Serialization;
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
        public float mass;

        private readonly float sleepThreshold = 0.2f;
        private float deathPerTick = 0f;
        private Transform target;

        // Sphere Of Influence
        private Collider[] objectsInArea;
        private float searchRadius = 50f;
        private int experiencedTicks;

        public Dictionary<string, float> statDict;
        private Dictionary<string, GameObject[]> resourceMap;

        // New stat declaration for mating
        public int age;
        private float _lust;
        public GameObject mate;
        public bool canMate;

        private float _normalY; 

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
            mass = 1.0f;
            statDict.Add("food", 0f);
            statDict.Add("water", 0f);
            statDict.Add("exhaustion", 0f);
            statDict.Add("death", 0f);
            resourceMap.Add("food", _food);
            resourceMap.Add("water", new GameObject[] { _water });
            _normalY = transform.position.y;
            age = 0;
            _lust = 0;
            currentEulerAngleVector += new Vector3(0, 0, Random.Range(-360, 360)) * rotationSpeed;
            Chronos.OnTick += Life;
        }
        
        

        private void Life(object sender, Chronos.TickEventDispatcher tickedTime)
        {
            if (self.transform.position.y < 0)
            {
                self.transform.position = new Vector3(0, _normalY, 0);
            }

            if (age > 150 || statDict["death"] >= 100.0f)
            {
                Destroy(self);
            }
            
            experiencedTicks = tickedTime.CurrentTick;
            statDict["food"] += _hungerGainedPerTick;
            statDict["water"] += _thirstGainedPerTick;
            statDict["exhaustion"] += exhaustionGainedPerTick;
            statDict["death"] += 0f;
            age += experiencedTicks;
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
                // Only start reproducing once the animals have experienced 1/3 of their lives
                if (age >= 50)
                {
                    _lust += mood;
                }
                // Check for arbitrary threshold
                // A mood check could be done here, however, due to the previous condition,
                // we already know the animal is in a good mood. 
                if (_lust >= 50)
                {
                    // Set the publicly broad-casted variable to true
                    canMate = true;
                    // Get all potential mates in the nearby collider sphere's radius
                    GetLocalResource(mate);
                    MoveToTarget();
                }
                else
                {
                    // If everything else is fine, then simply idle. 
                    IdleState();
                }
            }
        }
        

        private void OnCollisionEnter(Collision collision)
        {
            if (target != null && collision.gameObject.name == target.name)
            {
                // Special case if the collision is with another object who is of the mate species. 
                if (collision.gameObject.name == mate.name)
                {
                    GetMateDetails(collision);
                }
                else
                {
                    EndDeathDecay();
                    print("Collided with" + collision.gameObject.name);
                    Destroy(collision.gameObject);
                }
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

        // Method to poll the details of a potential mate
        private void GetMateDetails(Collision potentialMate)
        {
            // If the potential mate is not ready, then leave
            if (!potentialMate.gameObject.GetComponent<Animal>().canMate) return;
            // If the mate is ready, spawn a "child" can set the lust to 0, putting reproduction on a cooldown.
            Instantiate(self);
            _lust = 0;
            canMate = false;
        }
    }
}

