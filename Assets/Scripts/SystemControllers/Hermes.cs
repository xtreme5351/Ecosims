using System;
using System.Collections;
using System.Collections.Generic;
using AnimalBehaviors;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace SystemControllers
{
    public class Hermes : MonoBehaviour
    {
        private static bool _sGameIsPaused;
        public GameObject pauseMenuUI;
        public GameObject mainMenu;
        public GameObject animalEditor;

        private GameObject _nameRow;
        private GameObject _ageRow;
        private GameObject _moodRow;
        private GameObject _hungerRow;
        private GameObject _thirstRow;

        private Animal _animal;
        private InputField _nameField;
        private InputField _ageField;
        private InputField _moodField;
        private InputField _hungerField;
        private InputField _thirstField;
        
        public void Start()
        {
            _sGameIsPaused = false;
            // Initialise all the table row elements
            _nameRow = animalEditor.transform.GetChild(1).gameObject;
            _ageRow = animalEditor.transform.GetChild(2).gameObject;
            _moodRow = animalEditor.transform.GetChild(3).gameObject;
            _hungerRow = animalEditor.transform.GetChild(4).gameObject;
            _thirstRow = animalEditor.transform.GetChild(5).gameObject;
        }

        void Update()
        {
            // If the key pressed is the escape key, pause the game
            // This condition is checked every frame by this object
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                // Boolean logic to toggle the main menu on and off based on the game state.
                mainMenu.SetActive(_sGameIsPaused);
                _sGameIsPaused = !_sGameIsPaused;
                pauseMenuUI.SetActive(_sGameIsPaused);
            }

            MouseTarget.OnClick += DrawAttributeTable;
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
            _sGameIsPaused = true;
        }

        // Method that is subscribed to the event delegate
        // Called whenever the user clicks on an object
        private void DrawAttributeTable(object sender, MouseTarget.UserClickEventDispatcher userClickEvent)
        {
            Debug.Log("User clicked: " + userClickEvent.Clicked.name);
            
            // Assign the values of the internal field objects to the input fields in the scene
            _nameField = _nameRow.transform.GetChild(0).GetComponent<InputField>();
            _ageField = _ageRow.transform.GetChild(0).GetComponent<InputField>();
            _moodField = _moodRow.transform.GetChild(0).GetComponent<InputField>();
            _hungerField = _hungerRow.transform.GetChild(0).GetComponent<InputField>();
            _thirstField = _thirstRow.transform.GetChild(0).GetComponent<InputField>();
            // Set a temporary variable to the GameObject's script component
            var obj = userClickEvent.Clicked.gameObject.GetComponent <Animal>();
            _animal = obj;
            // Assign all the relevant script components to their relevant fields.
            _nameField.text = _animal.name;
            _ageField.text = _animal.age.ToString();
            _moodField.text = _animal.mood.ToString();
            _hungerField.text = _animal.statDict["food"].ToString();
            _thirstField.text = _animal.statDict["water"].ToString();
        }

        // Method to update the name from input text
        public void UpdateName()
        {
            _animal.name = _nameField.text;
        }
        
        // Method to update the age from the input text
        public void UpdateAge()
        {
            _animal.age = int.Parse(_ageField.text);
        }
        
        // Method to update the mood from the input text
        public void UpdateMood()
        {
            _animal.mood = float.Parse(_moodField.text);
        }
        
        // Method to update the hunger value from the input text
        public void UpdateHunger()
        {
            _animal.statDict["food"] = float.Parse(_hungerField.text);
        }
        
        // Method to update the hunger value from the input text
        public void UpdateThirst()
        {
            _animal.statDict["water"] = float.Parse(_thirstField.text);
        }
    }
}




