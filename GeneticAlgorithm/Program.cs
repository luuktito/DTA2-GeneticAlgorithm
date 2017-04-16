using System;

namespace GeneticAlgorithm
{
    public class Program
    {
        static void Main(string[] args)
        {
            GeneticAlgorithm fakeProblemGA = new GeneticAlgorithm(0.85, 0.05, true, 50, 100, 5);
            var solution = fakeProblemGA.Run();
            Console.WriteLine("Best Individual: " + solution.bits + " (" + Convert.ToInt32(solution.bits, 2) + ")");
        }
    }
}
