using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace PeopleDBGUI
{
    public partial class NewPerson : Form
    {
        public List<string> Result = new List<string>();

        public NewPerson()
        {
            InitializeComponent();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            Result = new List<string>();
            if (textBoxFirst.Text != "")
            {
                Result.Add(textBoxFirst.Text);
                if (textBoxLast.Text != "")
                {
                    Result.Add(textBoxLast.Text);
                }
            }
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}