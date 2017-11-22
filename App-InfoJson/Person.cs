using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_InfoJson
{
    public class Person
    {

        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Gender { get; set; }
        public DateTimeOffset? Birthday { get; set; }
        public string Address { get; set; }
    }
}
