using PeopleDB;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace PeopleDBGUI
{
    public partial class Form1 : Form
    {
        private static People DB = new People();

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

            textBoxImagePath.Text = person.PhotoPath;
            textBoxFirstName.Text = person.FirstName;
            textBoxLastName.Text = person.LastName;

            if (person.PhotoPath == "")
            {
                pictureBox1.Image = PeopleDBGUI.Properties.Resources.default_image;
            }
            else
            {
                Person tempPerson = DB.FindPerson(dataGridView1.CurrentRow.Cells[0].Value.ToString() + " " + dataGridView1.CurrentRow.Cells[1].Value.ToString());
                try
                {
                    Image img;
                    using (Bitmap bmpTemp = new Bitmap(DB.DBPath + DB.ImagesPath + tempPerson.PhotoPath))
                    {
                        img = new Bitmap(bmpTemp);
                    }
                    pictureBox1.Image = img;
                }
                catch
                {
                    pictureBox1.Image = PeopleDBGUI.Properties.Resources.default_image;
                }
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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void newPersonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (NewPerson newperson = new NewPerson())
            {
                newperson.ShowDialog();
                List<string> result = newperson.Result;

                if (result.Count == 2)
                {
                    DB.AddPerson(result[0], result[1]);
                    UpdatePeopleGrid();
                }
                else if (result.Count == 1)
                {
                    DB.AddPerson(result[0]);
                    UpdatePeopleGrid();
                }
            }
        }

        private void buttonAddImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            // Set filter options and filter index.
            openFileDialog1.Filter = "PNG|*.png|JPG|*.jpg|All Files|*.*";
            openFileDialog1.FilterIndex = 3;

            // Process input if the user clicked OK.
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // Open the selected file to read.
                string filePath = openFileDialog1.FileName;
                string firstName = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                string lastName = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                DB.AddPhoto(firstName + " " + lastName, filePath);
                UpdatePersonGrid(DB.FindPerson(dataGridView1.CurrentRow.Cells[0].Value.ToString() + " " + dataGridView1.CurrentRow.Cells[1].Value.ToString()));
            }
        }

        private void fuzzySearchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (SearchDialog searchdialog = new SearchDialog())
            {
                searchdialog.ShowDialog();
            }
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            int personid = DB.getPerson(dataGridView1.CurrentRow.Cells[0].Value.ToString() + " " + dataGridView1.CurrentRow.Cells[1].Value.ToString());
            Person tempPerson = DB.FindPerson(dataGridView1.CurrentRow.Cells[0].Value.ToString() + " " + dataGridView1.CurrentRow.Cells[1].Value.ToString());
            tempPerson.FirstName = textBoxFirstName.Text;
            tempPerson.LastName = textBoxLastName.Text;
            DB.DB[personid] = tempPerson;
            UpdatePeopleGrid();
            UpdatePersonGrid(tempPerson);
        }

        private void changeDBLocationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();
            folderBrowserDialog1.Description = "Select the directory to store your private database.";
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                string folderName = folderBrowserDialog1.SelectedPath;
                DB.Move(folderName);
            }
        }
    }
}