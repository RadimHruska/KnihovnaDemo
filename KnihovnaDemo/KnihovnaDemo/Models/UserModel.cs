using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnihovnaDemo.Models
{
    public class UserModel
    {
        public static UserModel Instance { get; } = new UserModel();

        public UserModel() { }

        public int ID { get; set; }
        public string Name { get; set; }
        public bool IsAdmin { get; set; }
    }
}
