using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
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
        
        // Nahrazení LoginFunctions instancí ApiClient
        private readonly ApiClient _apiClient;

        public bool IsVisible
        {
            get => _isVisible;
            set
            {
                _isVisible = value;
                OnPropertyChanged(nameof(IsVisible));
            }
        }
        
        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }
        
        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }
        
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }
        
        public ICommand LoginCommand { get; }
        
        public LoginViewModel()
        {
            // Inicializace API klienta s URL backendu z konfigurace
            _apiClient = new ApiClient(Config.ApiBaseUrl);
            
            LoginCommand = new ViewModelCommand(LogInAsync, CanLogIn);
            
#if DEBUG
            Username = "Radim";
            Password = "heslo";
#endif
        }
        
        private bool CanLogIn(object obj)
        {
            return !string.IsNullOrWhiteSpace(Username) && Password != null;
        }
        
        private async void LogInAsync(object obj)
        {
            try
            {
                var user = await _apiClient.LoginAsync(Username, Password);
                
                if (user != null)
                {
                    // Uložení informací o přihlášeném uživateli (např. do singletonu nebo static property)
                    UserModel.CurrentUser = user;
                    
                    // Pokud je uživatel admin, přepni na admin view
                    if (user.IsAdmin)
                    {
                        IsVisible = false;
                    }
                    else
                    {
                        ErrorMessage = "Nemáte oprávnění pro vstup do administrace";
                        MessageBox.Show("Nemáte oprávnění pro vstup do administrace", "Chyba přihlášení", 
                            MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                else
                {
                    ErrorMessage = "Nesprávné uživatelské jméno nebo heslo";
                    MessageBox.Show("Nesprávné uživatelské jméno nebo heslo", "Chyba přihlášení", 
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Chyba při přihlašování";
                MessageBox.Show($"Chyba při přihlašování: {ex.Message}", "Chyba", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
