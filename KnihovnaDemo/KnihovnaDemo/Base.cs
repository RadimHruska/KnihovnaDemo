using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnihovnaDemo
{
    public abstract class Base
    {
        private string _baseAddress = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\radim\OneDrive\Dokumenty\KnihovnaDemo\KnihovnaDemo\KnihovnaDemo\Views\Knihovna.mdf;Integrated Security=True";
        public string BaseAddress { get { return _baseAddress; } }

        protected SqlConnection GetConnection()
        {
            return new SqlConnection(BaseAddress);
        }
    }
}
