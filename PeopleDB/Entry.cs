﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleDB
{
    public class Entry
    {
        public string Title;
        public string Text;

        public Entry()
        {
            Title = "";
            Text = "";
        }

        public Entry(string title, string text)
        {
            Title = title;
            Text = text;
        }

        public void Append(string text)
        {
            Text += "\n" + text;
        }
    }
}
