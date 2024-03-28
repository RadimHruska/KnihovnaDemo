using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace KnihovnaDemo.ViewModels
{
    class LoginViewModel : ViewModelBase
    {
        private string _username;
        private string _password;


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
        }
 

        private bool CanLogIn (object obj)
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
            
        }
    }
}
