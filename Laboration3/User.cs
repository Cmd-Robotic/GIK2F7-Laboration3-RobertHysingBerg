using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Laboration3
{
    class User : IUser, INotifyPropertyChanged
    {
        private string Username;
        private string Password;
        private string Email;

        public event PropertyChangedEventHandler PropertyChanged;

        public User(string Username, string Password, string Email)
        {
            this.Username = Username;
            this.Password = Password;
            this.Email = Email;
        }
        public User()
        {

        }
        public override string ToString()
        {
            return Username;
        }
        public string GetEmail()
        {
            return Email;
        }
        public bool SetEmail(string Email, string Password)
        {
            if (this.Password == Password)
            {
                this.Email = Email;
                OnPropertyChanged("Email");
                return true;
            }
            return false;
        }
        public string GetUsername()
        {
            return Username;
        }
        public bool SetUsername(string Username, string Password)
        {
            if (this.Password == Password)
            {
                this.Username = Username;
                OnPropertyChanged("Username");
                return true;
            }
            return false;
        }
        public string GetPassword()
        {
            return Password;
        }
        public bool SetPassword(string Password, string PasswordConfirm, string NewPassword, string NewPasswordConfirm)
        {
            if (this.Password == Password && Password == PasswordConfirm && NewPassword == NewPasswordConfirm)
            {
                this.Password = NewPassword;
                OnPropertyChanged("Password");
                return true;
            }
            return false;
        }
        private void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public Dictionary<string, string> ToDictionary()
        {
            Dictionary<string, string> Dict = new Dictionary<string, string>();
            Dict.Add("Username", Username);
            Dict.Add("Password", Password);
            Dict.Add("Email", Email);
            return Dict;
        }
    }
}
