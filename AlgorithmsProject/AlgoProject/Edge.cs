using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoProject
{
    public class Edge : IComparable<Edge>
    {
        public int To; 
        
        public double Distance, Time, Speed;
        public Edge(int to, double distance, double speed)
        {
            To = to;
            Distance = distance;
            Speed = speed;
            Time = Distance / Speed; //	calculate the time 


        }
        public Edge(int to, double distance, double speed, double time)
        {
            To = to;
            Distance = distance;
            Speed = speed;
            Time = time;

        }
        public int CompareTo(Edge e)// comparator 3shan el priority queue
        {
            if (this.Time > e.Time) return 1;
            else if (this.Time < e.Time) return -1;
            else return 0;
        }
    }
}
