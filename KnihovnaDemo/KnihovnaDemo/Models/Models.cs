using System;
using System.Collections.Generic;

namespace KnihovnaDemo.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsAdmin { get; set; }
        public string Password { get; set; }
        public ICollection<Lend> Lends { get; set; }
    }

    public class Book
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public int InStock { get; set; }
        public ICollection<Lend> Lends { get; set; }
    }

    public class Lend
    {
        public int Id { get; set; }
        public DateTime LandedDate { get; set; }
        public DateTime? ReturnedDate { get; set; }
        public int IdUser { get; set; }
        public User User { get; set; }
        public int IdBook { get; set; }
        public Book Book { get; set; }
    }
}
