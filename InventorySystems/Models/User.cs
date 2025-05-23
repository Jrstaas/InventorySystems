﻿using SQLite;
using System;
using InventorySystems.Models;


namespace InventorySystems.Models
{
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int UserID { get; set; }

        public string Username { get; set; }

        public string PasswordHash { get; set; }

        public string Email { get; set; }

        public string Role { get; set; }

        public DateTime LastLogin { get; set; }  // Stored as a string in ISO format for now

        public User() { }

        public User(string username, string passwordHash, string email, string role, DateTime lastLogin)
        {
            Username = username;
            PasswordHash = passwordHash;
            Email = email;
            Role = role;
            LastLogin = lastLogin;
        }
    }
}
