using KnihovnaDemo.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace KnihovnaDemo.Functions
{
    public class DatabaseFunctions
    {
        public ObservableCollection<Lend> SelectLends()
        {
            using (var context = new LibraryContext())
            {
                var lends = context.Lends
                    .Include(l => l.User)
                    .Include(l => l.Book)
                    .Select(l => new Lend
                    {
                        Id = l.Id,
                        IdUser = l.User.Id,
                        IdBook = l.Book.Id,
                        LandedDate = l.LandedDate,
                        ReturnedDate = l.ReturnedDate
                    })
                    .ToList();

                return new ObservableCollection<Lend>(lends);
            }
        }

        public ObservableCollection<User> SelectUsers()
        {
            using (var context = new LibraryContext())
            {
                var users = context.Users.ToList();
                return new ObservableCollection<User>(users);
            }
        }

        public int InsertBook(string name, string author, int inStock)
        {
            using (var context = new LibraryContext())
            {
                var book = new Book
                {
                    Name = name,
                    Author = author,
                    InStock = inStock
                };

                context.Books.Add(book);
                context.SaveChanges();

                return book.Id;
            }
        }

        public int InsertLend(int idUser, int idBook, DateTime lendedTime)
        {
            using (var context = new LibraryContext())
            {
                var lend = new Lend
                {
                    IdUser = idUser,
                    IdBook = idBook,
                    LandedDate = lendedTime
                };

                context.Lends.Add(lend);
                context.SaveChanges();

                return lend.Id;
            }
        }

        public int InsertUser(string name, bool isAdmin)
        {
            using (var context = new LibraryContext())
            {
                var user = new User
                {
                    Name = name,
                    IsAdmin = isAdmin
                };

                context.Users.Add(user);
                context.SaveChanges();

                return user.Id;
            }
        }

        public void ReturnBook(int id, DateTime returned)
        {
            using (var context = new LibraryContext())
            {
                var lend = context.Lends.Find(id);
                if (lend != null)
                {
                    lend.ReturnedDate = returned;
                    context.SaveChanges();
                }
            }
        }

        public void UpdateUser(int id, string name, bool isAdmin)
        {
            using (var context = new LibraryContext())
            {
                var user = context.Users.Find(id);
                if (user != null)
                {
                    user.Name = name;
                    user.IsAdmin = isAdmin;
                    context.SaveChanges();
                }
            }
        }

        public void UpdateBook(int id, string name, string author, int inStock)
        {
            using (var context = new LibraryContext())
            {
                var book = context.Books.Find(id);
                if (book != null)
                {
                    book.Name = name;
                    book.Author = author;
                    book.InStock = inStock;
                    context.SaveChanges();
                }
            }
        }

        public ObservableCollection<Book> SelectBooks()
        {
            using (var context = new LibraryContext())
            {
                var books = context.Books.ToList();
                return new ObservableCollection<Book>(books);
            }
        }
    }
}
