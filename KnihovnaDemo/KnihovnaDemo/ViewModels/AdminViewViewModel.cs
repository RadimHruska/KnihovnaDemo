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



        private UserModel _selectedUser;
        private BookModel _selectedBook;
        private BookModel _selectedBookMain;
        private UserModel _selectedUserMain;
        private LendModel _selectedLendMain;
        private string _nameOfBook;
        private string _author;
        private int _inStock;
        private string _nameOfUser;
        private string _password;
        private string _isAdmin;
        private DateTime _lended;
        private DateTime _returned;


        public DatabaseFunctions DatabaseFunctions = new DatabaseFunctions();

        public ICommand InsertBookCommnand {  get;}
        public ICommand InsertUserCommand { get;}
        public ICommand InsertLendCommand { get; }
        public ICommand ReturnBookCommand { get; }
        public ICommand EditUserCommand { get; }
        public ICommand EditBookCommand { get; }
        public AdminViewViewModel()
        {
            LendModels = DatabaseFunctions.SelectLends();
            BookModels = DatabaseFunctions.SelectBook();
            UserModels = DatabaseFunctions.SelectUsers();
            InsertBookCommnand = new ViewModelCommand(InsertBook);
            InsertUserCommand = new ViewModelCommand(InsertUser);
            InsertLendCommand = new ViewModelCommand(InsertLend);
            ReturnBookCommand = new ViewModelCommand(ReturnBook);
            EditUserCommand = new ViewModelCommand(UpdateUser);
            EditBookCommand = new ViewModelCommand(UpdateBook);
            Lended = Returned = DateTime.Now;
            
            
        }

        public UserModel SelectedUser
        {
            get { return _selectedUser; }
            set { _selectedUser = value; OnPropertyChanged(nameof(SelectedUser)); }
        }
        public BookModel SelectedBook
        {
            get { return _selectedBook; }
            set { _selectedBook = value; OnPropertyChanged(nameof(SelectedBook)); }
        }

        public BookModel SelectedBookMain
        {
            get { return _selectedBookMain; }
            set { _selectedBookMain = value; OnPropertyChanged(nameof(SelectedBookMain)); }
        }
        public UserModel SelectedUserMain
        {
            get { return _selectedUserMain; }
            set { _selectedUserMain = value; OnPropertyChanged(nameof(SelectedUserMain)); }
        }
        public LendModel SelectedLendMain
        {
            get { return _selectedLendMain; }
            set { _selectedLendMain = value; OnPropertyChanged(nameof(SelectedLendMain)); }
        }
        public DateTime Lended
        {
            get { return _lended; }
            set { _lended = value; OnPropertyChanged(nameof(Lended)); }
        }
        public DateTime Returned
        {
            get { return _returned; }
            set { _returned = value; OnPropertyChanged(nameof(Returned)); }
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

        public void InsertLend(object obj)
        {
            var count = LendModels.Count(item => item.IdBook == SelectedBook.Id && item.ReturnedDate == null);
            if(count < SelectedBook.InStock) { 
            int id = 0;
            id = DatabaseFunctions.InsertLend(SelectedUser.ID, SelectedBook.Id, Lended);

            if (id != 0)
            {
                var lend = new LendModel()
                { Id = id, IdUser=SelectedUser.ID, IdBook = SelectedBook.Id, NameOfBook = SelectedBook.Name, NameOfUser= SelectedUser.Name, LandedDate = Lended };

                LendModels.Add(lend);
            }
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

        public void ReturnBook(object obj)
        {
            DatabaseFunctions.ReturnBook(SelectedLendMain.Id, Returned);
            LendModels.Remove(SelectedLendMain);
            SelectedLendMain.ReturnedDate = Returned;
            LendModels.Add(SelectedLendMain);
            LendModels = new ObservableCollection<LendModel>( LendModels.OrderBy(l => l.Id));
            
        }
        public void UpdateUser(object obj)
        {
            DatabaseFunctions.UpdateUser(SelectedUserMain.ID, SelectedUserMain.Name, SelectedUserMain.IsAdmin);
            UserModels.Remove(UserModels.FirstOrDefault(item => item.ID == SelectedUserMain.ID));
            UserModels.Add(SelectedUserMain);
            UserModels = new ObservableCollection<UserModel>(UserModels.OrderBy(l => l.ID));
            foreach (var item in LendModels)
            {
                if (item.IdUser == SelectedUserMain.ID)
                {
                  item.NameOfUser = SelectedUserMain.Name;
                }
            }
            LendModels = new ObservableCollection<LendModel>(LendModels.OrderBy(l => l.Id));
        }
        public void UpdateBook(object obj)
        {
            DatabaseFunctions.UpdateBook(SelectedBookMain.Id,SelectedBookMain.Name,SelectedBookMain.Author,SelectedBookMain.InStock);
            BookModels.Remove(BookModels.FirstOrDefault(item => item.Id == SelectedBookMain.Id));
            BookModels.Add(SelectedBookMain);
            BookModels = new ObservableCollection<BookModel>(BookModels.OrderBy(l => l.Id));
            foreach (var item in LendModels)
            {
                if (item.IdBook == SelectedBookMain.Id)
                {
                    item.NameOfBook = SelectedBookMain.Name;
                }
            }
            LendModels = new ObservableCollection<LendModel>(LendModels.OrderBy(l => l.Id));

        }

    }
}
