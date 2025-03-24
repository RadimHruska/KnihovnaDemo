using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace KnihovnaAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsAdmin { get; set; }
        public string Password { get; set; } = string.Empty;
        
        [JsonIgnore] // Ignorovat v JSON serializaci, aby nebyla cyklická reference
        public ICollection<Lend>? Lends { get; set; }
    }

    public class Book
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public int InStock { get; set; }
        public int PublicationYear { get; set; }
        
        [JsonIgnore] // Ignorovat v JSON serializaci, aby nebyla cyklická reference
        public ICollection<Lend>? Lends { get; set; }
    }

    public class Lend
    {
        public int Id { get; set; }
        public DateTime LandedDate { get; set; }
        public DateTime? ReturnedDate { get; set; }
        public int IdUser { get; set; }
        public User? User { get; set; }
        public int IdBook { get; set; }
        public Book? Book { get; set; }
    }
} 