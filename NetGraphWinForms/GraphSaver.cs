using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace NetGraphWinForms
{
    class GraphSaver
    {
        public static bool SaveGraph(string fileName, NetGraph graph)
        {
            bool res = true;
            try
            {
                XmlDocument doc = new XmlDocument();
                XmlNode root = doc.CreateNode(XmlNodeType.Element, "net_graph", null);
                doc.AppendChild(root);

                // вершины
                XmlNode allApexesNode = doc.CreateNode(XmlNodeType.Element, "apexes", null);
                root.AppendChild(allApexesNode);

                foreach (Apex apex in graph.ApexList)
                {
                    XmlNode apexNode = doc.CreateNode(XmlNodeType.Element, "apex", null);

                    XmlAttribute atr_apex_index = doc.CreateAttribute("id");
                    atr_apex_index.Value = apex.Index.ToString();
                    apexNode.Attributes.Append(atr_apex_index);

                    XmlAttribute atr_apex_coord_x = doc.CreateAttribute("X");
                    atr_apex_coord_x.Value = apex.X.ToString();
                    apexNode.Attributes.Append(atr_apex_coord_x);

                    XmlAttribute atr_apex_coord_y = doc.CreateAttribute("Y");
                    atr_apex_coord_y.Value = apex.Y.ToString();
                    apexNode.Attributes.Append(atr_apex_coord_y);

                    allApexesNode.AppendChild(apexNode);
                }

                // ребра
                XmlNode allEdgesNode = doc.CreateNode(XmlNodeType.Element, "edges", null);
                root.AppendChild(allEdgesNode);

                foreach (Edge edge in graph.EdgesList)
                {
                    XmlNode edgeNode = doc.CreateNode(XmlNodeType.Element, "edge", null);

                    XmlAttribute atr_apexFromId = doc.CreateAttribute("apexFromId");
                    atr_apexFromId.Value = edge.ApexFrom.Index.ToString();
                    edgeNode.Attributes.Append(atr_apexFromId);

                    XmlAttribute atr_apexToId = doc.CreateAttribute("apexToId");
                    atr_apexToId.Value = edge.ApexTo.Index.ToString();
                    edgeNode.Attributes.Append(atr_apexToId);

                    XmlAttribute atr_metric_value = doc.CreateAttribute("metricValue");
                    atr_metric_value.Value = edge.Metric.ToString();
                    edgeNode.Attributes.Append(atr_metric_value);

                    allEdgesNode.AppendChild(edgeNode);
                }
                doc.Save(fileName);
            }
            catch (Exception ex)
            {
                Log.DisplayMessage(ex);
                res = false;
            }
            return res;
        }

        public static void LoadGraph(XmlDocument xmlDoc)
        {
            NetGraph graph = NetGraph.GetGraph();
            graph.Clear();

            foreach (XmlNode node in xmlDoc.SelectNodes("net_graph"))
            {
                try
                {
                    foreach (XmlNode childBase in node.ChildNodes)
                    {
                        foreach (XmlNode child in childBase.ChildNodes)
                        {
                            if (child.Name == "apex")
                            {
                                int id = Convert.ToInt32(child.Attributes["id"].Value);
                                int coord_x = Convert.ToInt32(child.Attributes["X"].Value);
                                int coord_y = Convert.ToInt32(child.Attributes["Y"].Value);
                                // создание вершин
                                graph.ApexList.Add(Apex.Create(id, coord_x, coord_y));
                            }
                        }

                        foreach (XmlNode child in childBase.ChildNodes)
                        {
                            if (child.Name == "edge")
                            {
                                int apexFromId = Convert.ToInt32(child.Attributes["apexFromId"].Value);
                                int apexToId = Convert.ToInt32(child.Attributes["apexToId"].Value);
                                byte metric = Convert.ToByte(child.Attributes["metricValue"].Value);
                                // создание ребер
                                Edge edge = new Edge(graph.GetApex(apexFromId), graph.GetApex(apexToId));
                                edge.SetMetric(metric);
                                graph.AddEdge(edge);
                            }
                        }
                    }
                }
                catch (Exception ex) { Log.DisplayMessage(ex.ToString()); }
            }
            //return graph;
        }
    }
}
