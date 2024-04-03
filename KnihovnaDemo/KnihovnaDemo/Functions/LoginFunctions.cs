using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using KnihovnaDemo.Models;

namespace KnihovnaDemo.Functions
{
    class LoginFunctions : Base
    {
        //celá tahle část by měla být úplně jinak, ale tak na střední to stačí, ne :D
        public bool AuthUser(string name, string password)
        {
            UserModel user = null;
            using (var conn = GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("", conn))
                {
                    cmd.CommandText = $"SELECT * FROM Uzivatele WHERE Name = @name AND Password = @password";
                    cmd.Parameters.AddWithValue("name", name);
                    cmd.Parameters.AddWithValue("password", password);
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            user = UserModel.Instance;
                            UserModel.Instance.ID = int.Parse(reader[0].ToString());
                            UserModel.Instance.Name = reader[1].ToString().Trim();
                            UserModel.Instance.IsAdmin = (reader["IsAdmin"].ToString().ToLower().Trim() == "1") ? true : false;
                        }
                    }
                    conn.Close();
                }
            }
            if (user is null)
                return false;
            else
                return true;
        }
    }
}