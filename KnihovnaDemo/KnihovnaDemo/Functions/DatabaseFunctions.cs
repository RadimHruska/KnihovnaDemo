using KnihovnaDemo.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;

namespace KnihovnaDemo.Functions
{
    class DatabaseFunctions : Base
    {
        public ObservableCollection<LendModel> SelectLends()
        {
            ObservableCollection<LendModel> lends = new ObservableCollection<LendModel>();
            using (var conn = GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("", conn))
                {
                    cmd.CommandText = $"SELECT L.Id as Id, L.Landed as Landed, " +
                        $" L.Returned as Returned, U.Name AS UserName, U.ID AS UserID, " +
                        $" B.Name AS BookName, B.ID AS BookID " +
                        $"FROM Lends L JOIN Uzivatele U ON L.IdUser = U.ID JOIN Books B ON L.IdBook = B.ID;";
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var lend = new LendModel() {
                                Id = int.Parse(reader["Id"].ToString()),
                                IdUser = int.Parse(reader["UserID"].ToString()),
                                IdBook = int.Parse(reader["BookID"].ToString()),
                                NameOfBook = reader["BookName"].ToString(),
                                NameOfUser = reader["UserName"].ToString(),
                                LandedDate = reader.GetDateTime("Landed"),
                                ReturnedDate = !reader.IsDBNull("Returned") ? reader.GetDateTime("Returned") : null,
                            };
                            lends.Add(lend);
                           
                        }
                    }
                    conn.Close();
                }
            }
            return lends;
        }

        public ObservableCollection<UserModel> SelectUsers()
        {
            ObservableCollection<UserModel> users = new ObservableCollection<UserModel>();
            using (var conn = GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("", conn))
                {
                    cmd.CommandText = $"SELECT * FROM Uzivatele";
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var user = new UserModel()
                            {
                            ID = int.Parse(reader[0].ToString()),
                            Name = reader[1].ToString().Trim(),
                            IsAdmin = (reader["IsAdmin"].ToString().ToLower().Trim() == "1") ? true : false,

                        };
                            users.Add(user);

                        }
                    }
                    conn.Close();
                }
            }
            return users;  
        }

        public int InsertBook(string name, string author, int inStock)
        {
            int generatedId;

            using (var conn = GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("", conn))
                {
                    cmd.CommandText = $"INSERT INTO Books (Name, Author, InStock) VALUES (@name, @author, @inStock); SELECT SCOPE_IDENTITY();";
                    conn.Open();
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@author", author);
                    cmd.Parameters.AddWithValue("@inStock", inStock);

                    generatedId = Convert.ToInt32(cmd.ExecuteScalar());
                    conn.Close();
                }
            }

            return generatedId;
        }

        public int InsertUser(string name, string isAdmin, string password)
        {
            int generatedId;

            using (var conn = GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("", conn))
                {
                    cmd.CommandText = $"INSERT INTO Uzivatele (Name, IsAdmin, Password) VALUES (@name, @isAdmin, @password); SELECT SCOPE_IDENTITY();";
                    conn.Open();
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@isAdmin", isAdmin);
                    cmd.Parameters.AddWithValue("@password", password);

                    generatedId = Convert.ToInt32(cmd.ExecuteScalar());
                    conn.Close();
                }
            }

            return generatedId;
        }


        public ObservableCollection<BookModel> SelectBook()
        {
            ObservableCollection<BookModel> books = new ObservableCollection<BookModel>();
            using (var conn = GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("", conn))
                {
                    cmd.CommandText = $"SELECT * FROM Books";
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var book = new BookModel()
                            {
                                Id = int.Parse(reader[0].ToString()),
                                Name = reader[1].ToString().Trim(),
                                Author = reader[2].ToString().Trim(),
                                InStock = int.Parse(reader[3].ToString()),

                            };
                            books.Add(book);

                        }
                    }
                    conn.Close();
                }
            }
            return books;
        }
    }
}
