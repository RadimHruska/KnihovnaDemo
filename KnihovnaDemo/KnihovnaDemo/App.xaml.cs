using KnihovnaDemo.Views;
using System.Configuration;
using System.Data;
using System.Windows;
using Views;

namespace KnihovnaDemo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected void ApplicationStart(object sender, StartupEventArgs e)
        {
            try
            {
            var loginView = new LoginWindow();
            loginView.Show();
            loginView.IsVisibleChanged += (s, ev) =>
            {
                if (loginView.IsVisible == false && loginView.IsLoaded)
                {
                    var mainView = new AdminView();
                    mainView.Show();
                    loginView.Close();
                }
            };
            }
            catch (Exception)
            {
                //toto taky není úpně správně, ale tak co se dá dělat
              
            }
           
        }
    }

}
