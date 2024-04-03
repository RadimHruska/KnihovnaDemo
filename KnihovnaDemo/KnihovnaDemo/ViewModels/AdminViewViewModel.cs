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

        public DatabaseFunctions DatabaseFunctions = new DatabaseFunctions();
        public AdminViewViewModel()
        {
            LendModels = DatabaseFunctions.SelectLends();
            BookModels = DatabaseFunctions.SelectBook();
            UserModels = DatabaseFunctions.SelectUsers();
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

    }
}
