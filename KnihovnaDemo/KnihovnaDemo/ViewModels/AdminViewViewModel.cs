using KnihovnaDemo.Functions;
using KnihovnaDemo.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;


namespace KnihovnaDemo.ViewModels
{
    class AdminViewViewModel : ViewModelBase
    {
        private ObservableCollection<LendModel> _lendModels;
        private ObservableCollection<BookModel> _bookModels;
        private ObservableCollection<UserModel> _userModels;

        private string _nameOfBook;
        private string _author;
        private int _inStock;
        private string _nameOfUser;
        private string _password;
        private string _isAdmin;

        public DatabaseFunctions DatabaseFunctions = new DatabaseFunctions();

        public ICommand InsertBookCommnand {  get;}
        public ICommand InsertUserCommand { get;}
        public AdminViewViewModel()
        {
            LendModels = DatabaseFunctions.SelectLends();
            BookModels = DatabaseFunctions.SelectBook();
            UserModels = DatabaseFunctions.SelectUsers();
            InsertBookCommnand = new ViewModelCommand(InsertBook);
            InsertUserCommand = new ViewModelCommand(InsertUser);
        }
        public string NameOfUser
        {
            get { return _nameOfUser; }
            set { _nameOfUser = value; OnPropertyChanged(nameof(NameOfUser)); }
        }
        public string Password
        {
            get { return _password; }
            set { _password = value; OnPropertyChanged(nameof(Password)); }
        }
        public string IsAdmin
        {
            get { return _isAdmin; }
            set { _isAdmin = value; OnPropertyChanged(nameof(NameOfBook)); }
        }
        public string NameOfBook
        {
            get { return _nameOfBook; }
            set { _nameOfBook = value; OnPropertyChanged(nameof(IsAdmin)); }
        }
        public string Author
        {
            get { return _author; }
            set { _author = value; OnPropertyChanged(nameof(Author)); }
        }
        public int InStock
        {
            get { return _inStock; }
            set { _inStock = value; OnPropertyChanged(nameof(InStock)); }
        }

        public ObservableCollection<LendModel> LendModels
        {
            get
            {
                return _lendModels;
            }
            set
            {
                _lendModels = value;
                OnPropertyChanged(nameof(LendModels));
            }
        }

        public ObservableCollection<UserModel> UserModels
        {
            get
            {
                return _userModels;
            }
            set
            {
                _userModels = value;
                OnPropertyChanged(nameof(UserModels));
            }
        }
        public ObservableCollection<BookModel> BookModels
        {
            get
            {
                return _bookModels;
            }
            set
            {
                _bookModels = value;
                OnPropertyChanged(nameof(BookModels));
            }
        }

        public void InsertBook(object obj)
        {
            int id = 0;
            id = DatabaseFunctions.InsertBook(NameOfBook, Author, InStock);

            if(id != 0)
            {
                var book = new BookModel()
                { Id = id, Author = Author, InStock = InStock, Name = NameOfBook};

                BookModels.Add(book);
            }
        }

        public void InsertUser(object obj)
        {
            int id = 0;
            id = DatabaseFunctions.InsertUser(NameOfUser, Password, IsAdmin);

            if (id != 0)
            {
                var user = new UserModel()
                { ID = id, IsAdmin = IsAdmin == "1" ? true : false, Name = NameOfUser };

                UserModels.Add(user);
            }
        }

    }
}
