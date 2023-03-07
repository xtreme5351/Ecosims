using System;
using System.Collections.Generic;
using SQLite4Unity3d;
using SystemControllers.Data;
using UnityEngine;

namespace SystemControllers
{
    public class DBManager
    {
        private readonly string _path;
        private readonly SQLiteConnection _conn;

        public DBManager(string path)
        {
            _path = path;
            _conn = new SQLiteConnection(_path);
            Debug.Log("Connection success");
            GetUserInfo();
        }

        public List<UserLinq> GetUserInfo()
        {
            var interim = _conn.Query<UserLinq>($"SELECT * FROM Users");
            return interim;
        }

        public void InsertNewUser(Tuple<string, string> insert)
        {
            var newRecord = new UserLinq { Username = insert.Item1, Password = insert.Item2};
            _conn.Insert(newRecord);
            Debug.Log($"Successful new user creation {insert.Item1} | {insert.Item2}");
        }
        
        public void UpdateUser(Tuple<string, string> insert)
        {
            var newRecord = new UserLinq { Username = insert.Item1, Password = insert.Item2};
            _conn.Update(newRecord);
            Debug.Log($"Successful user update {insert.Item1} | {insert.Item2}");
        }
    }
}

