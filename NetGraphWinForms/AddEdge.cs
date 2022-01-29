using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NetGraphWinForms
{
    public partial class AddEdge : Form
    {
        public byte M_From_To
        {
            get { return (byte)this.numericUpDown1.Value; }
        }
        public byte M_To_From
        {
            get { return (byte)this.numericUpDown2.Value; }
        }

        public AddEdge(int apFromId, int apToId)
        {
            InitializeComponent();
            this.label1.Text = string.Format("M[{0},{1}] = ", apFromId, apToId);
            this.label2.Text = string.Format("M[{0},{1}] = ", apToId, apFromId);
        }

        public AddEdge(Edge edgeFromTo, Edge edgeToFrom)
        {
            InitializeComponent();
            int apexFromId = edgeFromTo.ApexFrom.Index;
            int apexToId = edgeFromTo.ApexTo.Index;

            this.numericUpDown1.Value = edgeFromTo.Metric;
            this.numericUpDown2.Value = edgeToFrom.Metric;

            this.label1.Text = string.Format("M[{0},{1}] = ", apexFromId, apexToId);
            this.label2.Text = string.Format("M[{0},{1}] = ", apexToId, apexFromId);
        }

        private void AddEdge_Shown(object sender, EventArgs e)
        {

        }
    }
}
