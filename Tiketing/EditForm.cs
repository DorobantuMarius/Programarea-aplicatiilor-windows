using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tiketing
{
    public partial class EditForm : Form
    {
        private Tiket tiket;

        public EditForm(Tiket tiket)
        {
            InitializeComponent();
            this.tiket = tiket;
        }

        private void EditForm_Load(object sender, EventArgs e)
        {
            tbTiketNr.Text = tiket.tiketNr;
            tbProject.Text = tiket.project.name;
            tbError.Text = tiket.project.error;
            tbProgrammer.Text = tiket.programmer.name;
            tbLanguage.Text = tiket.programmer.language;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            tiket.tiketNr = tbTiketNr.Text;
            tiket.project.name = tbProject.Text;
            tiket.project.error = tbError.Text;
            tiket.programmer.name = tbProgrammer.Text;
            tiket.programmer.language = tbLanguage.Text;
        }
    }
}
