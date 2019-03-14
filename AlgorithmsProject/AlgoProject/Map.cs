using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
namespace AlgoProject
{
    class Map
    {
        int Nodes; // no. of nodes.
        int Edges; // no. of edges.
        List<Node> NodesList = new List<Node>(); 
        List<List<Edge>> EdgesList = new List<List<Edge>>(); 
        List<int> Ends = new List<int>(); //list of ids of possible ends
        List<int> Path = new List<int>();
        Node Src, Des; // Source , destination
        double MaxDes; //maximum walking distance
        StreamWriter SW; //Stream Writer to write the output on file.
        public void FillMap(string filename)
        {
            FileStream stream = new FileStream(filename, FileMode.Open);
            StreamReader SR = new StreamReader(stream);
            while (SR.Peek() != -1)
            {
                // read the no. of nodes
                Nodes = int.Parse(SR.ReadLine());
                for (int i = 0; i < Nodes; ++i)
                {
                    // read node data and add it on NodesList
                    string[] line = SR.ReadLine().Split(' ');
                    int id = int.Parse(line[0]);
                    double x = double.Parse(line[1]);
                    double y = double.Parse(line[2]);
                    Node NewNode = new Node(x, y);
                    NodesList.Add(NewNode);
                    // initialize the list of the edges of each node with empty list
                    EdgesList.Add(new List<Edge>());
                }
                //initialize the list of the edges of the source and destination nodes with empty list
                EdgesList.Add(new List<Edge>());
                EdgesList.Add(new List<Edge>());

                //read the no. of edges
                Edges = int.Parse(SR.ReadLine());
                for (int i = 0; i < Edges; ++i)
                {
                    
                    string[] line = SR.ReadLine().Split(' ');
                    // read node1 , node2 , distance , speed

                    int id1 = int.Parse(line[0]);
                    int id2 = int.Parse(line[1]);
                    double distance = double.Parse(line[2]);
                    double speed = double.Parse(line[3]);
                    // bidirectional edge, add it in both nodes

                    Edge edge = new Edge(id2, distance, speed);
                    EdgesList[id1].Add(edge);
                    edge = new Edge(id1, distance, speed);
                    EdgesList[id2].Add(edge);
                }
            }
            SR.Close();
        }

        public void SolveQueries(string queries,string output)
        {
            Stopwatch all = Stopwatch.StartNew();

            FileStream stream = new FileStream(queries, FileMode.Open);
            StreamReader SR = new StreamReader(stream);

            FileStream outstream = new FileStream(output, FileMode.Create);
            SW = new StreamWriter(outstream);

            //read the no. of queries
            int Q = int.Parse(SR.ReadLine());
            for (int i = 0; i < Q; ++i)
            {


                // read the coordinates of source node and destination node
                string[] line = SR.ReadLine().Split(' ');
                double x1 = double.Parse(line[0]);
                double y1 = double.Parse(line[1]);
                double x2 = double.Parse(line[2]);
                double y2 = double.Parse(line[3]);
                // read the maximum walking distance
                double r = double.Parse(line[4]);
             //   Stopwatch sw = Stopwatch.StartNew();
                Solve(new Node(x1, y1), new Node(x2, y2), r);
           //     sw.Stop();
         //       SW.WriteLine(sw.ElapsedMilliseconds + " ms");
                SW.WriteLine();


            }

            all.Stop();
            string time = all.ElapsedMilliseconds.ToString();            
            SW.WriteLine(time + " ms");
            SW.Close();

        }


        public bool Compare(string file1,string file2)
        {
            // assume that the two files are identical
            bool flag = true;
            FileStream stream1 = new FileStream(file1, FileMode.Open);
            StreamReader SR1 = new StreamReader(stream1);
            FileStream stream2 = new FileStream(file2, FileMode.Open);
            StreamReader SR2 = new StreamReader(stream2);
            int i = 0;
            while (SR1.Peek() != -1)
            {
                string s1 = SR1.ReadLine();
                string s2 = SR2.ReadLine();
                ++i;
                // if it’s blank line or it’s the time execution of query 
                // or it’s the time execution of whole program
                //don’t compare 
                if (string.IsNullOrEmpty(s1) || (s1[s1.Length - 1] =='s'&&s1[s1.Length - 2]=='m') || SR1.Peek() == -1) continue;

                if (s1 != s2)
                {
                    // if the two lines are not identical print them
                    Console.WriteLine(i + " " + s1 + " " + s2);
                    // the two files are not identical
                    flag = false;
                }
            }
            return flag;
         }
        public void ShowMap()
        {
            for (int i = 0; i < NodesList.Count; ++i)
            {
                Node NewNode = NodesList[i];
                Console.WriteLine(i + " " + NewNode.X + " " + NewNode.Y);
            }

            for (int i = 0; i < EdgesList.Count; ++i)
            {
                Console.WriteLine(i);
                for (int j = 0; j < EdgesList[i].Count; ++j)
                {
                    Edge edge = EdgesList[i][j];
                    Console.WriteLine(edge.To + " " + edge.Distance + " " + edge.Speed + " " + edge.Time);
                }
            }
        }

        public void Solve(Node s, Node d, double r)
        {
            Des = d;
            Src = s;
            MaxDes = r / (double)1000.0; // convert maximum walking distance from meters to kilometers
            PossibleEndsAndStarts(); // find possible starts and ends
            // e3mel add lel source w el destination k2enohom nodes 3andak fe a5er el NodesList
            NodesList.Add(Src); // add src node on pos Nodes, keda ka2en el id bta3ha Nodes
            NodesList.Add(Des); // add des node on pos Nodes + 1, keda ka2en el id bta3ha Nodes + 1 
            Dij(Nodes, Nodes + 1);  // find the shortest path between node with id Nodes(src) and node with id Node +1 (des)
            DeleteTempEdges();
        }
        public void PossibleEndsAndStarts()
        {
            Ends.Clear();
            double x1, y1, x2, y2, distance;
            for (int i = 0; i < Nodes; ++i)
            {
                x1 = NodesList[i].X; 
                y1 = NodesList[i].Y;
                x2 = Src.X;
                y2 = Src.Y;
                // calc distance between the src and node with id i
                distance = (double)Math.Sqrt(Math.Pow((double)(x2 - x1), 2) + Math.Pow((double)(y2 - y1), 2));
                if (distance <= MaxDes)
                {
                    // add edge from src to node with id i 
                    // el distance 5.0 3ashan hwa bymshy 5 km/h 
                    Edge edge = new Edge(i, distance, (double)5.0);
                    EdgesList[Nodes].Add(edge);
                }


                x2 = Des.X;
                y2 = Des.Y;
                // calc distance between the des and node with id i

                distance = (double)Math.Sqrt(Math.Pow((double)(x2 - x1), 2) + Math.Pow((double)(y2 - y1), 2));
                if (distance <= MaxDes)
                {
                    //add edge from node with id i to des
                    Edge edge = new Edge(Nodes + 1, distance,(double) 5.0);
                    EdgesList[i].Add(edge);
                    //to free this edge after 
                    Ends.Add(i);
                }
            }

        }//possible


        public void Dij(int src, int des)
        {
            bool[] Vis = new bool[EdgesList.Count + 10]; // visited array
            double[] Distance = new double[EdgesList.Count + 10]; // distance from src to every node
            int[] Parent = new int[EdgesList.Count + 10]; // parent [nodeid]  = parent node id
            double[] MinTime = new double[EdgesList.Count + 10]; // minimum time from src to every node

            //initialize time to infinity
            for (int i = 0; i < EdgesList.Count + 10; i++) 
                MinTime[i] = (double)1e15; 

            Edge e = new Edge(src, 0, 0, 0); 
            MinTime[src] = 0; //time from src to src = 0
            Parent[src] = -1; // src node has no parent


            PriorityQueue<Edge> pq = new PriorityQueue<Edge>(); // min heap 
            pq.Enqueue(e);
            while (pq.Count() != 0)
            {
                double time = pq.Peek().Time;
                double distance = pq.Peek().Distance;
                int Node = pq.Peek().To;
                pq.Dequeue();

                if (Node == des) break;
                Vis[Node] = true; // mark it as visited 

                // childrens of Node
                for (int i = 0; i < EdgesList[Node].Count; ++i)
                {
                    int CurNode = EdgesList[Node][i].To;
                    double CurDistance = EdgesList[Node][i].Distance;
                    double CurTime = EdgesList[Node][i].Time;
                    if (Vis[CurNode]) //if visited. for sure i came with time less than current, so continue
                        continue;

                    if (MinTime[CurNode] > MinTime[Node] + CurTime)
                    {
                        MinTime[CurNode] = MinTime[Node] + CurTime;
                        Distance[CurNode] = Distance[Node] + CurDistance;
                        e = new Edge(CurNode, Distance[CurNode], 0, MinTime[CurNode]);
                        pq.Enqueue(e);
                        // update parent of CurNode
                         Parent[CurNode] = Node;
                    }

                }//for
            }//while


            BuildPath(Parent, des);
            for(int i=1;i<Path.Count-2;++i)
            {
                SW.Write(Path[i] + " ");
            }
            SW.WriteLine(Path[Path.Count - 2]);

            // Path[0] = Nodes, node with id (Nodes) ell hya el src, de haymsheha 3ala reglo
            // el node el 2a5era fel path feha node with id (Nodes +1) ell hya el des, de haymsheha 3ala reglo
            
            int StartNode = Path[1];  // awel node hayrkab feha el 3arabya
            int EndNode = Path[Path.Count - 2]; // el node el haynzel feha mn el 3arabya 

            double WholeTime = MinTime[des] * 60; 
            double WholeDistance = Distance[des]; 


            double ToStart = Distance[StartNode]; // distance from src to starc node
            double ToEnd = Math.Abs(Distance[des] - Distance[EndNode]); // distance from end node to des node

            double WalkingDistance = ToStart + ToEnd; 

            double CarDistance = Math.Abs(WholeDistance - WalkingDistance);

            SW.WriteLine(WholeTime.ToString("0.00") + " mins");
            SW.WriteLine(WholeDistance.ToString("0.00") + " km");
            SW.WriteLine(WalkingDistance.ToString("0.00") + " km");
            SW.WriteLine(CarDistance.ToString("0.00") + " km");
          
        }
        public void BuildPath(int[] parents, int des)
        {
            Path.Clear();
            int node = parents[des];
            Path.Add(des);
            while (node != -1)
            {
                Path.Add(node);
                node = parents[node];
            }
            Path.Reverse();

        }

        public void DeleteTempEdges()
        {
            //remove edges from possible ends to destination
            for (int i = 0; i < Ends.Count; ++i)
            {
                int node = Ends[i];
                EdgesList[node].RemoveAt(EdgesList[node].Count - 1);
            }  
            EdgesList[Nodes].Clear();
            EdgesList[Nodes + 1].Clear();
        }

    }//class

}//namespace
