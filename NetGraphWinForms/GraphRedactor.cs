using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetGraphWinForms
{
    public class GraphRedactor
    {
        private NetGraph graph = null;
        public Apex CurrentApex = null;
        public Edge CurrentEdge = null;

        /// <summary> Выбор двух вершин для построения ребер м-ду ними
        /// </summary>
        public Apex SelectedApex1 = null;
        public Apex SelectedApex2 = null;

        public GraphRedactor() //NetGraph _graph)
        {
            this.graph = NetGraph.GetGraph(); // _graph;
        }

        public bool DeleteApex(Apex apex)
        {
            bool res = true;
            try
            {
                RemoveAllConnectionsWithApex(apex.Index);
                if (CurrentApex.IsSelected)
                    this.UnselectApex(CurrentApex);
                graph.ApexList.Remove(apex);
                CorrectIndexesAfterDelete(apex.Index);
            }
            catch (Exception ex)
            {
                Log.DisplayMessage(ex);
                res = false;
            }
            return res;
        }

        /// <summary> Для сохранения последовательной непрерывной индексации вершин графа
        /// понижаем индексы всех вершин от удаленной и до конца
        /// </summary>
        /// <param name="indexFrom"></param>
        private void CorrectIndexesAfterDelete(int indexFrom)
        {
            //bool isOneDeleted = false;
            foreach (Apex apex in this.graph.ApexList)
            {
                if (apex.Index > indexFrom)
                {
                    apex.Index--;
                    //isOneDeleted = true;
                }
            }
            //if (isOneDeleted)
            Apex.SetCountIndex(this.graph.ApexCount); // DecremendIndex();
        }
        //private void CorrectIndexesAfterDelete(int indexFrom)
        //{
        //    bool isOneDeleted = false;
        //    foreach (Apex apex in this.graph.ApexList)
        //    {
        //        if (apex.Index > indexFrom)
        //        {
        //            int id = apex.Index;
        //            var edges = (from edge in graph.EdgesList 
        //                         where edge.ApexFrom.Index == id || edge.ApexTo.Index == id 
        //                         select edge).ToList<Edge>();
        //            foreach (var item in edges)
        //            {
        //                if (item.ApexFrom.Index == id) item.ApexFrom.Index--;
        //                if (item.ApexTo.Index == id) item.ApexTo.Index--;
        //            }
        //            if(edges.Count == 0) // если вершина не связана, то просто уменьшаем ее индекс
        //                apex.Index--;
        //            isOneDeleted = true;
        //        }
        //    }
        //    if (isOneDeleted)
        //        Apex.DecremendIndex();
        //}

        public Edge GetEdge(int apefFromId, int apexToId)
        {
            List<Edge> res = new List<Edge>();
            res = (from ed in graph.EdgesList
                   where (ed.ApexFrom.Index == apefFromId && ed.ApexTo.Index == apexToId)
                   select ed).ToList<Edge>();
            if (res.Count == 0)
                throw new Exception("Нет таких ребер в графе!");
            return res[0];
        }

        /// <summary> Удаление всех связей с вершиной
        /// </summary>
        /// <param name="apexId"></param>
        private void RemoveAllConnectionsWithApex(int apexId)
        {
            var edgesList = (from ed in graph.EdgesList 
                             where (ed.ApexFrom.Index == apexId || ed.ApexTo.Index == apexId) 
                             select ed).ToList<Edge>();
            foreach (var remEdge in edgesList)
            {
                graph.EdgesList.Remove(remEdge);
            }
        }

        public bool RemoveConnection(int apefFromId, int apexToId)
        {
            bool res = false;
            var edgesList = (from ed in graph.EdgesList
                             where (ed.ApexFrom.Index == apefFromId && ed.ApexTo.Index == apexToId)
                             select ed).ToList<Edge>();
            foreach (var remEdge in edgesList)
            {
                graph.EdgesList.Remove(remEdge);
                res = true;
            }
            return res;
        }

        public bool DeleteCurentteApex()
        {
            bool res = true;
            res = DeleteApex(CurrentApex);
            this.CurrentApex = null;
            return res;
        }

        internal void DeleteCurentteEdge()
        {
            RemoveConnection(this.CurrentEdge.ApexFrom.Index, this.CurrentEdge.ApexTo.Index);
            RemoveConnection(this.CurrentEdge.ApexTo.Index, this.CurrentEdge.ApexFrom.Index);
        }

        internal void SelectApex(Apex apex)
        {
            if (apex == null || apex.IsSelected)
                return;

            if (this.SelectedApex1 == null)
            {
                SelectedApex1 = apex;
                apex.IsSelected = true;
            }
            else if (this.SelectedApex2 == null && this.SelectedApex1 != apex)
            {
                SelectedApex2 = apex;
                apex.IsSelected = true;
            }
        }

        internal void UnselectApex(Apex apex)
        {
            if (this.SelectedApex1 == apex) SelectedApex1 = null;
            else if (this.SelectedApex2 == apex) SelectedApex2 = null;
            apex.IsSelected = false;
        }

        internal List<Edge> SelectAllEdges(Apex curentApex)
        {
            return (from ed in graph.EdgesList
                    where (ed.ApexFrom.Index == curentApex.Index || ed.ApexTo.Index == curentApex.Index)
                    select ed).ToList<Edge>();
        }

        internal void Clear()
        {
            this.CurrentApex = null;
            this.CurrentEdge = null;
            this.SelectedApex1 = null;
            this.SelectedApex2 = null;
        }
    }
}
