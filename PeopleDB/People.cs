using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

namespace PeopleDB
{
    public class People
    {
        public ListFile<Person> DB;
        public string ImagesPath = "Images\\";
        public string DBPath = "C:\\PeopleDB\\Database\\";

        public People()
        {
            Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("PeopleDB");
            if (key.GetValue("path") == null)
            {
                key.SetValue("path", "C:\\PeopleDB\\Database\\");
            }
            else
            {
                DBPath = (string)key.GetValue("path");
            }

            Directory.CreateDirectory(DBPath);
            Directory.CreateDirectory(DBPath + ImagesPath);
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
            Directory.CreateDirectory(DBPath + ImagesPath);
            string savePath = DBPath + ImagesPath + DB[getPerson(name)].LastName + "_" + DB[getPerson(name)].FirstName + ".png";

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

            File.Delete(savePath);
            bmpNew.Save(savePath, ImageFormat.Png);

            int personID = getPerson(name);
            if (personID >= 0)
            {
                Person tempPerson = DB[personID];
                tempPerson.PhotoPath = savePath;
                DB[personID] = tempPerson;
            }
        }

        public int getPerson(string name)
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
                foreach (Entry entry in DB[i].Information)
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

        public List<Person> FuzzyFind(string target, int count)
        {
            List<Person> result = new List<Person>();
            List<KeyValuePair<float, Person>> tempResult = new List<KeyValuePair<float, Person>>();
            for (int i = 0; i < DB.Count; i++)
            {
                string fullname = DB[i].FullName();
                float score = 0.0f;
                if (StringSimilarity(target, fullname) > 0.5f)
                { score += StringSimilarity(target, fullname); }

                int hits = 0;
                float tempscore = 0.0f;

                char[] delimiterChars = { ' ', ',', '.', ':', '\t' };
                string[] targetwords = target.Split(delimiterChars);
                foreach (Entry entry in DB[i].Information)
                {
                    string[] entrywords = (entry.Title + " " + entry.Text).Split(delimiterChars);
                    for (int j = 0; j < targetwords.Length; j++)
                    {
                        for (int k = 0; k < entrywords.Length; k++)
                        {
                            float similarity = StringSimilarity(targetwords[j], entrywords[k]);
                            if (similarity > 0.5f)
                            {
                                tempscore += similarity;
                                hits++;
                            }
                        }
                    }
                }
                if (hits > 0)
                {
                    score += tempscore / (float)hits;
                }
                Console.WriteLine(fullname + " = " + score);
                tempResult.Add(new KeyValuePair<float, Person>(score, DB[i]));
            }

            tempResult.Sort(CompareKeys);
            Console.WriteLine("==============================");
            foreach (KeyValuePair<float, Person> pair in tempResult)
            {
                Console.Write(pair.Key + "\t");
                Console.Write(pair.Value.FullName() + "\n");
            }

            for (int i = 0; i < Math.Min(count, tempResult.Count); i++)
            {
                result.Add(tempResult[tempResult.Count - (i + 1)].Value);
            }

            return result;
        }

        private static int CompareKeys(KeyValuePair<float, Person> a, KeyValuePair<float, Person> b)
        {
            return a.Key.CompareTo(b.Key);
        }

        public static float StringSimilarity(string s, string t)
        {
            float result = 0.0f;

            result = (Math.Max(s.Length, t.Length) - LevenshteinDistance(s, t)) / ((float)Math.Max(s.Length, t.Length));
            result *= 2;
            result *= result;
            //Console.Write(result + "\t|\t" + s + "\t|\t" + t + "\n");
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