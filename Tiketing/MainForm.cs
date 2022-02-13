using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tiketing
{
    public partial class MainForm : Form
    {
        private const string ConnectionString = "Data Source=BazaDeDate.db";
        private List<Tiket> tikets;
        public MainForm()
        {
            InitializeComponent();
            tikets = new List<Tiket>();
        }

        private void btnGenerateTiket_Click(object sender, EventArgs e)
        {
            string tiketNr = tbTiketNr.Text;
            string project = tbProject.Text;
            string error = tbError.Text;
            string programmer = tbProgrammer.Text;
            string language = tbLanguage.Text;

            bool generateTiketValid = true;
            if (tiketNr.Trim().Length < 1)
            {
                errorProvider.SetError(tbTiketNr, "Add tiket number!");
                generateTiketValid = false;
            }


            if (project.Trim().Length < 1)
            {
                errorProvider.SetError(tbProject, "Add project name!");
                generateTiketValid = false;
            }
           

            if (error.Trim().Length < 1)
            {
                errorProvider.SetError(tbError, "Add error name!");
                generateTiketValid = false;
            }
           

            if (programmer.Trim().Length < 1)
            {
                errorProvider.SetError(tbProgrammer, "Add programmer name!");
                generateTiketValid = false;
            }
           

            if (language.Trim().Length < 1)
            {
                errorProvider.SetError(tbLanguage, "Add programming language!");
                generateTiketValid = false;
            }
           

            if(!generateTiketValid)
            {
                MessageBox.Show("Fill all gaps!!!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
               
            }

            Tiket tiket = new Tiket(tiketNr, new Project(project, error), new Programmer(programmer, language));

          
                AddTiket(tiket);

                tbTiketNr.Text = string.Empty;//dupa adaugare tiket, goleste textbox
                tbProject.Text = string.Empty;
                tbError.Text = string.Empty;
                tbProgrammer.Text = string.Empty;
                tbLanguage.Text = string.Empty;


                DisplayTikets();
          
        }

        void DisplayTikets()
        {
            lvTikets.Items.Clear();


            foreach(Tiket t in tikets)
            {
                ListViewItem item = new ListViewItem(t.tiketNr);
                item.SubItems.Add(t.project.name);
                item.SubItems.Add(t.project.error);
                item.SubItems.Add(t.programmer.name);
                item.SubItems.Add(t.programmer.language);

                item.Tag = t;

                lvTikets.Items.Add(item);
            }
        }

        public void AddTiket(Tiket tiket)
        {
            var query = "INSERT INTO Tiket(TiketNr, Project, Error, Programmer, Language)" +
                " VALUES (@tiketNr, @project, @error, @programmer, @language); " +
                " SELECT last_insert_rowid()";

            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                SQLiteCommand command = new SQLiteCommand(query, connection);
                command.Parameters.AddWithValue("@tiketNr", tiket.tiketNr);
                command.Parameters.AddWithValue("@project", tiket.project.name);
                command.Parameters.AddWithValue("@error", tiket.project.error);
                command.Parameters.AddWithValue("@programmer", tiket.programmer.name);
                command.Parameters.AddWithValue("@language", tiket.programmer.language);

               
                tiket.id = (long)command.ExecuteScalar();

                tikets.Add(tiket);
            }
        }

        public void LoadTiket()
        {
            var querry = "SELECT * FROM Tiket";

            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                SQLiteCommand command = new SQLiteCommand(querry, connection);
               
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        long id = (long)reader["id"];
                        string tiketNr = (string)reader["TiketNr"];
                        string project = (string)reader["Project"];
                        string error = (string)reader["Error"];
                        string programmer = (string)reader["Programmer"];
                        string language = (string)reader["Language"];

                        Tiket tiket = new Tiket(id, tiketNr, new Programmer(programmer, language), new Project(project,error));

                        tikets.Add(tiket);
                    }
                }
            }

        }

        void DeleteTiket(Tiket tiket)
        {
            var query = "DELETE FROM Tiket WHERE id=@id";

            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                SQLiteCommand command = new SQLiteCommand(query, connection);

                command.Parameters.AddWithValue("@id", tiket.id);
                connection.Open();
                command.ExecuteNonQuery();

                tikets.Remove(tiket);
            }

        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            LoadTiket();
            DisplayTikets();
        }

        private void tbTiketNr_KeyPress(object sender, KeyPressEventArgs e)//pt a introduce doar cifre pt tiketNr
        {
            if (!(char.IsDigit(e.KeyChar) || (char)Keys.Back == e.KeyChar))
                e.Handled = true;
        }

        private void tbTiketNr_Validating(object sender, CancelEventArgs e)
        {
            if (tbTiketNr.Text.Trim().Length < 1)
            {
                errorProvider.SetError(tbTiketNr, "Add tiket number!");
            }
            else
            {
                errorProvider.SetError(tbTiketNr, null);
            }
        }

        private void tbProject_Validating(object sender, CancelEventArgs e)
        {
            if (tbProject.Text.Trim().Length < 1)
            {
                errorProvider.SetError(tbProject, "Add project name!");
            }
            else
            {
                errorProvider.SetError(tbProject, null);
            }
        }

        private void tbError_Validating(object sender, CancelEventArgs e)
        {
            if (tbError.Text.Trim().Length < 1)
            {
                errorProvider.SetError(tbError, "Add error name!");
            }
            else
            {
                errorProvider.SetError(tbError, null);
            }
        }

        private void tbProgrammer_Validating(object sender, CancelEventArgs e)
        {
            if (tbProgrammer.Text.Trim().Length < 1)
            {
                errorProvider.SetError(tbProgrammer, "Add programmer name!");
            }
            else
            {
                errorProvider.SetError(tbProgrammer, null);
            }
        }

        private void tbLanguage_Validating(object sender, CancelEventArgs e)
        {
            if (tbLanguage.Text.Trim().Length < 1)
            {
                errorProvider.SetError(tbLanguage, "Add programming language!");
            }
            else
            {
                errorProvider.SetError(tbLanguage, null);
            }
        }

        private void btnSerializeBinary_Click(object sender, EventArgs e)
        {
            BinaryFormatter binFormatter = new BinaryFormatter();
            using (FileStream s = File.Create("Tikets_Serialized.bin"))
            {
                binFormatter.Serialize(s, tikets);  
            }
        }

        private void btnDeserializeBinary_Click(object sender, EventArgs e)
        {
            BinaryFormatter binFormatter = new BinaryFormatter();
            using (FileStream s = File.OpenRead("Tikets_Serialized.bin"))
            {
                tikets = (List<Tiket>)binFormatter.Deserialize(s);
                DisplayTikets();
            }
        }

        private void btnExport_Click(object sender, EventArgs e)//export date in format cvs
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "CSV | *.csv";//extensia, tipul de fisier
            saveFileDialog.Title = "Export as csv";

            if(saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                using (StreamWriter sw = new StreamWriter(saveFileDialog.FileName))
                {
                    sw.WriteLine("TiketNr, Project, Error, Programmer, Language");

                    foreach (Tiket t in tikets)
                    {
                        sw.WriteLine($"{t.tiketNr},{t.project.name},{t.project.error},{t.programmer.name},{t.programmer.language}");
                    }
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)//buton stergere tiket
        {
            if(lvTikets.SelectedItems.Count == 0)
            {
                MessageBox.Show("Select a tiket!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                if (MessageBox.Show("Are you sure?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    ListViewItem selectedItems = lvTikets.SelectedItems[0];
                    Tiket t = (Tiket)selectedItems.Tag;

                    DeleteTiket(t);

                    DisplayTikets();
                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if(lvTikets.SelectedItems.Count == 0)
            {
                MessageBox.Show("Select a tiket!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                ListViewItem selectedItems = lvTikets.SelectedItems[0];
                Tiket t = (Tiket)selectedItems.Tag;

                EditForm editform = new EditForm(t);
                if(editform.ShowDialog() == DialogResult.OK)
                {
                    var querry = "UPDATE Tiket SET TiketNr = @tiketNr,Project = @project ,Error = @error,Programmer = @programmer,Language = @language WHERE id = @id";
                    using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
                    {
                        connection.Open();
                        using (SQLiteCommand command = new SQLiteCommand(querry, connection))
                        {
                            command.Parameters.AddWithValue("@id", t.id);
                            command.Parameters.AddWithValue("@tiketNr", t.tiketNr);
                            command.Parameters.AddWithValue("@project", t.project.name);
                            command.Parameters.AddWithValue("@error", t.project.error);
                            command.Parameters.AddWithValue("@programmer", t.programmer.name);
                            command.Parameters.AddWithValue("@language", t.programmer.language);
                            command.ExecuteNonQuery();

                        }
                    }


                    DisplayTikets();
                }


            }
        }

        private void printDocument_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Font font = new Font("Times New Roman", 14);
            int currentY = printDocument.DefaultPageSettings.Margins.Top;
            e.Graphics.DrawString("TiketNr - Project - Error - Programmer - ProgrammingLanguage", font, Brushes.Black, 10, currentY);
            currentY += 30;
            while (printIndex < tikets.Count)
            {
                Tiket tiket = tikets[printIndex];
                e.Graphics.DrawString($"{tiket.tiketNr} - {tiket.project.name} - {tiket.project.error} - {tiket.programmer.name} - {tiket.programmer.language}",font, Brushes.Black, 10, currentY);

                currentY += 30;
                if(currentY > e.MarginBounds.Height)//sa se creeze mai multe pagini in caz ca adaug multe tikete
                {
                    e.HasMorePages = true;
                    break;
                }

                printIndex++;
            }
        }

        int printIndex = 0;

        private void printDocument_BeginPrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            printIndex = 0;
        }

        private void btnPrintPreview_Click(object sender, EventArgs e)
        {
            printPreviewDialog.ShowDialog();
        }

        private void btnCopyToClipboard_Click(object sender, EventArgs e)
        {
            if (listBox.SelectedItems.Count == 0)
            {
                MessageBox.Show("Select a name!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                Clipboard.SetText(listBox.SelectedItem.ToString());
            }
        }

        private void tbProgrammer_MouseDown(object sender, MouseEventArgs e)
        {
            tbProgrammer.SelectAll();
            listBox.DoDragDrop(tbProgrammer.Text, DragDropEffects.All);
        }

        private void listBox_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
            listBox.Items.Add(e.Data.GetData(DataFormats.Text));
            tbProgrammer.Clear();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
