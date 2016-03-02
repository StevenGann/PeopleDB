using PeopleDB;
using System;
using System.Collections.Generic;

namespace Tester
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            People DB = new People();

            List<Person> result = DB.FuzzyFind("This is a test of some assorted text.", 100);
            Console.WriteLine("======================");
            foreach (Person person in result)
            {
                Console.WriteLine(person.FullName());
            }
            Console.ReadLine();
        }
    }
}