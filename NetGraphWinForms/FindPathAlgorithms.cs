using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetGraphWinForms
{
    class AlgorithmDeikstry : IGetPathVectoradle
    {
        NetGraph graph = null;

        public int N
        {
            get
            {
                try { return this.graph.ApexCount; }
                catch { return 0; }
            }
        }
        public int[] D = new int[0];
        public int[] P = new int[0];
        public bool[] Used = new bool[0];
        public int[,] Metrics = new int[0, 0];

        #region Time Of Operation
        public DateTime tBegin = DateTime.Now;
        public DateTime tEnd = DateTime.Now;
        public TimeSpan deltaT
        {
            get { return tEnd - tBegin; }
        }
        #endregion Time Of Operation

        public AlgorithmDeikstry()
        {
            graph = NetGraph.GetGraph();
        }

        public void InitialMatrixes()
        {
            tBegin = DateTime.Now;
            int minD = 254 * (this.graph.ApexCount - 1);
            
            D = new int[graph.ApexCount];
            P = new int[graph.ApexCount];
            Used = new bool[graph.ApexCount];
            Metrics = new int[graph.ApexCount, graph.ApexCount];
            
            tEnd = DateTime.Now;

            for (int i = 0; i < graph.ApexCount; i++)
            {
                D[i] = minD;
                Used[i] = false;
                P[i] = 0;
            }
            
            for (int i = 0; i < graph.ApexCount; i++)
                for (int j = 0; j < graph.ApexCount; j++)
                {
                    if(i==j) Metrics[i,j] = 0;
                    else Metrics[i,j] = 255;
                }

            foreach (Edge edge in graph.EdgesList)
                Metrics[edge.ApexFrom.Index, edge.ApexTo.Index] = edge.Metric;
        }

        public int[] GetMinPathesVector(int s)
        {
            D[s] = 0;
            int k = s; // s - индекс текущей вершины

            for (int n = 0; n < this.N; n++)
            {
                int min_d = 254 * (this.graph.ApexCount - 1);

                // выбор вершины с мин.весом
                for (int j = 0; j < this.N; j++)
                {
                    if (!Used[j] && D[j] < min_d)
                    {
                        k = j;
                        min_d = D[j];
                    }
                }
                // помечаем вершины
                Used[k] = true;
                // перебор всех вершин и определение мин. веса
                for (int i = 0; i < this.N; i++)
                {
                    if (Used[i] == false && Metrics[k, i] != 255 && Metrics[k, i] != 0)
                    {
                        if (D[i] > (D[k] + Metrics[k, i]))
                        {
                            D[i] = D[k] + Metrics[k, i];
                            P[i] = k;
                            //P[i] = n;
                        }
                    }
                }
            }
            return P;
        }

        public override string ToString()
        {
            ADataPrinterClass dkPrnter = new ADataPrinterClass(this);
            this.tEnd = DateTime.Now;
            string[] strs = new string[]
            {
                "Алгоритм Дейкстры", Environment.NewLine,
                "Затраченное время " + this.deltaT.ToString(@"h\:m\:ss\.FFFF"), Environment.NewLine,
                dkPrnter.GetMatrixD(), Environment.NewLine,
                dkPrnter.GetMatrixP(), Environment.NewLine,
                dkPrnter.GetMatrixUsed(), Environment.NewLine,
                dkPrnter.GetMatrixMetrics(), Environment.NewLine
            };
            return string.Concat(strs);
        }
    }


    class AlgorithmBelmanaForda : IGetPathVectoradle
    {
        NetGraph graph = null;

        public int N
        {
            get
            {
                try { return this.graph.ApexCount; }
                catch { return 0; }
            }
        }
        public int[] D = new int[0];
        public int[] P = new int[0];
        public int[,] Metrics = new int[0, 0];
        private int d_min = 0;

        #region Time Of Operation
        public DateTime tBegin = DateTime.Now;
        public DateTime tEnd = DateTime.Now;
        public TimeSpan deltaT
        {
            get { return tEnd - tBegin; }
        }
        #endregion Time Of Operation

        public AlgorithmBelmanaForda()
        {
            graph = NetGraph.GetGraph();
        }

        public void InitialMatrixes()
        {
            tBegin = DateTime.Now;
            D = new int[N];
            P = new int[N];
            Metrics = new int[N, N];
            tEnd = DateTime.Now;
            d_min = 254 * (this.N - 1);
            for (int i = 0; i < N; i++)
            {
                D[i] = d_min;
                P[i] = 0;
            }

            for (int i = 0; i < N; i++)
                for (int j = 0; j < N; j++)
                {
                    if (i == j) Metrics[i, j] = 0;
                    else Metrics[i, j] = 255;
                }

            foreach (Edge edge in graph.EdgesList)
                Metrics[edge.ApexFrom.Index, edge.ApexTo.Index] = edge.Metric;
        }

        public int[] GetMinPathesVector(int s)
        {
            D[s] = 0;
            for (int n = 0; n < this.N; n++)
            {
                for (int i = 0; i < this.N; i++)
                {
                    for (int j = 0; j < this.N; j++)
                    {
                        if (D[i] != d_min &&
                            Metrics[i, j] != 0 &&
                            Metrics[i, j] != 255)
                        {
                            if (D[j] > (D[i] + Metrics[i, j]))
                            {
                                D[j] = D[i] + Metrics[i, j];
                                P[j] = i;
                            }
                        }
                    }
                }
            }
            return P;
        }

        public override string ToString()
        {
            this.tEnd = DateTime.Now;
            string[] strs = new string[]
            {
                "Алгоритм Белмана Форда", Environment.NewLine,
                "Затраченное время " + this.deltaT.ToString(@"h\:m\:ss\.FFFF"), Environment.NewLine,
                ADataPrinterClass.VectorToString(this.D, "Вектор D: "), Environment.NewLine,
                ADataPrinterClass.VectorToString(this.P, "Вектор P: "), Environment.NewLine
            };
            return string.Concat(strs);
        }
    }


    class AlgorithmFloydaWorshala : IGetPathVectoradle
    {
        NetGraph graph = null;

        public int N
        {
            get
            {
                try { return this.graph.ApexCount; }
                catch { return 0; }
            }
        }
        public int[] P = new int[0];
        public int[,] Metrics = new int[0, 0];
        public int[,] W = new int[0, 0];
        private int infinity = 0;

        string infoStepsString = string.Empty;

        #region Time Of Operation
        public DateTime tBegin = DateTime.Now;
        public DateTime tEnd = DateTime.Now;
        public TimeSpan deltaT
        {
            get { return tEnd - tBegin; }
        }
        #endregion Time Of Operation

        public AlgorithmFloydaWorshala()
        {
            graph = NetGraph.GetGraph();
        }

        public void InitialMatrixes()
        {
            tBegin = DateTime.Now;
            P = new int[N];
            Metrics = new int[N, N];
            W = new int[N, N];
            tEnd = DateTime.Now;
            infinity = 254 * N;

            for (int i = 0; i < N; i++)
            {
                P[i] = 0;
                for (int j = 0; j < N; j++)
                {
                    if (i == j)
                    {
                        Metrics[i, j] = 0;
                        W[i, j] = 0;
                    }
                    else
                    {
                        Metrics[i, j] = infinity;
                        W[i, j] = infinity;
                    }
                    //W[i, j] = infinity;
                }
            }

            foreach (Edge edge in graph.EdgesList)
            {
                Metrics[edge.ApexFrom.Index, edge.ApexTo.Index] = edge.Metric;
                W[edge.ApexFrom.Index, edge.ApexTo.Index] = edge.Metric;
            }
        }

        public int[] GetMinPathesVector(int s)
        {
            bool done = false;
            for (int i = 0; i < this.N; i++)
            {
                for (int j = 0; j < this.N; j++)
                {
                    for (int k = 0; k < this.N; k++)
                    {
                        if (i != j && //j != k && i != k &&
                            W[i, k] != infinity &&
                            W[i, k] != 0 &&
                            W[k, j] != infinity &&
                            W[k, j] != 0)
                        {
                            this.infoStepsString += string.Format("i = {0}, j = {1}, k = {2};", i, j, k);
                            this.infoStepsString += string.Format("W[{0},{1}] ? (W[{0},{2}] + W[{2},{1}]);", i, j, k);
                            this.infoStepsString += string.Format("{0} ? {1} + {2} ;", W[i, j], W[i, k],  W[k, j]);
                            if (W[i, j] > (W[i, k] + W[k, j]))
                            {
                                W[i, j] = W[i, k] + W[k, j];
                                P[j] = k;
                                this.infoStepsString += string.Format("P[{0}] = {1}{2}", j, k, Environment.NewLine);
                            }
                            else
                            {
                                P[j] = i; 
                                this.infoStepsString += string.Format("P[{0}] = {1}{2}", j, i, Environment.NewLine);
                            }
                        }
                    }
                }
                done = true;
                this.infoStepsString += ADataPrinterClass.VectorToString(this.P, string.Format("Вектор  P {0}", i)) + Environment.NewLine;
                this.infoStepsString += ADataPrinterClass.MatrixToString(this.W, string.Format("Матрица W {0}", i)) + Environment.NewLine;
            }
            return P;
            
            //for (int i = 0; i < this.N; i++)
            //{
            //    for (int j = 0; j < this.N; j++)
            //    {
            //        for (int k = 0; k < this.N; k++)
            //        {
            //            if (Metrics[i, k] != infinity &&
            //                Metrics[i, k] != 0 &&
            //                Metrics[k, j] != infinity &&
            //                Metrics[k, j] != 0)
            //            {
            //                if (Metrics[i, j] > (Metrics[i, k] + Metrics[k, j]))
            //                {
            //                    Metrics[i, j] = Metrics[i, k] + Metrics[k, j];
            //                    P[j] = k;
            //                }
            //                else P[j] = i;
            //            }
            //        }
            //    }
            //    this.infoStepsString += ADataPrinterClass.MatrixToString(this.Metrics, string.Format("Матрица Metrics {0}: ", i)) + Environment.NewLine;
            //}
            //return P;

            //for (int i = 0; i < this.N; i++)
            //{
            //    for (int j = 0; j < this.N; j++)
            //    {
            //        for (int k = 0; k < this.N; k++)
            //        {
            //            if (W[i, k] != infinity &&
            //                W[i, k] != 0 &&
            //                W[k, j] != infinity &&
            //                W[k, j] != 0)
            //            {
            //                if (W[i, k] + W[k, j] < W[i, j])
            //                {
            //                    W[i, j] = W[i, k] + W[k, j];
            //                    P[j] = k;
            //                }
            //                else P[j] = i;
            //            }
            //        }
            //    }
            //}
            ////P[P.Length - 1] = 0;
            //return P;
        }

        public override string ToString()
        {
            this.tEnd = DateTime.Now;
            string[] strs = new string[]
            {
                "Алгоритм Флойда-Уоршала", Environment.NewLine,
                "Затраченное время " + this.deltaT.ToString(@"h\:m\:ss\.FFFF"), Environment.NewLine,
                ADataPrinterClass.VectorToString(this.P, "Вектор P: "), Environment.NewLine,
                infoStepsString, Environment.NewLine,
                ADataPrinterClass.MatrixToString(this.Metrics, "Матрица Metrics: "), Environment.NewLine
                //ADataPrinterClass.MatrixToString(this.W, "Матрица W: "), Environment.NewLine,
            };
            return string.Concat(strs);
        }
    }

    #region ADataPrinterClass
    class ADataPrinterClass
    {
        AlgorithmDeikstry aDeikstraClass = null;
        public ADataPrinterClass(AlgorithmDeikstry _aDeikstraClass)
        {
            this.aDeikstraClass = _aDeikstraClass;
        }

        public string GetMatrixP()
        {
            string res = string.Format("Матрица P:  {0}", Environment.NewLine);
            foreach (var item in aDeikstraClass.P)
                res += string.Format("{0}, ", item.ToString());
            return res + Environment.NewLine;
        }

        public static string VectorToString(int[] arr, string text)
        {
            string res = string.Format("Матрица {0}:  {1}", text, Environment.NewLine);
            foreach (int item in arr)
                res += string.Format("{0}, ", item.ToString());
            return res + Environment.NewLine;
        }

        public static string MatrixToString(int[][] arr, string text)
        {
            string res = string.Format("{0}:  {1}", text, Environment.NewLine);
            foreach (int[] row in arr)
            {
                foreach (int item in row)
                    res += string.Format("{0}, ", item.ToString());
                res += Environment.NewLine;
            }
            return res + Environment.NewLine;
        }

        public static string MatrixToString(int[,] arr, string text)
        {
            int len = (int)Math.Sqrt(arr.Length);
            string res = string.Format("{0}:  {1}", text, Environment.NewLine);
            for (int i = 0; i < len; i++)
            {
                for (int j = 0; j < len; j++)
                    res += string.Format("{0}, ", arr[i,j].ToString());
                res += Environment.NewLine;
            }
            return res + Environment.NewLine;
        }

        public string GetMatrixD()
        {
            string res = string.Format("Матрица D:{0} ", Environment.NewLine);
            foreach (var item in aDeikstraClass.D)
                res += string.Format("{0}, ", item.ToString());
            return res + Environment.NewLine;
        }

        public string GetMatrixUsed()
        {
            string res = string.Format("Матрица Used:{0} ", Environment.NewLine);
            foreach (var item in aDeikstraClass.Used)
                res += string.Format("{0}, ", item.ToString());
            return res + Environment.NewLine;
        }

        public string GetMatrixMetrics()
        {
            string res = string.Format("Матрица Metrics:{0}", Environment.NewLine);
            int i = 0;
            foreach (var item in aDeikstraClass.Metrics)
            {
                res += string.Format("{0}, ", item.ToString());
                i++;
                if (i == aDeikstraClass.N)
                {
                    res += Environment.NewLine;
                    i = 0;
                }
            }
            return res + Environment.NewLine;
        }
    }
    #endregion ADataPrinterClass

    interface IGetPathVectoradle
    {
        void InitialMatrixes();
        int[] GetMinPathesVector(int pointFrom);
    }
}
