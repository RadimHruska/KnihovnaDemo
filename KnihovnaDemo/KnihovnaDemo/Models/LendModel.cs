using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnihovnaDemo.Models
{
    public class LendModel
    {
        public int Id { get; set; }
        public int IdUser { get; set; }
        public int IdBook { get; set; }
        public string ?NameOfBook { get; set; }
        public string ?NameOfUser { get; set;}
        public DateTime LandedDate { get; set; }
        public DateTime? ReturnedDate { get; set;}

    }
}
