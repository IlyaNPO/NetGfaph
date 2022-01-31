using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace NetGraphWinForms
{
    // comment from user1
    public class NetGraph : IInfoDisplayable
    {
        private static NetGraph netraph = null;

        public int ApexCount
        {
            get { return this.ApexList.Count; }
        }
        public int EdgesCount
        {
            get { return this.EdgesList.Count; }
        }

        public List<Apex> ApexList = new List<Apex>();
        public List<Edge> EdgesList = new List<Edge>();

        private NetGraph() { }

        public static NetGraph GetGraph()
        {
            if(netraph == null)
                netraph = new NetGraph();
            return netraph;
        }

        public static NetGraph Create()
        {
            netraph = new NetGraph();
            return netraph;
        }

        internal static NetGraph Generate(int apexCount)
        {
            Apex.NullApexIndexCounter();
            NetGraph graph = NetGraph.GetGraph();

            int countInRow = 5;
            int xDistance = 100; 
            int yDistance = 100;
            int x0 = 20;
            int y0 = 20;
            int yJumpCoord = 30; // разброс координат по ОУ
            DetermineApexesLocation(out xDistance, out yDistance, out countInRow, apexCount);

            int x = x0;
            int y = y0;
            bool jump = true;
            for (int i = 0; i < apexCount; i++)
            {
                Apex apex = Apex.Create(x, y);
                graph.ApexList.Add(apex);
                x += xDistance;
                if (jump) y += yJumpCoord;
                else y -= yJumpCoord;
                jump = !jump;

                if ((i + 1) % countInRow == 0)
                {
                    y += yDistance;
                    x = x0;
                }
            }

            return graph;
        }

        internal static bool GenerateEdges(NetGraph graph)
        {
            try
            {
                graph.EdgesList.Clear();
                Random rndm = new Random(DateTime.Now.Second); 
                foreach (Apex apex in graph.ApexList)
                {
                    int apexEdgesCount = rndm.Next(1, 4); // случайное число ребер для текущей вершины
                    for (int i = 0; i < apexEdgesCount; i++)
                    {
                        int connectedAppex = rndm.Next(0, graph.ApexCount);
                        while (connectedAppex == apex.Index)
                        {
                            connectedAppex = rndm.Next(0, graph.ApexCount);
                        }
                        Edge edgeTo = new Edge(apex, graph.ApexList[connectedAppex]);
                        edgeTo.SetMetric((byte)rndm.Next(1, 254));
                        graph.AddEdge(edgeTo);
                        Edge edgeFrom = new Edge(graph.ApexList[connectedAppex], apex);
                        edgeFrom.SetMetric((byte)rndm.Next(1, 254));
                        graph.AddEdge(edgeFrom);
                    }
                }                
                return true;
            }
            catch (Exception ex)
            {
                Log.DisplayMessage(ex);
                return false;
            }
        }

        /// <summary> определение компоновки вершин на сцеме в зависимости от количества
        /// </summary>
        /// <param name="dX">дистанция по ОХ</param>
        /// <param name="dY">дистанция по ОУ</param>
        /// <param name="countInow">кол-во вершин в строке</param>
        /// <param name="apCount">кол-во вершин</param>
        private static void DetermineApexesLocation(out int dX, out int dY, out int countInow, int apCount)
        {
            if (apCount <= 25)
            {
                dX = 150;
                dY = 150;
                countInow = 5;
            }
            else if (apCount > 25 && apCount <= 100)
            {
                dX = 100;
                dY = 80;
                countInow = 10;
            }
            else if (apCount > 100 && apCount <= 529)
            {
                dX = 70;
                dY = 70;
                countInow = 20;
            }
            else 
            {
                dX = 50;
                dY = 50;
                countInow = 45;
            }
        }

        public Apex GetApex(int apexId)
        {
            var ap = from apex in this.ApexList where apex.Index == apexId select apex;
            List<Apex> aList = ap.ToList<Apex>();
            if(aList.Count > 0) return aList[0];
            else return null;
        }

        public Edge GetEdge(int apefFromId, int apexToId)
        {
            List<Edge> res = new List<Edge>();
            res = (from ed in this.EdgesList
                   where (ed.ApexFrom.Index == apefFromId && ed.ApexTo.Index == apexToId)
                   select ed).ToList<Edge>();
            if (res.Count == 0)
                return null;
            return res[0];
        }

        public void AddEdge(Edge edge)
        {
            if (!this.ContainsEdge(edge.ApexFrom.Index, edge.ApexTo.Index))
                this.EdgesList.Add(edge);
        }

        internal void AddEdge(int fromId, int toId, byte m1, byte m2)
        {
            Edge edge1 = new Edge(GetApex(fromId), GetApex(toId));
            edge1.SetMetric(m1);
            EdgesList.Add(edge1);

            Edge edge2 = new Edge(GetApex(toId), GetApex(fromId));
            edge2.SetMetric(m2);
            EdgesList.Add(edge2);
        }

        public bool ContainsEdge(int fromId, int toId)
        {
            bool result = false;
            var edgesList = (from ed in this.EdgesList
                             where (ed.ApexFrom.Index == fromId && ed.ApexTo.Index == toId)
                             select ed).ToList<Edge>();
            if (edgesList.Count > 0) 
                result = true;
            return result;
        }

        public void Clear()
        {
            this.ApexList.Clear();
            this.EdgesList.Clear();
            Apex.NullApexIndexCounter();
        }

        public string GetDescription()
        {
            return string.Format("Граф: вершин {0}, ребер {1}", this.ApexCount, this.EdgesCount);
        }

        internal void SetDefaultDisplay()
        {
            foreach (var apex in this.ApexList)
                apex.SetDefaultDisplayColor();
            foreach (var edge in this.EdgesList)
                edge.SetDefaultLineWidth();
        }
    }

    #region Apex
    public class Apex : IInfoDisplayable
    {
        private static int currentApexIndex = 0; // для последовательной раздачи индексов

        public int X = 10;
        public int Y = 10;
        private int index = 0;

        private bool isFocused = false;
        private bool isDragable = false;
        private bool isSelected = false;

        private Color displayColor = Color.White;

        public bool IsFocused
        {
            get { return this.isFocused; }
            set { this.isFocused = value; }
        }
        public bool IsDragable
        {
            get { return this.isDragable; }
            set { this.isDragable = value; }
        }
        public bool IsSelected
        {
            get { return this.isSelected; }
            set { this.isSelected = value; }
        }
        public Point Location
        {
            get { return new Point(X, Y); }
        }
        public Color DisplayColor
        {
            get { return this.displayColor; }
            set { this.displayColor = value; }
        }

        public int Index
        {
            get { return this.index; }
            set { this.index = value; }
        }

        public Apex(int _n, int _x, int _y)
        {
            index = _n;
            X = _x;
            Y = _y;
        }

        public static Apex Create()
        {
            Apex apex = new Apex(currentApexIndex, 10 + currentApexIndex * 5, 10 + currentApexIndex * 5);
            currentApexIndex++;
            return apex;
        }

        public static Apex Create(int x, int y)
        {
            Apex apex = new Apex(currentApexIndex, x, y);
            currentApexIndex++;
            return apex;
        }

        public static Apex Create(int id, int x, int y)
        {
            Apex apex = new Apex(id, x, y);
            if(currentApexIndex < id) 
                currentApexIndex++;
            return apex;
        }

        public static void NullApexIndexCounter()
        {
            currentApexIndex = 0;
        }

        public string GetDescription()
        {
            return string.Format("Вершина {0}; ", this.Index + 1);
        }

        public void SetDefaultDisplayColor()
        {
            this.displayColor = Color.White;
        }

        internal static void DecremendIndex()
        {
            if(currentApexIndex > 0)
                currentApexIndex--;
        }
        
        internal static void SetCountIndex(int index)
        {
            currentApexIndex = index;
        }
    }
    #endregion Apex

    #region Edge
    public class Edge : IInfoDisplayable
    {
        Apex apexFrom = null;
        Apex apexTo = null;
        //double s = 0;
        byte metric = 25;
        float lineWidth = 1.2f;
        private bool isFocused = false;
        public System.Drawing.Drawing2D.GraphicsPath Path = null;

        public Apex ApexFrom
        {
            get { return this.apexFrom; }
        }
        public Apex ApexTo
        {
            get { return this.apexTo; }
        }
        public double S
        {
            //get { return this.s; }
            get
            {
                double xPow2 = Math.Pow(apexTo.X - apexFrom.X, 2);
                double yPow2 = Math.Pow(apexTo.Y - apexFrom.Y, 2); 
                double s = Math.Sqrt(xPow2 + yPow2);
                if (s > 1000)  // if (s > 10000) 
                    this.metric = 255;
                return s;
            }
        }
        public byte Metric
        {
            get { return this.metric; }
        }
        public float LineWidth
        {
            get { return lineWidth; }
            set { this.lineWidth = value; }
        }
        public bool IsFocused
        {
            get { return this.isFocused; }
            set { this.isFocused = value; }
        }

        private Edge() { }

        public Edge(Apex _apexFrom, Apex _apexTo)
        {
            this.apexFrom = _apexFrom;
            this.apexTo = _apexTo;

            //double xPow2 = Math.Pow(apexTo.X - apexFrom.X, 2);
            //double yPow2 = Math.Pow(apexTo.Y - apexFrom.Y, 2);
            //this.s = Math.Sqrt(xPow2 + yPow2);
        }

        public void SetMetric(byte value)
        {
            this.metric = value;
        }

        public string GetDescription()
        {
            return string.Format("Ребро ({0} - {1}), метрика m={2}",
                this.ApexFrom.Index + 1,
                this.ApexTo.Index + 1,
                Metric);
        }

        internal void SetDefaultLineWidth()
        {
            this.LineWidth = 1.2f;
        }
    }
    #endregion Edge

    interface IInfoDisplayable
    {
        string GetDescription();
    }
}
