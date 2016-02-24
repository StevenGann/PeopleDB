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
    public partial class Form1 : Form
    {
        static People DB = new People();

        public Form1()
        {
            InitializeComponent();
            UpdatePeopleGrid();
            UpdatePersonGrid(DB.DB[0]);
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void dataGridView2_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            string firstName = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            string lastName = dataGridView1.CurrentRow.Cells[1].Value.ToString();

            Person tempPerson = new Person(firstName, lastName);

            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                if (!row.IsNewRow)
                {
                    try
                    {
                        string title = row.Cells[0].Value.ToString();
                        string text = row.Cells[1].Value.ToString();
                        tempPerson.Information.Add(new Entry(title, text));
                        
                    }
                    catch { }
                }
            }

            DB.DB[DB.getPerson(firstName + " " + lastName)] = tempPerson;
        }

        //==========================================================================================

        public void UpdatePeopleGrid()
        {
            //Wipe the whole grid
            dataGridView1.Rows.Clear();

            //Add a row for every Person in DB
            for (int i = 0; i < DB.DB.Count; i++)
            {
                string[] row = { DB.DB[i].FirstName, DB.DB[i].LastName };
                dataGridView1.Rows.Add(row);
            }
        }

        public void UpdatePersonGrid(Person person)
        {
            //Wipe the whole grid
            dataGridView2.Rows.Clear();

            for (int i = 0; i < person.Information.Count; i++)
            {
                string[] row = { person.Information[i].Title, person.Information[i].Text };
                dataGridView2.Rows.Add(row);
            }
        }

        private void dataGridView1_CurrentCellChanged(object sender, EventArgs e)
        {
            try
            {
                UpdatePersonGrid(DB.FindPerson(dataGridView1.CurrentRow.Cells[0].Value.ToString() + " " + dataGridView1.CurrentRow.Cells[1].Value.ToString()));
            }
            catch { }
        }
    }
}
