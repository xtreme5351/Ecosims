using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using JetBrains.Annotations;
using SystemControllers.Data;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace SystemControllers
{
    public class Janus : MonoBehaviour
    {
        // Variable initialisation/declaration
        // These include all the variables that need to be used during run-time and in this class.
        
        // The instance of the button and input objects must be assigned.
        public Button submitButton;
        public InputField password;
        public InputField username;
        [CanBeNull] public InputField confirmPassword;
        [CanBeNull] public Text userMessage;

        // This is a tuple for internal use only.
        // Contains an inputted username and hashed password.
        private Tuple<string, string> _toValidate;

        public char state;
        
        // Instance of the database manager class.
        private DBManager _dbManager;
        
        // Input path to database.
        private const string DBPath = "/Users/pc/Code/Unity Projects/Ecosims/Assets/Scripts/SystemControllers/Data/UserData.db";

        private const string BannedPath =
            @"/Users/pc/Code/Unity Projects/Ecosims/Assets/Scripts/SystemControllers/Data/BannedWords.txt";
        void Start()
        {
            // Before the first frame is updated, the login button must subscribe to the delegate of the event of user input.
            // When the button is clicked, the GetLoginDetails method is called.
            switch (state)
            {
                case 'n':
                    submitButton.onClick.AddListener(GetNormalLoginDetails);
                    break;
                case 'c':
                    submitButton.onClick.AddListener(CreateNewUser);
                    break;
                case 'r':
                    submitButton.onClick.AddListener(ForgotUserDetails);
                    break;
            }


            // Database manager initialisation. 
            _dbManager = new DBManager(DBPath);
        }

        private void GetNormalLoginDetails()
        {
            userMessage.text = " ";
            // Create the validation tuple by creating a new Tuple object with the username and hashed pwd.
            // The pwd is hashed here. 
            _toValidate = new Tuple<string, string>(username.text, HashToSHA256(password.text));
            // This is C# syntax for nested if statements.
            // If validation fails, null is returned. If not, the print statement is executed. 
            if (!CheckUser() || !Validate())
            {
                Debug.Log("Auth failed");
                userMessage.text = "Incorrect details";
                return;
            }
            SceneManager.LoadScene(3);
        }

        private void CreateNewUser()
        {
            Debug.Log("Creating new user");
            _toValidate = new Tuple<string, string>(username.text, HashToSHA256(password.text));
            // All we need to check here is if the username contains any banned keywords. If not, it is a valid new entry.
            if (!Validate()) return;
            _dbManager.InsertNewUser(_toValidate);
            userMessage.text = "Successful creation";
        }

        private void ForgotUserDetails()
        {
            userMessage.text = " ";
            Debug.Log("Forgot user");
            // Debug.Log("E "  + password.text + " | " + confirmPassword.text);
            // Check if the confirm password field matches the new password field. If not, reject and return void.
            if (password.text != confirmPassword.text) return;
            _toValidate = new Tuple<string, string>(username.text, HashToSHA256(password.text));
            _dbManager.UpdateUser(_toValidate);
            Debug.Log("Updated user");
            userMessage.text = "Updated user";
        }
        private static string HashToSHA256(string rawData)
        {
            // This is the hashing method, using the SHA256 object from System.Security.Cryptography.
            using var sha256Hash = SHA256.Create();
            // In C#, sometimes it is more efficient to use the var variable type instead of explicit declaration.
            var bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));  
  
            // Convert byte array to a string   
            StringBuilder builder = new StringBuilder(); 
            // Convert to each byte to a character and construct a hashed string.
            foreach (var i in bytes) 
            {
                builder.Append(i.ToString("x2"));  
            }  
            return builder.ToString();
        }

        private bool Validate()
        {
            // This opens the BannedWords.txt and scans the input username for any of these prohibited words.
            // Assumes the validation is true until the program finds a prohibited word, in which case False is returned.
            // False indicates that validation has failed.
            string[] lines = System.IO.File.ReadAllLines(BannedPath);
            foreach (var line in lines)
            {
                if (_toValidate.Item1.Contains(line))
                {
                    return false;
                }
            }
            // return from string line in lines where username.text.Contains(line) select true;
            return true;
        }

        private bool CheckUser()
        {
            var userData = _dbManager.GetUserInfo();
            Debug.Log("Checking user");
            // Get all the records from the database that match the username and password.
            var results = userData.Any(obj => obj.Username == _toValidate.Item1 && obj.Password == _toValidate.Item2);
            return results;
        }
    }
}
