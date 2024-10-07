using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using KnihovnaDemo.Functions;
using KnihovnaDemo.Models;

namespace KnihovnaDemo.ViewModels
{
    class LoginViewModel : ViewModelBase
    {
        private string _username;
        private string _password;
        private string _errorMessage;
        private bool _isVisible = true;
        public LoginFunctions LoginFunctions = new LoginFunctions();


        public bool IsVisible
        {
            get
            {
                return _isVisible;
            }
            set
            {
                _isVisible = value;
                OnPropertyChanged(nameof(IsVisible));
            }
        }
        public string ErrorMessage
        {
            get
            {
                return _errorMessage;
            }
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }
        public string Username
        {
            get
            {
                return _username;
            }
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }
        public string Password
        {
            get {
                return _password;
            }
            set {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }
        public ICommand LoginCommad { get; }
        public LoginViewModel()
        {
            LoginCommad = new ViewModelCommand(LogIn, CanLogIn);
#if DEBUG
            Username = "Radim";
            Password = "heslo";
#endif
        }
        private bool CanLogIn(object obj)
        {
            if (string.IsNullOrWhiteSpace(Username) || Password == null)
            {
                return false;
            }
            else
                return true;
        }
        private void LogIn(object obj)
        {
            var isUserValid = LoginFunctions.AuthUser(Username, Password);

            if (isUserValid && UserModel.Instance.IsAdmin == true)
            {
                IsVisible = false;
            }
            else
            {
                ErrorMessage = "Něco se nepovedlo";
                MessageBox.Show("něco se nepovedlo");
            }
               



        }
    }
}
