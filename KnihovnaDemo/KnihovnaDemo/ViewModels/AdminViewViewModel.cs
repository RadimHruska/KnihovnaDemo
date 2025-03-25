using KnihovnaDemo.Functions;
using KnihovnaDemo.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace KnihovnaDemo.ViewModels
{
    class AdminViewViewModel : ViewModelBase
    {
        private ObservableCollection<Lend> _lendModels;
        private ObservableCollection<Book> _bookModels;
        private ObservableCollection<User> _userModels;

        private User _selectedUser;
        private Book _selectedBook;
        private Book _selectedBookMain;
        private User _selectedUserMain;
        private Lend _selectedLendMain;
        private string _nameOfBook;
        private string _author;
        private int _inStock;
        private string _nameOfUser;
        private string _password;
        private string _isAdmin;
        private DateTime _lended;
        private DateTime _returned;

        private readonly ApiClient _apiClient;

        public ICommand InsertBookCommand { get; }
        public ICommand InsertUserCommand { get; }
        public ICommand InsertLendCommand { get; }
        public ICommand ReturnBookCommand { get; }
        public ICommand EditUserCommand { get; }
        public ICommand EditBookCommand { get; }

        public AdminViewViewModel()
        {
            _apiClient = new ApiClient(Config.ApiBaseUrl);
            
            InitializeDataAsync();
            
            InsertBookCommand = new ViewModelCommand(InsertBook);
            InsertUserCommand = new ViewModelCommand(InsertUser);
            InsertLendCommand = new ViewModelCommand(InsertLend);
            ReturnBookCommand = new ViewModelCommand(ReturnBook);
            EditUserCommand = new ViewModelCommand(UpdateUser);
            EditBookCommand = new ViewModelCommand(UpdateBook);
        }

        private async void InitializeDataAsync()
        {
            try
            {
                LendModels = await _apiClient.GetLendsAsync();
                BookModels = await _apiClient.GetBooksAsync();
                UserModels = await _apiClient.GetUsersAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Chyba při načítání dat: {ex.Message}", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public User SelectedUser
        {
            get => _selectedUser;
            set
            {
                _selectedUser = value;
                OnPropertyChanged(nameof(SelectedUser));
            }
        }

        public Book SelectedBook
        {
            get => _selectedBook;
            set
            {
                _selectedBook = value;
                OnPropertyChanged(nameof(SelectedBook));
            }
        }

        public Book SelectedBookMain
        {
            get => _selectedBookMain;
            set
            {
                _selectedBookMain = value;
                OnPropertyChanged(nameof(SelectedBookMain));
            }
        }

        public User SelectedUserMain
        {
            get => _selectedUserMain;
            set
            {
                _selectedUserMain = value;
                OnPropertyChanged(nameof(SelectedUserMain));
            }
        }

        public Lend SelectedLendMain
        {
            get => _selectedLendMain;
            set
            {
                _selectedLendMain = value;
                OnPropertyChanged(nameof(SelectedLendMain));
            }
        }

        public DateTime Lended
        {
            get => _lended;
            set
            {
                _lended = value;
                OnPropertyChanged(nameof(Lended));
            }
        }

        public DateTime Returned
        {
            get => _returned;
            set
            {
                _returned = value;
                OnPropertyChanged(nameof(Returned));
            }
        }

        public string NameOfUser
        {
            get => _nameOfUser;
            set
            {
                _nameOfUser = value;
                OnPropertyChanged(nameof(NameOfUser));
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

        public string IsAdmin
        {
            get => _isAdmin;
            set
            {
                _isAdmin = value;
                OnPropertyChanged(nameof(IsAdmin));
            }
        }

        public string NameOfBook
        {
            get => _nameOfBook;
            set
            {
                _nameOfBook = value;
                OnPropertyChanged(nameof(NameOfBook));
            }
        }

        public string Author
        {
            get => _author;
            set
            {
                _author = value;
                OnPropertyChanged(nameof(Author));
            }
        }

        public int InStock
        {
            get => _inStock;
            set
            {
                _inStock = value;
                OnPropertyChanged(nameof(InStock));
            }
        }

        public ObservableCollection<Lend> LendModels
        {
            get => _lendModels;
            set
            {
                _lendModels = value;
                OnPropertyChanged(nameof(LendModels));
            }
        }

        public ObservableCollection<User> UserModels
        {
            get => _userModels;
            set
            {
                _userModels = value;
                OnPropertyChanged(nameof(UserModels));
            }
        }

        public ObservableCollection<Book> BookModels
        {
            get => _bookModels;
            set
            {
                _bookModels = value;
                OnPropertyChanged(nameof(BookModels));
            }
        }

        public async void InsertBook(object obj)
        {
            try
            {
                if (string.IsNullOrEmpty(NameOfBook) || string.IsNullOrEmpty(Author))
                {
                    MessageBox.Show("Vyplňte všechna pole", "Upozornění");
                    return;
                }

                await _apiClient.CreateBookAsync(NameOfBook, Author, InStock);
                BookModels = await _apiClient.GetBooksAsync();
                
                NameOfBook = "";
                Author = "";
                InStock = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Chyba při přidávání knihy: {ex.Message}", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public async void InsertUser(object obj)
        {
            try
            {
                if (string.IsNullOrEmpty(NameOfUser) || string.IsNullOrEmpty(Password))
                {
                    MessageBox.Show("Vyplňte všechna pole", "Upozornění");
                    return;
                }

                bool isAdminValue = false;
                if (!string.IsNullOrEmpty(IsAdmin))
                {
                    bool.TryParse(IsAdmin, out isAdminValue);
                }

                await _apiClient.CreateUserAsync(NameOfUser, Password, isAdminValue);
                UserModels = await _apiClient.GetUsersAsync();
                
                NameOfUser = "";
                Password = "";
                IsAdmin = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Chyba při přidávání uživatele: {ex.Message}", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public async void InsertLend(object obj)
        {
            try
            {
                if (SelectedUser == null || SelectedBook == null)
                {
                    MessageBox.Show("Vyberte uživatele a knihu", "Upozornění");
                    return;
                }

                await _apiClient.CreateLendAsync(SelectedUser.Id, SelectedBook.Id, Lended);
                LendModels = await _apiClient.GetLendsAsync();
                BookModels = await _apiClient.GetBooksAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Chyba při vytváření výpůjčky: {ex.Message}", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public async void ReturnBook(object obj)
        {
            try
            {
                if (SelectedLendMain == null)
                {
                    MessageBox.Show("Vyberte výpůjčku pro vrácení", "Upozornění");
                    return;
                }

                await _apiClient.ReturnBookAsync(SelectedLendMain.Id, Returned);
                LendModels = await _apiClient.GetLendsAsync();
                BookModels = await _apiClient.GetBooksAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Chyba při vracení knihy: {ex.Message}", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public async void UpdateUser(object obj)
        {
            try
            {
                if (SelectedUserMain == null)
                {
                    MessageBox.Show("Vyberte uživatele pro úpravu", "Upozornění");
                    return;
                }

                await _apiClient.UpdateUserAsync(SelectedUserMain.Id, SelectedUserMain.Name, SelectedUserMain.IsAdmin);
                UserModels = await _apiClient.GetUsersAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Chyba při aktualizaci uživatele: {ex.Message}", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public async void UpdateBook(object obj)
        {
            try
            {
                if (SelectedBookMain == null)
                {
                    MessageBox.Show("Vyberte knihu pro úpravu", "Upozornění");
                    return;
                }

                await _apiClient.UpdateBookAsync(SelectedBookMain.Id, SelectedBookMain.Name, SelectedBookMain.Author, SelectedBookMain.InStock);
                BookModels = await _apiClient.GetBooksAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Chyba při aktualizaci knihy: {ex.Message}", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
