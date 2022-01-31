using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace NetGraphWinForms
{
    class PathBuilder
    {
        private NetGraph graph = null;
        private int pathFrom = 0;
        int[][] path = new int[0][];
        bool isActualData = false;
        public IGetPathVectoradle Algorithm = null;
        
        #region Path Display Settings
        Color apexFillColor = Color.Blue; //Color.Red;
        float edgeWifth = 4f;
        #endregion Path Display Settings

        public int PathFrom
        {
            get { return this.pathFrom; }
            set { this.pathFrom = value; }
        }
        public bool IsActualData
        {
            get { return this.isActualData; }
            set { this.isActualData = value; }
        }

        public PathBuilder()
        {
            this.graph = NetGraph.GetGraph();
        }

        private void Calculate()
        {
            //Algorithm = new AlgorithmDeikstry();
            Algorithm.InitialMatrixes();
            int[] P = Algorithm.GetMinPathesVector(pathFrom);
            if(Algorithm is AlgorithmFloydaWorshala)
                return;
            this.path = PathBuilder.GetPathMatrix(P, pathFrom);
            this.IsActualData = true;
        }

        public static int[][] GetPathMatrix(int[] p, int s)
        {
            int N = p.Length;
            //
            int[][] path = new int[N][];
            for (int init_i = 0; init_i < N; init_i++)
            {
                path[init_i] = new int[N];
                for (int init_j = 0; init_j < N; init_j++)
                    path[init_i][init_j] = -1;
            }

            int k = 0;
            for (int n = 0; n < N; n++)
            {
                if (n != s)
                {
                    k = n;
                repeat:
                    HelpClassel.MoveVector(ref path[n], 1);
                    path[n][1] = p[k];
                    k = p[k];

                    if (p[k] != s)
                    {
                        if (p[k] == 0) 
                            ; //throw new Exception("Ошибка алгоритма определения путей!");
                        else goto repeat;
                    }
                }
            }
            return path;
        }


        public void DisplayPathTo(int pathTo)
        {
            HidePath();
            try
            {
                if (!IsActualData)
                    this.Calculate();
                graph.ApexList[pathFrom].DisplayColor = apexFillColor;
                graph.ApexList[pathTo].DisplayColor = apexFillColor;

                int apexesCountInPath = 0;
                for (int i = 1; i < path[pathTo].Length; i++)
                {
                    if (path[pathTo][i] != -1)
                    {
                        int indexFrom = path[pathTo][i - 1];
                        int indexTo = path[pathTo][i];
                        graph.ApexList[indexTo].DisplayColor = apexFillColor;
                        BoldEdge(indexFrom, indexTo);
                        apexesCountInPath++;
                    }
                }

                if (apexesCountInPath == 0)
                    BoldEdge(this.pathFrom, pathTo);
                else
                {
                    BoldEdge(this.pathFrom, path[pathTo][1]);
                    BoldEdge(path[pathTo][apexesCountInPath], pathTo);
                }
            }
            catch (Exception ex) { Log.DisplayMessage(ex); }
        }

        public void HidePath()
        {
            this.graph.SetDefaultDisplay();
        }

        /// <summary> Set Edge Bold
        /// </summary>
        /// <param name="indexFrom"></param>
        /// <param name="indexTo"></param>
        private void BoldEdge(int indexFrom, int indexTo)
        {
            Edge edge = graph.GetEdge(indexFrom, indexTo);
            if (edge != null)
                edge.LineWidth = this.edgeWifth;
        }

        public string PathMatrixToString()
        {
            try
            {
                return ADataPrinterClass.MatrixToString(this.path, 
                    string.Format("Матрица путей от вершины {0}. ", this.pathFrom + 1));
            }
            catch (Exception ex) { return ex.Message; }
        }
    }

}
