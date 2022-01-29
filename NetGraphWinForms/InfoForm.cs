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
    public partial class InfoForm : Form
    {
        public InfoForm(string data)
        {
            InitializeComponent();
            this.richTextBox1.Text = data;
        }

        public InfoForm(string[] data)
        {
            InitializeComponent();
            this.richTextBox1.Clear();
            foreach (var item in data)
                this.richTextBox1.AppendText(item);
        }
    }
}
