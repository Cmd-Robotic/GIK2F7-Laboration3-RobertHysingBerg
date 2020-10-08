using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Laboration3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<User> Users = new ObservableCollection<User>();
        public MainWindow()
        {
            InitializeComponent();
            UserList.ItemsSource = Users;
        }
        private bool IsUsernameAvailable(string Username)
        {
            bool IsAvailable = true;
            foreach (User Account in Users)
            {
                if (Account.GetUsername() == Username)
                {
                    IsAvailable = false;
                }
            }
            return IsAvailable;
        }
        private void LoadList_Click(object sender, RoutedEventArgs e)
        {
            string FilePath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\UserList.txt";
            StreamReader SR = new StreamReader(FilePath, Encoding.UTF8);
            string JsonDataString = SR.ReadToEnd();
            SR.Close();
            List<Dictionary<string, string>> DictUsers = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(JsonDataString);
            Users.Clear();
            foreach (Dictionary<string, string> Account in DictUsers)
            {
                Users.Add(new User(Account["Username"], Account["Password"], Account["Email"]));
            }
        }
        private void AddUserButton_Click(object sender, RoutedEventArgs e)
        {
            if (AddPassword.Password == AddPasswordConfirm.Password && AddUsername.Text != "" && AddPassword.Password != "" && AddEmail.Text != "" && IsUsernameAvailable(AddUsername.Text))
            {
                User NewUser = new User(AddUsername.Text, AddPassword.Password, AddEmail.Text);
                Users.Add(NewUser);
                AddUserMessage.Text = "User succesfully added!";
                AddUsername.Text = "";
                AddPassword.Password = "";
                AddPasswordConfirm.Password = "";
                AddEmail.Text = "";
            }
            else
            {
                if (AddUsername.Text == "")
                {
                    AddUserMessage.Text = "You must have a username!";
                    AddPassword.Password = "";
                    AddPasswordConfirm.Password = "";
                }
                else if (AddEmail.Text == "")
                {
                    AddUserMessage.Text = "You must have an e-mail!";
                    AddPassword.Password = "";
                    AddPasswordConfirm.Password = "";
                }
                else if (AddPassword.Password == "")
                {
                    AddUserMessage.Text = "You must have a password!";
                    AddPassword.Password = "";
                    AddPasswordConfirm.Password = "";
                }
                else if (AddPassword.Password != AddPasswordConfirm.Password)
                {
                    AddUserMessage.Text = "The passwords do not match!";
                    AddPassword.Password = "";
                    AddPasswordConfirm.Password = "";
                }
                else
                {
                    AddUserMessage.Text = "That username is already taken!";
                }
            }
        }
        private void SaveList_Click(object sender, RoutedEventArgs e)
        {
            List<Dictionary<string, string>> DictUsers = new List<Dictionary<string, string>>();
            foreach (User Account in Users)
            {
                DictUsers.Add(Account.ToDictionary());
            }
            string JsonDataString = JsonSerializer.Serialize(DictUsers);
            //string FilePath = System.Reflection.Assembly.GetExecutingAssembly().Location;   //Gets where the executing assembly is currently located. note, it's in the bin/debug... dangit
            string FilePath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\UserList.txt";
            StreamWriter SR = new StreamWriter(FilePath, false, Encoding.UTF8);
            SR.Write(JsonDataString);
            SR.Close();
        }

        private void UserList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (UserList.SelectedItem != null)
            {
                User SelectedUser = (User)UserList.SelectedItem;
                UserInfoUsername.Text = SelectedUser.GetUsername();
                UserInfoEmail.Text = SelectedUser.GetEmail();
            }
        }
        private void ChangeUsernameSubmit_Click(object sender, RoutedEventArgs e)
        {
            bool ChangeSuccess = false;
            foreach (User Account in Users)
            {
                if (Account.GetUsername() == ChangeUsernameOldUsername.Text)
                {
                    ChangeSuccess = Account.SetUsername(ChangeUsernameNewUsername.Text, ChangeUsernameOldPassword.Password);
                }
            }
            ChangePropertySelectPanel.Visibility = 0;
            ChangePropertySelectButton.Visibility = 0;
            ChangeUsernamePanel.Visibility = (Visibility)2;
            ChangeUsernameSubmit.Visibility = (Visibility)2;
            ChangeUsernameOldUsername.Text = "";
            ChangeUsernameNewUsername.Text = "";
            ChangeUsernameOldPassword.Password = "";
            if (ChangeSuccess)
            {
                ChangePropertySuccesOrFailureText.Text = "Succesfully changed username!";
            }
            else
            {
                ChangePropertySuccesOrFailureText.Text = "Failed to changed username! Did you enter the username/password correctly?";
            }
        }

        private void ChangePropertySelectButton_Click(object sender, RoutedEventArgs e)
        {
            int SelectedProperty = PropertySelectionList.SelectedIndex;
            if (SelectedProperty == 0)
            {
                ChangePropertySelectPanel.Visibility = (Visibility)2;
                ChangePropertySelectButton.Visibility = (Visibility)2;
                ChangeUsernamePanel.Visibility = 0;
                ChangeUsernameSubmit.Visibility = 0;
            }
            else if (SelectedProperty == 1)
            {
                ChangePropertySelectPanel.Visibility = (Visibility)2;
                ChangePropertySelectButton.Visibility = (Visibility)2;
                ChangePasswordPanel.Visibility = 0;
                ChangePasswordSubmit.Visibility = 0;
            }
            else if (SelectedProperty == 2)
            {
                ChangePropertySelectPanel.Visibility = (Visibility)2;
                ChangePropertySelectButton.Visibility = (Visibility)2;
                ChangeEmailPanel.Visibility = 0;
                ChangeEmailSubmit.Visibility = 0;
            }
            else
            {
                ChangePropertySuccesOrFailureText.Text = "Please select something before pressing select";
            }
        }

        private void NewPasswordSubmit_Click(object sender, RoutedEventArgs e)
        {
            bool ChangeSuccess = false;
            foreach (User Account in Users)
            {
                if (Account.GetUsername() == ChangePasswordPanelUsername.Text)
                {
                    ChangeSuccess = Account.SetPassword(ChangePasswordOldPassword.Password, ChangePasswordOldPasswordConfirm.Password, NewPasswordPanelNewPassword.Password, NewPasswordPanelNewPasswordConfirm.Password);
                }
            }
            ChangePropertySelectPanel.Visibility = 0;
            ChangePropertySelectButton.Visibility = 0;
            NewPasswordPanel.Visibility = (Visibility)2;
            NewPasswordSubmit.Visibility = (Visibility)2;
            NewPasswordPanelNewPassword.Password = "";
            NewPasswordPanelNewPasswordConfirm.Password = "";
            ChangePasswordOldPassword.Password = "";
            ChangePasswordOldPasswordConfirm.Password = "";
            ChangePasswordPanelUsername.Text = "";
            if (ChangeSuccess)
            {
                ChangePropertySuccesOrFailureText.Text = "Succesfully changed Password!";
            }
            else
            {
                ChangePropertySuccesOrFailureText.Text = "Failed to changed password! Did you enter the username/passwords correctly?";
            }
            //reset the texts!!!!
        }

        private void ChangeEmailSubmit_Click(object sender, RoutedEventArgs e)
        {
            bool ChangeSuccess = false;
            foreach (User Account in Users)
            {
                if (Account.GetUsername() == ChangeEmailPanelUsername.Text)
                {
                    ChangeSuccess = Account.SetEmail(ChangeEmailPanelEmail.Text, ChangeEmailPanelPassword.Password);
                }
            }
            ChangePropertySelectPanel.Visibility = 0;
            ChangePropertySelectButton.Visibility = 0;
            ChangeEmailPanel.Visibility = (Visibility)2;
            ChangeEmailSubmit.Visibility = (Visibility)2;
            ChangeEmailPanelEmail.Text = "";
            ChangeEmailPanelPassword.Password = "";
            ChangeEmailPanelUsername.Text = "";
            if (ChangeSuccess)
            {
                ChangePropertySuccesOrFailureText.Text = "Succesfully changed e-mail!";
            }
            else
            {
                ChangePropertySuccesOrFailureText.Text = "Failed to changed username! Did you enter the username/password correctly?";
            }
        }

        private void ChangePasswordSubmit_Click(object sender, RoutedEventArgs e)
        {
            ChangePasswordPanel.Visibility = (Visibility)2;
            ChangePasswordSubmit.Visibility = (Visibility)2;
            NewPasswordPanel.Visibility = 0;
            NewPasswordSubmit.Visibility = 0;
        }
    }
}
