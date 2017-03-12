using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithm
{
    public class Program
    {
        static void Main(string[] args)
        {
            GeneticAlgorithm fakeProblemGA = new GeneticAlgorithm(0.85, 0.05, false, 10, 5, 5); // CHANGE THE GENERIC TYPE (NOW IT'S INT AS AN EXAMPLE) AND THE PARAMETERS VALUES
            var solution = fakeProblemGA.Run();
            Console.WriteLine("Solution: ");
            Console.WriteLine(solution.bits);

        }
    }
}
