using PeopleDB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PeopleDBGUI
{
    public partial class SearchDialog : Form
    {
        public List<Person> result = new List<Person>();
        People DB = new People();

        public SearchDialog()
        {
            InitializeComponent();
        }

        private void buttonGo_Click(object sender, EventArgs e)
        {
            result = DB.FuzzyFind(textBoxQuery.Text, 5);
            UpdatePeopleGrid();
        }

        public void UpdatePeopleGrid()
        {
            //Wipe the whole grid
            dataGridView1.Rows.Clear();

            //Add a row for every Person in DB
            for (int i = 0; i < result.Count; i++)
            {
                string[] row = { result[i].FirstName, result[i].LastName };
                dataGridView1.Rows.Add(row);
            }
        }
    }
}
