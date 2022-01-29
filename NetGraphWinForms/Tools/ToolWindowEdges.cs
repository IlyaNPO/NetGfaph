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
    public partial class ToolWindowEdges : Form
    {
        GraphRedactor graphRedactor = null;
        NetGraph graph = null;
        VoidDelegate action = null;

        public ToolWindowEdges(GraphRedactor _graphRedactor, VoidDelegate _action)
        {
            InitializeComponent();
            this.graphRedactor = _graphRedactor;
            this.graph = NetGraph.GetGraph();
            this.action = _action;
            Form1.ListOfUpdateData.Add(DisplayGraphApexesData);
        }

        private void btnEditEdge_Click(object sender, EventArgs e)
        {
            if (this.cbxApexFrom.Text == string.Empty || this.cbxApexTo.Text == string.Empty)
                return;
            if (this.cbxApexFrom.Text != this.cbxApexTo.Text)
            {
                byte m1 = (byte)nudMetricFrom.Value;
                byte m2 = (byte)nudMetricTo.Value;
                int fromId = Convert.ToInt32(cbxApexFrom.Text) - 1;
                int toId = Convert.ToInt32(cbxApexTo.Text) - 1;
                if (!graph.ContainsEdge(fromId, toId))
                {
                    graph.AddEdge(fromId, toId, m1, m2);
                    action();
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Удалить связь?", "Подтверждение", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                int fromId = Convert.ToInt32(cbxApexFrom.Text) - 1;
                int toId = Convert.ToInt32(cbxApexTo.Text) - 1;
                if (graph.ContainsEdge(fromId, toId))
                {
                    graphRedactor.RemoveConnection(fromId, toId);
                    graphRedactor.RemoveConnection(toId, fromId);
                    this.action();
                }
            }
        }

        private void ShowData()
        {
            if (this.cbxApexFrom.Text == string.Empty || this.cbxApexTo.Text == string.Empty)
                return;
            if (this.cbxApexFrom.Text == this.cbxApexTo.Text)
            {
                // сам на себя
                this.nudMetricFrom.Value = 0;
                this.nudMetricTo.Value = 0;
                return;
            }
            else
            {
                int fromId = Convert.ToInt32(cbxApexFrom.Text) - 1;
                int toId = Convert.ToInt32(cbxApexTo.Text) - 1;
                if (!graph.ContainsEdge(fromId, toId))
                {
                    this.nudMetricFrom.Value = 255;
                    this.nudMetricTo.Value = 255;
                    return;
                }
                Edge edgeFrom = null;
                Edge edgeTo = null;
                try
                {
                    edgeFrom = graphRedactor.GetEdge(fromId, toId);
                    edgeTo = graphRedactor.GetEdge(toId, fromId);
                    byte m1 = edgeFrom.Metric;
                    byte m2 = edgeTo.Metric;
                    this.nudMetricFrom.Value = m1;
                    this.nudMetricTo.Value = m2;
                    this.btnEdit.Enabled = true;
                    this.btnDelete.Enabled = true;
                }
                catch
                {
                    // связей нет
                    this.nudMetricFrom.Value = 255;
                    this.nudMetricTo.Value = 255;
                    return;
                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (this.cbxApexFrom.Text == string.Empty || this.cbxApexTo.Text == string.Empty)
                return;
            if (this.cbxApexFrom.Text != this.cbxApexTo.Text)
            {
                int fromId = Convert.ToInt32(cbxApexFrom.Text) - 1;
                int toId = Convert.ToInt32(cbxApexTo.Text) - 1;
                if (!graph.ContainsEdge(fromId, toId))
                    return;
                Edge edgeFrom = graphRedactor.GetEdge(fromId, toId);
                Edge edgeTo = graphRedactor.GetEdge(toId, fromId);
                byte m1 = (byte)nudMetricFrom.Value;
                byte m2 = (byte)nudMetricTo.Value;
                edgeFrom.SetMetric(m1);
                edgeTo.SetMetric(m2);
                action();
            }
            this.btnEdit.Enabled = false;
        }

        private void DisplayGraphApexesData()
        {
            string val1 = this.cbxApexFrom.SelectedText;
            string val2 = this.cbxApexTo.SelectedText;

            this.cbxApexFrom.Items.Clear();
            this.cbxApexTo.Items.Clear();
            int[] ids = (from apex in graph.ApexList select apex.Index + 1).ToArray<int>();
            if (ids.Length == 0)
                return;
            foreach (int apId in ids)
            {
                this.cbxApexFrom.Items.Add(apId);
                this.cbxApexTo.Items.Add(apId);
            }
            this.cbxApexFrom.SelectedText = val1;
            this.cbxApexTo.SelectedText = val2;
        }

        private void ToolWindowEdges_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Form1.ListOfUpdateData.Contains(DisplayGraphApexesData))
                Form1.ListOfUpdateData.Remove(DisplayGraphApexesData);
        }

        private void cbxApexFrom_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowData();
        }

        private void nudMetricFrom_ValueChanged(object sender, EventArgs e)
        {
            byte v = (byte)(sender as NumericUpDown).Value;
            if (v == 0 || v == 255)
            {
                DialogResult result = MessageBox.Show("Значения выходят за допустимые пределы, вы уверены?", "Внимание!", MessageBoxButtons.YesNo);
                if (result == DialogResult.No)
                {
                    if (v == 0) (sender as NumericUpDown).Value = 1;
                    if (v == 255) (sender as NumericUpDown).Value = 254;
                }
            }
        }

        private void ToolWindowEdges_Shown(object sender, EventArgs e)
        {
            DisplayGraphApexesData();
        }
    }
}
