using KnihovnaDemo.Models;
using KnihovnaDemo.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace KnihovnaDemo.Views
{
    /// <summary>
    /// Interaction logic for AdminView.xaml
    /// </summary>
    public partial class AdminView : Window
    {
        public AdminView()
        {
            InitializeComponent();
        }

        private void ListView_SelectionChanged_Users(object sender, SelectionChangedEventArgs e)
        {
            var viewModel = (AdminViewViewModel)DataContext;
            var item = e.AddedItems[0] as User;

            viewModel.SelectedUser = item;

       

        }

        private void ListView_SelectionChanged_Books(object sender, SelectionChangedEventArgs e)
        {
            try
            {
            var viewModel = (AdminViewViewModel)DataContext;
            var item = e.AddedItems[0] as Book;

            viewModel.SelectedBook = item;
            }
            catch (Exception)
            {

               // throw;
            }

        }

        private void ListView_SelectionChanged_lendsMain(object sender, SelectionChangedEventArgs e)
        {
            try
            {
            var viewModel = (AdminViewViewModel)DataContext;
            var item = e.AddedItems[0] as Lend;

            viewModel.SelectedLendMain = item;
            }
            catch (Exception)
            {

                //throw;
            }

        }

        private void ListView_SelectionChanged_BooksMain(object sender, SelectionChangedEventArgs e)
        {
            try
            {
            var viewModel = (AdminViewViewModel)DataContext;
            var item = e.AddedItems[0] as Book;

            viewModel.SelectedBookMain = item;
            }
            catch (Exception)
            {

                //throw;
            }

        }
        private void ListView_SelectionChanged_UsersMain(object sender, SelectionChangedEventArgs e)
        {
            try
            {
            var viewModel = (AdminViewViewModel)DataContext;
            var item = e.AddedItems[0] as User;

            viewModel.SelectedUserMain = item;
            }
            catch (Exception)
            {

                //throw;
            }


        }
    }
}
