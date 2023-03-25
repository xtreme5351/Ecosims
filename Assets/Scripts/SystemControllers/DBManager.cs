using System;
using System.Collections.Generic;
using System.Linq;
using SQLite4Unity3d;
using SystemControllers.Data;
using Unity.VisualScripting;
using UnityEngine;

namespace SystemControllers
{
    public class DBManager
    {
        // Attribute initialisation 
        // This is just to reference and manipulate the objects
        private readonly string _path;
        private readonly SQLiteConnection _conn;
        
        // Constructor to initialise and start the database connection
        public DBManager(string path)
        {
            _path = path;
            _conn = new SQLiteConnection(_path);
            Debug.Log("Connection success");
            GetUserInfo();
        }
        
        // Getter method from the database. Queries all the rows from the Users Table
        public List<UserLinq> GetUserInfo()
        {
            var interim = _conn.Query<UserLinq>($"SELECT * FROM Users");
            return interim;
        }
        
        // Setter method for the database, this method is used to insert the data into the db
        // This involves the creation of a new LINQ object for insertion.
        public void InsertNewUser(Tuple<string, string> insert)
        {
            var newRecord = new UserLinq { Username = insert.Item1, Password = insert.Item2};
            _conn.Insert(newRecord);
            Debug.Log($"Successful new user creation {insert.Item1} | {insert.Item2}");
        }
        
        // Update method for the database. This method also involves the creation of a LINQ object for updating the
        // user credentials. 
        public void UpdateUser(Tuple<string, string> insert)
        {
            var newRecord = new UserLinq { Username = insert.Item1, Password = insert.Item2};
            _conn.Update(newRecord);
            Debug.Log($"Successful user update {insert.Item1} | {insert.Item2}");
        }

        // Setter method for adding a saved session to the SaveSession table
        public void AddSaveSession(int userID, float duration)
        {
            var newRecord = new SaveLinq {UserID = userID, Last = Time.time.ToString(), Duration = duration};
            _conn.Insert(newRecord);
            Debug.Log("New save session created");
        }
        
        // Update method for updating an already existing save session with new information
        public void UpdateSaveSession(int userID, float duration)
        {
            var newRecord = new SaveLinq {UserID = userID, Last = DateTime.Now.ToString(), Duration = duration};
            _conn.Update(newRecord);
            Debug.Log("Save session updated");
        }
        
        // Setter method for adding new session data to the SessionData table
        public void AddSessionData(int sessionID, int internalTime, string pwd, string path)
        {
            var newRecord = new SessionLinq { SessionID = sessionID, InternalTime = internalTime, NumUsers = 1, SessionPwd = pwd, SavePath = path};
            _conn.Insert(newRecord);
            Debug.Log("New session data added");
        }
        
        // Update method for updating pre-existing session data with new information
        public void UpdateSessionData(int sessionID, int internalTime, string pwd, string path)
        {
            var newRecord = new SessionLinq { SessionID = sessionID, InternalTime = internalTime, NumUsers = 1, SessionPwd = pwd, SavePath = path};
            _conn.Update(newRecord);
            Debug.Log("Session data updated");
        }
    }
}

