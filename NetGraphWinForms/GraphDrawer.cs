using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace NetGraphWinForms
{

    public class GraphDrawer
    {
        private NetGraph graph = null;

        private float scaleCoeff = 1f;

        public float ScaleCoeff
        {
            get { return this.scaleCoeff; }
            set 
            {
                if (value < 0.1) this.scaleCoeff = 0.1f;
                else if (value > 5) this.scaleCoeff = 5f;
                else this.scaleCoeff = value;
            }
        }
        public double L
        {
            get { return Math.Sqrt(graph.ApexCount * 4); }
        }
        public int X0 = 10;
        public int Y0 = 10;

        #region View Settings Properties
        private int CircleR = 15;
        private float borderWidth = 2f;
        //private float edgeLineWidth = 1.3f;
        private int focusDelta = 5;
        private Brush borderBrush = Brushes.Black;
        private Brush ApexFillBrash = Brushes.LightGreen;
        private Font textFont = new Font("Microsoft Sans Serif", (float)9.75);

        private Brush BorderBrush
        {
            get { return this.borderBrush; }
            set { this.borderBrush = value; }
        }
        private float BorderWidth
        {
            get { return this.borderWidth; }
            set { this.borderWidth = value; }
        }
        private Pen BorderPen
        {
            get { return new Pen(BorderBrush, BorderWidth); }
            set { this.BorderPen = value; }
        }
        #endregion View Settings Properties

        public GraphDrawer() //NetGraph _graph)
        {
            this.graph = NetGraph.GetGraph();
        }

        public Rectangle GetApexArea(Apex apex)
        {
            int scaledX = (int)((X0 + apex.X) * scaleCoeff);
            int scaledY = (int)((Y0 + apex.Y) * scaleCoeff);
            int scaledD = (int)(CircleR * 2 * scaleCoeff);
            return new Rectangle(scaledX, scaledY, scaledD, scaledD);
        }

        public void Draw(Graphics g, NetGraph graph)
        {
            g.ScaleTransform(scaleCoeff, scaleCoeff, System.Drawing.Drawing2D.MatrixOrder.Prepend);
            // Рисование ребер
            foreach (var edge in graph.EdgesList)
                DrawEdge(g, edge); 
            // Рисование вершин
            foreach (var apex in graph.ApexList)
                DrawApex(g, apex); 
        }

        private void DrawApex(Graphics g, Apex apex)
        {
            try
            {
                GraphicsPath path = new GraphicsPath();
                path.AddEllipse(X0 + apex.X, Y0 + apex.Y, CircleR * 2, CircleR * 2);
                if (apex.IsFocused)
                {
                    GraphicsPath focusPath = new GraphicsPath();
                    focusPath.AddEllipse(X0 + apex.X - focusDelta, Y0 + apex.Y - focusDelta,(CircleR + focusDelta) * 2, (CircleR + focusDelta) * 2);
                    g.DrawPath(new Pen(Color.Gray, 1f), focusPath);
                    g.FillPath(new HatchBrush(HatchStyle.BackwardDiagonal, Color.Gray, Color.Transparent), focusPath);
                }
                if (apex.IsSelected)
                {
                    GraphicsPath SelectPath = new GraphicsPath();
                    SelectPath.AddEllipse(X0 + apex.X - focusDelta, Y0 + apex.Y - focusDelta, (CircleR + focusDelta) * 2, (CircleR + focusDelta) * 2);
                    g.DrawPath(new Pen(Color.DarkRed, 1f), SelectPath);
                    g.FillPath(new HatchBrush(HatchStyle.Cross, Color.DarkRed, Color.Transparent), SelectPath);
                }
                // -------------- Gradient Brash ------------------
                PathGradientBrush pthGrBrush = new PathGradientBrush(path);
                pthGrBrush.CenterColor = Color.FromArgb(155, 100, 100, 100);
                Color[] colors = { apex.DisplayColor };
                pthGrBrush.SurroundColors = colors;
                //-------------------------------------------------
                g.DrawPath(BorderPen, path);
                g.FillPath(pthGrBrush, path);
                Point apexCenter = GetApexCenter(apex);
                g.DrawString((apex.Index + 1).ToString(), textFont, BorderBrush, apexCenter.X - CircleR / 2, apexCenter.Y - CircleR / 2);
            }
            catch (Exception ex) { Log.DisplayMessage(ex); }
        }

        public static void DisplayOnInfoPannel(Graphics g, Apex apex)
        {
            try
            {
                GraphicsPath path = new GraphicsPath();
                path.AddEllipse(10, 10, 80, 80);
                if (apex.IsSelected)
                {
                    GraphicsPath SelectPath = new GraphicsPath();
                    SelectPath.AddEllipse(5, 5, 90, 90);
                    g.DrawPath(new Pen(Color.DarkRed, 1f), SelectPath);
                    g.FillPath(new HatchBrush(HatchStyle.Cross, Color.DarkRed, Color.Transparent), SelectPath);
                }
                // -------------- Gradient Brash ------------------
                PathGradientBrush pthGrBrush = new PathGradientBrush(path);
                pthGrBrush.CenterColor = Color.FromArgb(155, 100, 100, 100);
                Color[] colors = { apex.DisplayColor };
                pthGrBrush.SurroundColors = colors;
                //-------------------------------------------------
                g.DrawPath(new Pen(Brushes.Black, 2f), path);
                g.FillPath(pthGrBrush, path);
                g.DrawString((apex.Index + 1).ToString(), new Font("Microsoft Sans Serif", (float)11), Brushes.Black, 40, 40);
            }
            catch (Exception ex) { Log.DisplayMessage(ex); }
        }

        private Point GetApexCenter(Apex apex)
        {
            return new Point(X0 + apex.X + CircleR, Y0 + apex.Y + CircleR);
        }

        private void DrawEdge(Graphics g, Edge edge)
        {
            Point apFromCenter = GetApexCenter(edge.ApexFrom);
            Point apToCenter = GetApexCenter(edge.ApexTo);
            try
            {
                int endArrowSize = 8;
                int izgibKrivoy = 20;
                int deltaX = izgibKrivoy;
                int deltaY = izgibKrivoy / 2;
                if (edge.ApexFrom.X <= edge.ApexTo.X) deltaX = -izgibKrivoy;
                if (edge.ApexFrom.Y <= edge.ApexTo.Y) deltaY = -izgibKrivoy / 2;

                Point MidPoint = new Point((apFromCenter.X + apToCenter.X) / 2 + deltaX, (apFromCenter.Y + apToCenter.Y) / 2 + deltaY);
                Point[] points = new Point[] { apFromCenter, MidPoint, apToCenter };

                GraphicsPath path = new GraphicsPath();
                path.AddCurve(points, 0.7f);
                Pen pen = new Pen(GetEdgeColor(edge.Metric), edge. LineWidth);
                // стрелка в конце отрезка
                GraphicsPath capPath = new GraphicsPath();
                capPath.AddLine(0, -CircleR, -endArrowSize / 2, - CircleR - endArrowSize);
                capPath.AddLine(0, -CircleR, endArrowSize / 2, -CircleR - endArrowSize);
                pen.CustomEndCap = new System.Drawing.Drawing2D.CustomLineCap(null, capPath);
                edge.Path = path;
                if (edge.IsFocused)
                {
                    RectangleF rectF = path.GetBounds();
                    Rectangle rect = new Rectangle((int)rectF.X, (int)rectF.Y, (int)rectF.Width, (int)rectF.Height);
                    Pen penline = new Pen(Color.Gray, 1f);
                    penline.DashStyle = DashStyle.Dash;
                    g.DrawRectangle(penline, rect);
                }
                g.DrawPath(pen, path);
                if (Properties.Settings.Default.DisplayMetrics)
                {
                    g.DrawString(edge.Metric.ToString(), textFont, BorderBrush, MidPoint);
                    //g.DrawString(string.Format("m[{0},{1}]={2}", edge.ApexFrom.Index + 1, edge.ApexTo.Index + 1, edge.Metric), textFont, BorderBrush, MidPoint);
                }
            }
            catch (Exception ex) { Log.DisplayMessage(ex); }
        }

        public bool IsVisibleEdge(Edge edge, Point point)
        {
            if ( GetEdgeArea(edge). IsVisible(point))
                return true;
            return false;
        }

        //private GraphicsPath GetEdgeArea(Edge edge)
        //{
        //    Point apFromCenter = GetApexCenter(edge.ApexFrom);
        //    Point apToCenter = GetApexCenter(edge.ApexTo);
        //
        //    int izgibKrivoy = 20;
        //    int deltaX = izgibKrivoy;
        //    int deltaY = izgibKrivoy / 2;
        //    if (edge.ApexFrom.X <= edge.ApexTo.X) deltaX = -izgibKrivoy;
        //    if (edge.ApexFrom.Y <= edge.ApexTo.Y) deltaY = -izgibKrivoy / 2;
        //
        //    Point MidPoint = new Point((int)((apFromCenter.X + apToCenter.X) * scaleCoeff / 2 + deltaX * scaleCoeff),
        //        (int)((apFromCenter.Y + apToCenter.Y) * scaleCoeff / 2 + deltaY * scaleCoeff));
        //    Point[] points = new Point[] { apFromCenter, MidPoint, apToCenter };
        //
        //    GraphicsPath path = new GraphicsPath();
        //    path.AddCurve(points, 0.7f);
        //    return path;
        //}

        private GraphicsPath GetEdgeArea(Edge edge)
        {
            Point apFromCenter = GetApexCenter(edge.ApexFrom);
            Point apToCenter = GetApexCenter(edge.ApexTo);

            int izgibKrivoy = 20;
            int deltaX = izgibKrivoy;
            int deltaY = izgibKrivoy / 2;
            if (edge.ApexFrom.X <= edge.ApexTo.X) deltaX = -izgibKrivoy;
            if (edge.ApexFrom.Y <= edge.ApexTo.Y) deltaY = -izgibKrivoy / 2;

            Point MidPoint = new Point((int)((apFromCenter.X + apToCenter.X) * scaleCoeff / 2 + deltaX * scaleCoeff),
                (int)((apFromCenter.Y + apToCenter.Y) * scaleCoeff / 2 + deltaY * scaleCoeff));

            Point[] points = new Point[] { ScaleCoord(apFromCenter), ScaleCoord(MidPoint), ScaleCoord(apToCenter) };

            GraphicsPath path = new GraphicsPath();
            path.AddCurve(points, 0.7f);
            return path;
        }

        /// <summary> Цвет отображения зависит от метрики
        /// </summary>
        public static Color GetEdgeColor(byte metric)
        {
            if (metric >= 1 && metric <= 13) return Color.FromArgb(0, 0, 64);
            else if (metric >= 14 && metric <= 26) return Color.FromArgb(0, 0, 128);
            else if (metric >= 27 && metric <= 39) return Color.FromArgb(0, 0, 192);
            else if (metric >= 40 && metric <= 52) return Color.FromArgb(0, 0, 255);
            else if (metric >= 53 && metric <= 65) return Color.FromArgb(0, 64, 255);
            else if (metric >= 66 && metric <= 78) return Color.FromArgb(0, 128, 255);
            else if (metric >= 79 && metric <= 91) return Color.FromArgb(0, 192, 255);
            else if (metric >= 92 && metric <= 104) return Color.FromArgb(0, 255, 255);
            else if (metric >= 105 && metric <= 117) return Color.FromArgb(0, 255, 192);
            else if (metric >= 118 && metric <= 130) return Color.FromArgb(0, 255, 128);
            else if (metric >= 131 && metric <= 143) return Color.FromArgb(0, 255, 64);
            else if (metric >= 144 && metric <= 156) return Color.FromArgb(0, 255, 0);
            else if (metric >= 157 && metric <= 169) return Color.FromArgb(64, 255, 0);
            else if (metric >= 170 && metric <= 182) return Color.FromArgb(128, 255, 0);
            else if (metric >= 183 && metric <= 195) return Color.FromArgb(192, 255, 0);
            else if (metric >= 196 && metric <= 208) return Color.FromArgb(255, 255, 0);
            else if (metric >= 209 && metric <= 221) return Color.FromArgb(255, 192, 0);
            else if (metric >= 222 && metric <= 234) return Color.FromArgb(255, 128, 0);
            else if (metric >= 235 && metric <= 247) return Color.FromArgb(255, 64, 0);
            else if (metric >= 248 && metric <= 254) return Color.FromArgb(255, 0, 0);
            else return Color.Transparent;
        }

        internal int CorrectorCoordinateX(int p)
        {
            return p;
        }

        internal int ScaleCoord(int coord)
        {
            return (int)(coord * ScaleCoeff);
        }

        internal Point ScaleCoord(Point coord)
        {
            return new Point((int)(coord.X * ScaleCoeff), 
                (int)(coord.Y * ScaleCoeff));
        }
    }
}
