using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ScrolledPanel;

namespace NetGraphWinForms
{
    public partial class Form1 : Form
    {
        MyPanel myPanel = null; 
        NetGraph MyGraph = null;
        GraphDrawer graphDrawer = null;
        GraphRedactor graphRedactor = null;
        PathBuilder pathBuilder = null;

        private Point tmpBeginDragPopint = new Point();
        private Dictionary<int, string> algoritmSelectionList = new Dictionary<int, string>();
        public static List<VoidDelegate> ListOfUpdateData = new List<VoidDelegate>();

        public Form1()
        {
            InitializeComponent();
            InitialMyPannel();
            MyGraph = NetGraph.GetGraph();
            graphDrawer = new GraphDrawer();
            graphRedactor = new GraphRedactor();
            pathBuilder = new PathBuilder();
            SetComboBoxesStartValues();
        }

        /// <summary> Заполнение комбобоксов
        /// </summary>
        private void SetComboBoxesStartValues()
        {
            this.algoritmSelectionList.Clear();
            this.algoritmSelectionList.Add(0, "Дейкстры");
            this.algoritmSelectionList.Add(1, "Белмана-Форда");
            this.algoritmSelectionList.Add(2, "Флойда-Уоршела");

            tscbxAlgorithmSelector.ComboBox.DataSource = new BindingSource(algoritmSelectionList, null);
            tscbxAlgorithmSelector.ComboBox.DisplayMember = "Value";
            tscbxAlgorithmSelector.ComboBox.ValueMember = "Key";

            tscbxAlgorithmSelector.ComboBox.SelectedIndex = 0;
        }

        #region My Panel
        private void InitialMyPannel()
        {
            try
            {
                //myPanel = new MyPanel(this.splitContainer1.Panel2);
                myPanel = new MyPanel(this.toolStripContainer1.ContentPanel);
                myPanel.Dock = DockStyle.Fill;
                myPanel.hScrollBar.Visible = false;
                myPanel.vScrollBar.Visible = false;
                this.SetPannelSize();
                myPanel.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                myPanel.BackColor = Color.White;
                myPanel.MouseDown += new MouseEventHandler(myPannel_MouseDown);
                myPanel.MouseUp += new MouseEventHandler(myPanel_MouseUp);
                myPanel.MouseMove += new MouseEventHandler(myPanel_MouseMove);
                myPanel.Paint += new PaintEventHandler(myPannel_Paint);
                myPanel.MouseWheel += new MouseEventHandler(myPanel_MouseWheel);
                //(myPanel as Control).KeyDown += new KeyEventHandler(Form1_KeyDown);
                myPanel.Invalidate();
            }
            catch (Exception ex) { Log.DisplayMessage(ex); }
        }

        void myPanel_MouseWheel(object sender, MouseEventArgs e)
        {
            if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
            {
                if (e.Delta < 0)
                    this.btnZoomMinus_Click(null, null);
                if (e.Delta > 0)
                    this.btnZoomPlus_Click(null, null);
            }
            else if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
            {
                this.graphDrawer.X0 += e.Delta / 5;
            }
            else this.graphDrawer.Y0 += e.Delta / 5;
            this.myPanel.Invalidate(); 
        }

        private void SetPannelSize()
        {
            return;
            //int apexCount = 4;
            //if (this.MyGraph != null && this.MyGraph.ApexCount > 4)
            //    apexCount = this.MyGraph.ApexCount;
            //int pixelsIn1km = 100;
            //double L = Math.Sqrt(4*apexCount);
            //myPanel.Width = (int)(L * pixelsIn1km);
            //myPanel.Height = (int)(L * pixelsIn1km);
        }

        void myPanel_MouseMove(object sender, MouseEventArgs e)
        {
            Point point = myPanel.PointToClient(Cursor.Position);
            foreach (var apex in MyGraph.ApexList)
            {
                if (apex.IsDragable)
                {
                    apex.X = graphDrawer.CorrectorCoordinateX(point.X + this.tmpBeginDragPopint.X);
                    apex.Y = graphDrawer.CorrectorCoordinateX(point.Y + this.tmpBeginDragPopint.Y);
                }
            }
            myPanel.Invalidate();
        }

        void myPanel_MouseUp(object sender, MouseEventArgs e)
        {
            foreach (Apex apex in MyGraph.ApexList)
            {
                apex.IsDragable = false;
            }
        }

        private void myPannel_Paint(object sender, PaintEventArgs e)
        {
            graphDrawer.Draw(e.Graphics, MyGraph);
        }

        void myPannel_MouseDown(object sender, MouseEventArgs e)
        {
            this.graphRedactor.CurrentApex = null;
            foreach (Apex apex in MyGraph.ApexList)
                apex.IsFocused = false;
            foreach (Edge edges in MyGraph.EdgesList)
                edges.IsFocused = false;

            for (int i = MyGraph.ApexCount - 1; i >= 0 ; i--) // выбор вершины
            {
                if (graphDrawer.GetApexArea(MyGraph.ApexList[i]).Contains(e.Location))
                {
                    Point point = myPanel.PointToClient(Cursor.Position);
                    MyGraph.ApexList[i].IsFocused = true;
                    MyGraph.ApexList[i].IsDragable = true;
                    tmpBeginDragPopint = new Point(MyGraph.ApexList[i].X - point.X, MyGraph.ApexList[i].Y - point.Y);
                    this.graphRedactor.CurrentApex = MyGraph.ApexList[i];
                    this.toolStripStatusLabelInfo.Text = this.MyGraph.GetDescription() + " " + this.graphRedactor.CurrentApex.GetDescription();
                    
                    if (e.Button == MouseButtons.Right)
                        this.contextMenuSelectApex.Show(Cursor.Position);
                    break;
                }
            }

            for (int j = 0; j < MyGraph.EdgesCount; j++)    // Выбор ребра
            {
                if (graphDrawer.IsVisibleEdge(MyGraph.EdgesList[j], e.Location))
                {
                    MyGraph.EdgesList[j].IsFocused = true;
                    graphRedactor.CurrentEdge = MyGraph.EdgesList[j];
                    this.toolStripStatusLabelInfo.Text = this.MyGraph.GetDescription() + " " + this.graphRedactor.CurrentEdge.GetDescription();
                    break;
                }
            }

            //foreach (Edge edge in MyGraph.EdgesList) // Выбор ребра
            //{
            //    if (graphDrawer.IsVisibleEdge(edge, e.Location))
            //    {
            //        edge.IsFocused = true;
            //        graphRedactor.CurrentEdge = edge;
            //        this.toolStripStatusLabelInfo.Text = this.MyGraph.GetInfo() + " " + this.graphRedactor.CurrentEdge.GetInfo();
            //    }
            //    else edge.IsFocused = false;
            //}
            myPanel.Invalidate();
        }
        #endregion My Panel

        #region Open Save Exit
        private void tsbtnSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfDlg = new SaveFileDialog();
            sfDlg.Filter = "XML схемы (*.xml)|*.xml;|Все файлы (*.*)|*.*";
            sfDlg.AddExtension = true;
            if (sfDlg.ShowDialog() == DialogResult.OK)
            {
                if (!GraphSaver.SaveGraph(sfDlg.FileName, MyGraph))
                    Log.DisplayMessage("Ошибка сохранения графа!");
            }
        }

        private void tsbtnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog openDlg = new OpenFileDialog();
            openDlg.Filter = "XML схемы (*.xml, *.xsd)|*.xml;*.xsd|Все файлы (*.*)|*.*";
            if (openDlg.ShowDialog() == DialogResult.OK)
            {
                System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                doc.Load(openDlg.FileName);
                GraphSaver.LoadGraph(doc);
                //MyGraph = GraphSaver.LoadGraph(doc);
                myPanel.Invalidate();
            }
            this.SetPannelSize();
            UpdateGraphData();
        }

        private void tsbtnExit_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Все несохранённые данные будут утеряны! Завершить работу приложения?", "Внимание!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
        #endregion Open Save Exit

        #region Zoom
        private void btnZoomPlus_Click(object sender, EventArgs e)
        {
            graphDrawer.ScaleCoeff += 0.3f;
            myPanel.Invalidate();
        }

        private void btnZoomMinus_Click(object sender, EventArgs e)
        {
            graphDrawer.ScaleCoeff -= 0.3f;
            myPanel.Invalidate();
        }

        private float GetZoomCoefficient(int val)
        {
            switch (val)
            {
                case 0:
                    return 0.3f;
                case 1:
                    return 0.2f;
                case 2:
                    return 0.2f;
                case 3:
                    return 0.2f;
                case 4:
                    return 0.2f;
                case 5:
                    return 1f;
                case 6:
                    return 2f;
                case 7:
                    return 2f;
                case 8:
                    return 2f;
                case 9:
                    return 2f;
                case 10:
                    return 3f;
                default:
                    return 1f;
            }
        }

        private void btnZoomDefault_Click(object sender, EventArgs e)
        {
            graphDrawer.ScaleCoeff = 1f;
            myPanel.Invalidate();
        }
        #endregion Zoom

        #region Scrolling
        private void button1_Click_1(object sender, EventArgs e)
        {
            this.graphDrawer.Y0 += 10;
            this.myPanel.Invalidate();
        }

        private void btnDefaultLocation_Click(object sender, EventArgs e)
        {
            this.graphDrawer.Y0 = 10;
            this.graphDrawer.X0 = 10;
            this.myPanel.Invalidate();
        }

        private void btnMoveUp_Click(object sender, EventArgs e)
        {
            this.graphDrawer.Y0 -= 10;
            this.myPanel.Invalidate();
        }

        private void btnMoveLeft_Click(object sender, EventArgs e)
        {
            this.graphDrawer.X0 -= 10;
            this.myPanel.Invalidate();
        }

        private void btnMoveRight_Click(object sender, EventArgs e)
        {
            this.graphDrawer.X0 += 10;
            this.myPanel.Invalidate();
        }
        #endregion Scrolling

        #region Add RemoveApexes
        private void btnAddApex_Click(object sender, EventArgs e)
        {
            MyGraph.ApexList.Add(Apex.Create());
            this.SetPannelSize();
            myPanel.Invalidate();
            UpdateGraphData();
        }

        private void btnRemoveApex_Click(object sender, EventArgs e)
        {
            this.pathBuilder.HidePath();
            this.graphRedactor.DeleteCurentteApex();
            myPanel.Invalidate();
            this.SetPannelSize();
            UpdateGraphData();
        }
        #endregion Add RemoveApexes

        #region Add Edit Delete Edge
        private void btnCreateEdge_Click(object sender, EventArgs e)
        {
            if (this.graphRedactor.SelectedApex1 == null || this.graphRedactor.SelectedApex2 == null)
            {
                Log.DisplayMessage("Должны быть выбраны две вершины!");
                return;
            }
            AddEdge addEdge = new AddEdge(this.graphRedactor.SelectedApex1.Index, this.graphRedactor.SelectedApex2.Index);
            if (addEdge.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Edge ed1 = new Edge(graphRedactor.SelectedApex1, graphRedactor.SelectedApex2);
                ed1.SetMetric(addEdge.M_From_To);
                this.MyGraph.AddEdge(ed1);

                Edge ed2 = new Edge(graphRedactor.SelectedApex2, graphRedactor.SelectedApex1);
                ed2.SetMetric(addEdge.M_To_From);
                this.MyGraph.AddEdge(ed2);
            }
        }

        private void tsbtnEditEdge_Click(object sender, EventArgs e)
        {
            if (this.graphRedactor.CurrentEdge != null)
            {
                try
                {
                    Edge edgeFromTo = this.graphRedactor.CurrentEdge;
                    Edge edgeToFrom = this.graphRedactor.GetEdge(edgeFromTo.ApexTo.Index, edgeFromTo.ApexFrom.Index);
                    AddEdge editEdge = new AddEdge(edgeFromTo, edgeToFrom);
                    if (editEdge.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        edgeFromTo.SetMetric(editEdge.M_From_To);
                        edgeToFrom.SetMetric(editEdge.M_To_From);
                        this.myPanel.Invalidate();
                    }
                }
                catch (Exception ex) { Log.DisplayMessage(ex.Message); }
            }
        }

        private void btnRemEdges_Click(object sender, EventArgs e)
        {
            this.pathBuilder.HidePath();
            this.graphRedactor.DeleteCurentteEdge();
            myPanel.Invalidate();
        }
        #endregion Add Edit Delete Edge

        #region Generate / Cleare
        private void btnGenerate_Click(object sender, EventArgs e)
        {
            int apexCount = Convert.ToInt32(this.tstbxApexCount.Text);
            if (apexCount <= 1) apexCount = 2;
            else if (apexCount > 1000) apexCount = 1000;

            MyGraph = NetGraph.Generate(apexCount);
            this.SetPannelSize();
            myPanel.Invalidate();
            UpdateGraphData();
        }

        private void btnGenerateEdges_Click(object sender, EventArgs e)
        {
            NetGraph.GenerateEdges(MyGraph);
            this.SetPannelSize();
            myPanel.Invalidate();
        }

        private void tsbtnCreate_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Cоздать граф?", "Внимание!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                int apexCount = Convert.ToInt32(this.tstbxApexCount.Text);
                if (apexCount <= 1) apexCount = 2;
                else if (apexCount > 1000) apexCount = 1000;

                MyGraph.Clear();
                MyGraph = NetGraph.Generate(apexCount);
                NetGraph.GenerateEdges(MyGraph);
                myPanel.Parent.Width = 1000;
                myPanel.Parent.Height = 1000;
                //graphRedactor = new GraphRedactor(MyGraph);
                this.SetPannelSize();
                myPanel.Invalidate();
                UpdateGraphData();
            }
        }

        private void tsbtnClearAll_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Все несохранённые данные будут утеряны! Продолжить?", "Внимание!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                MyGraph.EdgesList.Clear();
                MyGraph.ApexList.Clear();
                this.graphRedactor.Clear();
                Apex.NullApexIndexCounter();
                this.SetPannelSize();
                myPanel.Invalidate();
                UpdateGraphData();
            }
        }
        #endregion Generate / Cleare

        #region Select Apex
        private void tsmiSelectApex_Click(object sender, EventArgs e)
        {
            this.graphRedactor.SelectApex(this.graphRedactor.CurrentApex);
        }

        private void tsmiCancelSelectApex_Click(object sender, EventArgs e)
        {
            this.graphRedactor.UnselectApex(this.graphRedactor.CurrentApex);
        }
        #endregion Select Apex

        /// <summary> Вызов функций из листа  для обновления 
        /// данных о графе на дочерних формах
        /// </summary>
        private void UpdateGraphData()
        {
            foreach (VoidDelegate updateDataMethod in ListOfUpdateData)
            {
                updateDataMethod();
            }
        }
        
        #region Tool Windows Open
        private void edgesPanelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolWindowEdges twEdges = new ToolWindowEdges(this.graphRedactor, () => { myPanel.Invalidate(); });
            twEdges.Show();
            twEdges.TopMost = true;
        }

        private void navigPanelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolWindowNavigation navig = new ToolWindowNavigation(() => { myPanel.Invalidate(); }, this.graphDrawer);
            navig.Show();
            navig.TopMost = true;
        }

        private void tsmiInfo_Click(object sender, EventArgs e)
        {
            if (graphRedactor.CurrentApex == null)
                return;
            ToolWindowInfoApexEdges apexForm = new ToolWindowInfoApexEdges(graphRedactor, graphRedactor.CurrentApex);
            apexForm.ShowDialog();
        }

        private void tsmiAbout_Click(object sender, EventArgs e)
        {
            Log.DisplayMessage("Программа нходится в процессе разработки");
        }
        #endregion Tool Windows Open

        private void tstbxApexCount_TextChanged(object sender, EventArgs e)
        {
            int count = 0;
            if (Int32.TryParse(this.tstbxApexCount.Text, out count))
            {
                if (count < 2) this.tstbxApexCount.Text = "2";
                if (count > 1000) this.tstbxApexCount.Text = "1000";
            }
            else this.tstbxApexCount.Text = "100";
        }

        private void tsbtnAlgorithmInfo_Click(object sender, EventArgs e)
        {
            InfoForm iForm = new InfoForm(new string[] {
                pathBuilder.Algorithm.ToString(),
                pathBuilder.PathMatrixToString() });
            iForm.TopMost = true;
            iForm.Show();
        }

        #region Select Path points
        private void tsmiSelectPathFrom_Click(object sender, EventArgs e)
        {
            this.pathBuilder.PathFrom = this.graphRedactor.CurrentApex.Index;
            this.pathBuilder.IsActualData = false; // <- updating wanted
        }

        private void tsmiSelectPathTo_Click(object sender, EventArgs e)
        {
            this.pathBuilder.DisplayPathTo(this.graphRedactor.CurrentApex.Index);
        }
        #endregion Select Path points

        private void stcbxAlgorithmSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            int algorithmId = ((KeyValuePair<int, string>)tscbxAlgorithmSelector.ComboBox.SelectedItem).Key;

            switch (algorithmId)
            {
                case 0:
                    this.pathBuilder.Algorithm = new AlgorithmDeikstry();
                    break;
                case 1:
                    this.pathBuilder.Algorithm = new AlgorithmBelmanaForda();
                    break;
                case 2:
                    this.pathBuilder.Algorithm = new AlgorithmFloydaWorshala();
                    break;
                default:
                    this.pathBuilder.Algorithm = new AlgorithmDeikstry();
                    break;

            }
        }

        private void tsmiDisplayEdgeMetrics_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.DisplayMetrics = tsmiDisplayEdgeMetrics.Checked;
            Properties.Settings.Default.Save();
            this.myPanel.Invalidate();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            tsmiDisplayEdgeMetrics.Checked = Properties.Settings.Default.DisplayMetrics;
        }

    }
}
