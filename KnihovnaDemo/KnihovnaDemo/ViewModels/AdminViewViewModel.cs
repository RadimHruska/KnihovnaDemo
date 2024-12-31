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

        public DatabaseFunctions DatabaseFunctions = new DatabaseFunctions();

        public ICommand InsertBookCommand { get; }
        public ICommand InsertUserCommand { get; }
        public ICommand InsertLendCommand { get; }
        public ICommand ReturnBookCommand { get; }
        public ICommand EditUserCommand { get; }
        public ICommand EditBookCommand { get; }

        public AdminViewViewModel()
        {
            LendModels = DatabaseFunctions.SelectLends();
            BookModels = DatabaseFunctions.SelectBooks();
            UserModels = DatabaseFunctions.SelectUsers();
            InsertBookCommand = new ViewModelCommand(InsertBook);
            //InsertUserCommand = new ViewModelCommand(InsertUser);
            InsertLendCommand = new ViewModelCommand(InsertLend);
            ReturnBookCommand = new ViewModelCommand(ReturnBook);
            EditUserCommand = new ViewModelCommand(UpdateUser);
            EditBookCommand = new ViewModelCommand(UpdateBook);
            Lended = Returned = DateTime.Now;
        }

        public User SelectedUser
        {
            get { return _selectedUser; }
            set { _selectedUser = value; OnPropertyChanged(nameof(SelectedUser)); }
        }

        public Book SelectedBook
        {
            get { return _selectedBook; }
            set { _selectedBook = value; OnPropertyChanged(nameof(SelectedBook)); }
        }

        public Book SelectedBookMain
        {
            get { return _selectedBookMain; }
            set { _selectedBookMain = value; OnPropertyChanged(nameof(SelectedBookMain)); }
        }

        public User SelectedUserMain
        {
            get { return _selectedUserMain; }
            set { _selectedUserMain = value; OnPropertyChanged(nameof(SelectedUserMain)); }
        }

        public Lend SelectedLendMain
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
            set { _isAdmin = value; OnPropertyChanged(nameof(IsAdmin)); }
        }

        public string NameOfBook
        {
            get { return _nameOfBook; }
            set { _nameOfBook = value; OnPropertyChanged(nameof(NameOfBook)); }
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

        public ObservableCollection<Lend> LendModels
        {
            get { return _lendModels; }
            set { _lendModels = value; OnPropertyChanged(nameof(LendModels)); }
        }

        public ObservableCollection<User> UserModels
        {
            get { return _userModels; }
            set { _userModels = value; OnPropertyChanged(nameof(UserModels)); }
        }

        public ObservableCollection<Book> BookModels
        {
            get { return _bookModels; }
            set { _bookModels = value; OnPropertyChanged(nameof(BookModels)); }
        }

        public void InsertBook(object obj)
        {
            int id = DatabaseFunctions.InsertBook(NameOfBook, Author, InStock);
            if (id != 0)
            {
                var book = new Book
                {
                    Id = id,
                    Name = NameOfBook,
                    Author = Author,
                    InStock = InStock
                };
                BookModels.Add(book);
            }
        }

        //public void InsertUser(object obj)
        //{
        //    int id = DatabaseFunctions.InsertUser(NameOfUser, IsAdmin == "1", Password);
        //    if (id != 0)
        //    {
        //        var user = new User
        //        {
        //            Id = id,
        //            Name = NameOfUser,
        //            IsAdmin = IsAdmin == "1",
        //            Password = Password
        //        };
        //        UserModels.Add(user);
        //    }
        //}

        public void InsertLend(object obj)
        {
            var count = LendModels.Count(item => item.IdBook == SelectedBook.Id && item.ReturnedDate == null);
            if (count < SelectedBook.InStock)
            {
                int id = DatabaseFunctions.InsertLend(SelectedUser.Id, SelectedBook.Id, Lended);
                if (id != 0)
                {
                    var lend = new Lend
                    {
                        Id = id,
                        IdUser = SelectedUser.Id,
                        IdBook= SelectedBook.Id,
                        LandedDate = Lended,
                        User = SelectedUser,
                        Book = SelectedBook
                    };
                    LendModels.Add(lend);
                }
            }
        }

        public void ReturnBook(object obj)
        {
            var result = MessageBox.Show("Opravdu chcete vrátit knihu?", "Vrátit?", MessageBoxButton.YesNoCancel);
            if (result == MessageBoxResult.Yes)
            {
                DatabaseFunctions.ReturnBook(SelectedLendMain.Id, Returned);
                LendModels.Remove(SelectedLendMain);
                SelectedLendMain.ReturnedDate = Returned;
                LendModels.Add(SelectedLendMain);
                LendModels = new ObservableCollection<Lend>(LendModels.OrderBy(l => l.Id));
            }
            else
            {
                MessageBox.Show("nevráceno");
            }
        }

        public void UpdateUser(object obj)
        {
            DatabaseFunctions.UpdateUser(SelectedUserMain.Id, SelectedUserMain.Name, SelectedUserMain.IsAdmin);
            UserModels.Remove(UserModels.FirstOrDefault(item => item.Id == SelectedUserMain.Id));
            UserModels.Add(SelectedUserMain);
            UserModels = new ObservableCollection<User>(UserModels.OrderBy(l => l.Id));
            foreach (var item in LendModels)
            {
                if (item.IdUser == SelectedUserMain.Id)
                {
                    item.User = SelectedUserMain;
                }
            }
            LendModels = new ObservableCollection<Lend>(LendModels.OrderBy(l => l.Id));
        }

        public void UpdateBook(object obj)
        {
            DatabaseFunctions.UpdateBook(SelectedBookMain.Id, SelectedBookMain.Name, SelectedBookMain.Author, SelectedBookMain.InStock);
            BookModels.Remove(BookModels.FirstOrDefault(item => item.Id == SelectedBookMain.Id));
            BookModels.Add(SelectedBookMain);
            BookModels = new ObservableCollection<Book>(BookModels.OrderBy(l => l.Id));
            foreach (var item in LendModels)
            {
                if (item.IdBook == SelectedBookMain.Id)
                {
                    item.Book = SelectedBookMain;
                }
            }
            LendModels = new ObservableCollection<Lend>(LendModels.OrderBy(l => l.Id));
        }
    }
}
