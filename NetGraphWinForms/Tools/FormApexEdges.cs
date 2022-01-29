using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace NetGraphWinForms
{
    public partial class ToolWindowInfoApexEdges : Form
    {
        Apex curentApex = null;
        GraphRedactor graphRedactor = null;

        public ToolWindowInfoApexEdges(GraphRedactor graphRedactor,Apex apex)
        {
            InitializeComponent();
            this.curentApex = apex;
            this.graphRedactor = graphRedactor;
            //this.graph = NetGraph.GetGraph();
        }

        private void FormApexEdges_Shown(object sender, EventArgs e)
        {
            this.Text = string.Format("Данные узла {0}", this.curentApex.Index + 1);
            DisplayEdges();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            GraphDrawer.DisplayOnInfoPannel(e.Graphics, this.curentApex);

            int x0 = 100;
            int y0 = 5;
            List<Edge> el = graphRedactor.SelectAllEdges(curentApex);
            if (el.Count == 0)
                return;
            int dy = 0;
            try
            {
                dy = panel1.Height / el.Count;
                if (el.Count > 10)
                    dy = 2 * panel1.Height / el.Count;
            }
            catch (Exception ex) { Log.DisplayMessage(ex); }

            int countInRow = 9;
            int index = 0;
            foreach (var item in el)
            {
                e.Graphics.DrawLine(new Pen(GraphDrawer.GetEdgeColor(item.Metric), 2f), x0, y0, x0 + 20, y0);
                e.Graphics.DrawString(string.Format("{0}->{1}", item.ApexFrom.Index + 1, item.ApexTo.Index + 1),
                    new Font("Microsoft Sans Serif", (float)6), Brushes.Black, x0 + 22, y0);
                y0 += 10;
                index++;
                if (index == countInRow)
                {
                    index = 0;
                    x0 += 50;
                    y0 = 5;
                }
            }
        }

        private void DisplayEdges()
        {
            List<Edge> edges = graphRedactor.SelectAllEdges(curentApex);
            this.listView1.Items.Clear();
            foreach (var edge in edges)
            {
                string direction = string.Format("{0} - {1}", edge.ApexFrom.Index + 1, edge.ApexTo.Index + 1);
                ListViewItem lvItem = new ListViewItem(new string[] { direction, edge.Metric.ToString(), edge.S.ToString("F2") });
                listView1.Items.Add(lvItem);
            }
        }
    }
}
