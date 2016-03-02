using PeopleDB;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace PeopleDBGUI
{
    public partial class SearchDialog : Form
    {
        public List<Person> Result = new List<Person>();
        public People DB = new People();

        public SearchDialog()
        {
            InitializeComponent();
        }

        private void buttonGo_Click(object sender, EventArgs e)
        {
            Result = DB.FuzzyFind(textBoxQuery.Text, 5);
            UpdatePeopleGrid();
        }

        public void UpdatePeopleGrid()
        {
            //Wipe the whole grid
            dataGridView1.Rows.Clear();

            //Add a row for every Person in DB
            for (int i = 0; i < Result.Count; i++)
            {
                string[] row = { Result[i].FirstName, Result[i].LastName };
                dataGridView1.Rows.Add(row);
            }
        }
    }
}