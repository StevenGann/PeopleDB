using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Threading;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.IO;

namespace PeopleDB
{
    public class People
    {
        public ListFile<Person> DB;
        public string ImagesPath = "Database\\Images\\";
        public string DBPath = "Database\\";

        public People()
        {
            Directory.CreateDirectory(DBPath);
            Directory.CreateDirectory(ImagesPath);
            DB = new ListFile<Person>(DBPath);
            DB.Load();
        }

        public void AddPerson(string firstName, string lastName)
        {
            if (getPerson(firstName + " " + lastName) == -1)
            {
                DB.Add(new Person(firstName, lastName));
            }
            else
            {
                throw new Exception("This name already exists in the database.");
            }
        }

        public void AddPerson(string firstName)
        {
            AddPerson(firstName, "UNKNOWN");
        }

        public void AddInformation(string name, string title, string text)
        {
            int personID = getPerson(name);
            if (personID >= 0)
            {
                Person tempPerson = DB[personID];
                tempPerson.Add(title, text);
                DB[personID] = tempPerson;
            }
        }

        public void AddPhoto(string name, string path)
        {
            Directory.CreateDirectory(ImagesPath);
            string savePath = ImagesPath + DB[getPerson(name)].LastName + "_" + DB[getPerson(name)].FirstName + ".png";

            Image img = Image.FromFile(path);
            Bitmap bmp = img as Bitmap;

            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, img.Width, img.Height), ImageLockMode.ReadWrite, bmp.PixelFormat);

            int byteCount = bmpData.Stride * bmpData.Height;
            byte[] bytes = new byte[byteCount];

            Marshal.Copy(bmpData.Scan0, bytes, 0, byteCount);
            bmp.UnlockBits(bmpData);

            Bitmap bmpNew = new Bitmap(bmp.Width, bmp.Height);
            BitmapData bmpData1 = bmpNew.LockBits(new Rectangle(new Point(), bmpNew.Size), ImageLockMode.ReadWrite, bmp.PixelFormat);
            Marshal.Copy(bytes, 0, bmpData1.Scan0, bytes.Length);
            bmpNew.UnlockBits(bmpData1);
            bmp.Dispose();

            bmpNew.Save(savePath, ImageFormat.Png);
        }

        private int getPerson(string name)
        {
            int result = -1;

            for (int i = 0; i < DB.Count; i++)
            {
                //Check if their full name matches
                if (name.ToLower() == DB[i].FullName().ToLower())
                {
                    return i;
                }
            }

            for (int i = 0; i < DB.Count; i++)
            {
                //Check if anyone's first name matches
                if (name.ToLower() == DB[i].FirstName.ToLower())
                {
                    return i;
                }
            }

            for (int i = 0; i < DB.Count; i++)
            {
                //Check if anyone's last name matches
                if (name.ToLower() == DB[i].LastName.ToLower())
                {
                    return i;
                }
            }

            for (int i = 0; i < DB.Count; i++)
            {
                //Check if anyone's alias matches. Last resort! This is slow.
                foreach(Entry entry in DB[i].Information)
                {
                    if (entry.Title.ToLower() == "alias" && entry.Text.ToLower() == name.ToLower())
                    {
                        return i;
                    }
                    if (entry.Title.ToLower() == "nickname" && entry.Text.ToLower() == name.ToLower())
                    {
                        return i;
                    }
                    if (entry.Title.ToLower() == "username" && entry.Text.ToLower() == name.ToLower())
                    {
                        return i;
                    }
                }
            }

            return result;
        }

        public Person FindPerson(string name)
        {
            Person result = new Person();
            if (CheckPerson(name))
            {
                result = DB[getPerson(name)];
            }

            return result;
        }

        public bool CheckPerson(string name)
        {
            bool result = false;
            if (getPerson(name) != -1) { result = true; }
            return result;
        }

        public Person FuzzyFind(string target)
        {
            Person result = new Person();
            float bestscore = 0.0f;
            for (int i = 0; i < DB.Count; i++)
            {
                float score = 0.0f;
                if (StringSimilarity(target, DB[i].FullName()) > 0.5f)
                { score += StringSimilarity(target, DB[i].FullName()); }

                foreach(Entry entry in DB[i].Information)
                {
                    if (StringSimilarity(target, entry.Text) > 0.25f) { score += StringSimilarity(target, entry.Text); }
                    if (StringSimilarity(target, entry.Title) > 0.25f) { score += StringSimilarity(target, entry.Title); }
                }

                if (score > bestscore)
                {
                    bestscore = score;
                    result = DB[i];
                }
            }

            return result;
        }

        public static float StringSimilarity(string s, string t)
        {
            float result = 0.0f;

            result = (Math.Max(s.Length, t.Length) - LevenshteinDistance(s, t)) / ((float)Math.Max(s.Length, t.Length));
            Console.Write(result + "\t|\t" + s + "\t|\t" + t + "\n");
            return result;
        }

        public static int LevenshteinDistance(string s, string t)
        {
            if (string.IsNullOrEmpty(s))
            {
                if (string.IsNullOrEmpty(t))
                    return 0;
                return t.Length;
            }

            if (string.IsNullOrEmpty(t))
            {
                return s.Length;
            }

            int n = s.Length;
            int m = t.Length;
            int[,] d = new int[n + 1, m + 1];

            // initialize the top and right of the table to 0, 1, 2, ...
            for (int i = 0; i <= n; d[i, 0] = i++) ;
            for (int j = 1; j <= m; d[0, j] = j++) ;

            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= m; j++)
                {
                    int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;
                    int min1 = d[i - 1, j] + 1;
                    int min2 = d[i, j - 1] + 1;
                    int min3 = d[i - 1, j - 1] + cost;
                    d[i, j] = Math.Min(Math.Min(min1, min2), min3);
                }
            }
            return d[n, m];
        }
    }
}
