using PeopleDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            People DB = new People();

            Person test = DB.FuzzyFind("Bachelor");
            Console.WriteLine(test.FullName());
            Console.ReadLine();
        }
    }
}
