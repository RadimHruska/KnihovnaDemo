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
        public int ID { get; set; }
        public string Name { get; set; }
        public bool IsAdmin { get; set; }

    }
}
