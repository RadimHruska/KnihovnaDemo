using KnihovnaAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace KnihovnaAPI.Data
{
    public static class DbInitializer
    {
        public static void Initialize(LibraryContext context)
        {
            // Ujistíme se, že databáze existuje a jsou aplikovány migrace
            context.Database.Migrate();

            // Kontrola, zda jsou v DB již nějaká data
            if (context.Users.Any())
            {
                return; // DB již byla nainicializována
            }

            // Vytvoření výchozích uživatelů
            var users = new User[]
            {
                new User { Name = "Admin", IsAdmin = true, Password = "admin" },
                new User { Name = "Radim", IsAdmin = true, Password = "heslo" },
                new User { Name = "Uživatel", IsAdmin = false, Password = "heslo" }
            };

            context.Users.AddRange(users);
            context.SaveChanges();

            // Vytvoření výchozích knih
            var books = new Book[]
            {
                new Book { Name = "Pán prstenů", Author = "J.R.R. Tolkien", InStock = 3 },
                new Book { Name = "Harry Potter", Author = "J.K. Rowling", InStock = 5 },
                new Book { Name = "1984", Author = "George Orwell", InStock = 2 }
            };

            context.Books.AddRange(books);
            context.SaveChanges();

            // Vytvoření výchozích výpůjček
            var lends = new Lend[]
            {
                new Lend { 
                    IdUser = 2, 
                    IdBook = 1, 
                    LandedDate = DateTime.Now.AddDays(-10), 
                    ReturnedDate = null 
                },
                new Lend { 
                    IdUser = 3, 
                    IdBook = 2, 
                    LandedDate = DateTime.Now.AddDays(-15), 
                    ReturnedDate = DateTime.Now.AddDays(-5) 
                }
            };

            context.Lends.AddRange(lends);
            context.SaveChanges();
        }
    }
} 