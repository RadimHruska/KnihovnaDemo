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
            var loginView = new LoginWindow();
            loginView.Show();
        }
    }

}
