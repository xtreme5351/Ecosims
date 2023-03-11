using System;
using System.Collections.Generic;
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
            var newRecord = new UserLinq { UserID = 0, Username = insert.Item1, Password = insert.Item2};
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
    }
}

