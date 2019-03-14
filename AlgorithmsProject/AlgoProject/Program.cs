using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace AlgoProject
{
    class Program
    {
        static void Main(string[] args)
        {
            Map m = new Map();
            m.FillMap("map1.txt");
            m.SolveQueries("queries1.txt", "GenOutput.txt");
            bool Identical = m.Compare("output1.txt", "GenOutput.txt");
            if (!Identical)
                Console.WriteLine("Not Identical");
            else
                Console.WriteLine("Identical");
            string s = Console.ReadLine();

        }
    }
}
