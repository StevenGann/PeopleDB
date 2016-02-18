using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PeopleDB
{
    public class Person
    {
        public string FirstName = "";
        public string LastName = "";
        public string PhotoPath = "";

        public List<Entry> Information;

        public Person()
        {
            Information = new List<Entry>();
        }

        public Person(string firstName, string lastName)
        {
            Information = new List<Entry>();
            FirstName = firstName;
            LastName = lastName;
        }

        public string FullName()
        {
            return FirstName + " " + LastName;
        }

        public void Add(string title, string text)
        {
            Information.Add(new Entry(title, text));
        }
    }
}
