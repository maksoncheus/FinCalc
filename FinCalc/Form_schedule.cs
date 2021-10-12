using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FinCalc
{
    public partial class Form_schedule : Form
    {
        public Form_schedule(DataGridView dgv)
        {
            InitializeComponent();
            this.Controls.Add(dgv);
            dgv.ReadOnly = true;
            dgv.Show();
        }

        private void Form_schedule_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
