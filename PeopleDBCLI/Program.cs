using PeopleDB;
using System;

namespace PeopleDBCLI
{
    internal class Program
    {
        private static People DB = new People();
        private static string prompt = ">>";

        private static void Main(string[] args)
        {
            bool exit = false;
            while (!exit)
            {
                Console.Write(prompt);
                string[] inputTokens = tokenize(Console.ReadLine());
                if (inputTokens[0].ToLower() == "add")
                {
                    Add(inputTokens);
                }
                else if (inputTokens[0].ToLower() == "show")
                {
                    if (inputTokens.Length < 2)
                    {
                        showError("Syntax error. Please see \"help\" for a list of commands and how to use them.");
                    }
                    else
                    {
                        ShowPerson(inputTokens);
                    }
                }
                else if (inputTokens[0].ToLower() == "help")
                {
                    showHelp();
                }
                else if (inputTokens[0].ToLower() == "exit")
                {
                    exit = true;
                }
                else
                {
                    showError("Command not recognized. Use \"help\" for a list of commands and how to use them.");
                }
            }
        }

        private static void showHelp()
        {
            Console.Clear();
            Console.WriteLine("PeopleDB Terminal Interface\n");
            Console.WriteLine("add person \tBegins prompt to add a new person to the database.\n");
            Console.WriteLine("add info\t Begins prompt to add information about an existing person.\n");
            Console.WriteLine();
            Console.WriteLine("show <name>\t Displays database entry for specified person.\n");
            Console.WriteLine("show all\t Lists all names in the database.");
        }

        private static bool ShowPerson(string[] inputTokens)
        {
            if (inputTokens[1].ToLower() == "all")
            {
                Console.Clear();
                for (int i = 0; i < DB.DB.Count; i++)
                {
                    Console.WriteLine(DB.DB[i].FullName());
                }
                return true;
            }

            string firstName = inputTokens[1];
            string lastName = "";
            string name = "";

            if (inputTokens.Length > 2)
            {
                lastName = inputTokens[3];
            }

            name = firstName + " " + lastName;

            bool found = DB.CheckPerson(name) || DB.CheckPerson(firstName) || DB.CheckPerson(lastName);

            if (!found)
            {
                showError("Person not found.");
            }
            else
            {
                Person tempPerson = new Person();

                if (DB.CheckPerson(name))
                {
                    tempPerson = DB.FindPerson(name);
                }
                else if (DB.CheckPerson(firstName))
                {
                    tempPerson = DB.FindPerson(firstName);
                }
                else
                {
                    tempPerson = DB.FindPerson(lastName);
                }

                Console.Clear();
                Console.WriteLine("First Name:\t" + tempPerson.FirstName);
                Console.WriteLine("Last Name:\t" + tempPerson.LastName);
                Console.WriteLine("=================================================");

                foreach (Entry entry in tempPerson.Information)
                {
                    Console.WriteLine(entry.Title + ":");
                    Console.WriteLine(entry.Text);
                    Console.WriteLine("-------------------------------------------------");
                }
            }
            return true;
        }

        private static bool Add(string[] inputTokens)
        {
            if (inputTokens[1].ToLower() == "person")
            {
                Console.Write("First Name " + prompt);
                string firstName = Console.ReadLine();
                if (firstName == "")
                {
                    showError("A first name is required.");
                    return false;
                }
                Console.Write("Last Name " + prompt);
                string lastName = Console.ReadLine();

                if (firstName != "" && lastName == "")
                {
                    Console.WriteLine("Adding " + firstName);
                    DB.AddPerson(firstName);
                }
                if (firstName != "" && lastName != "")
                {
                    Console.WriteLine("Adding " + firstName + " " + lastName);
                    DB.AddPerson(firstName, lastName);
                }
            }
            else if (inputTokens[1].ToLower() == "info")
            {
                Console.Write("Name " + prompt);
                string name = Console.ReadLine();
                if (name == "" || !DB.CheckPerson(name))
                {
                    showError("A name matching an existing database entry is required.");
                    return false;
                }

                Console.WriteLine("Adding info for " + DB.FindPerson(name).FullName());

                Console.Write("Title " + prompt);
                string title = Console.ReadLine();
                if (title == "") { title = "Untitled"; }
                Console.Write("Information " + prompt);
                string info = Console.ReadLine();
                if (info == "")
                {
                    showError("Information must be supplied for information entry.");
                    return false;
                }
                DB.AddInformation(name, title, info);
            }

            return true;
        }

        private static void showError(string message)
        {
            Console.WriteLine("There has been an error.");
            if (message != "") { Console.WriteLine("Message: " + message); }
        }

        private static string[] tokenize(string input)
        {
            char[] delimiterChars = { ' ' };
            string[] words = input.Split(delimiterChars);
            return words;
        }
    }
}