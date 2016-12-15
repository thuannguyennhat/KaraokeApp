using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace KaraokeApp
{
    public class User
    {
        public string Username
        {
            get;
            set;
        }
        public string Password
        {
            get;
            set;
        }

        public User()
        {
        }

        public User(string username, string password)
        {
            this.Username = username;
            this.Password = password;
        }
    }
}