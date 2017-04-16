using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace GeneticAlgorithm
{
    public class GeneticAlgorithm
    {
        double crossoverRate;
        double mutationRate;
        bool elitism;
        int populationSize;
        int numIterations;
        int digits;
        Random r = new Random();

        public GeneticAlgorithm(double crossoverRate, double mutationRate, bool elitism, int populationSize, int numIterations, int digits)
        {
            this.crossoverRate = crossoverRate;
            this.mutationRate = mutationRate;
            this.elitism = elitism;
            this.populationSize = populationSize;
            this.numIterations = numIterations;
            this.digits = digits;
        }

        public Ind Run()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            // initialize the first population
            var initialPopulation = Enumerable.Range(0, populationSize).Select(i => CreateIndividual()).ToArray();

            var currentPopulation = initialPopulation;

            for (int generation = 0; generation < numIterations; generation++)
            {
                // compute fitness of each individual in the population
                var fitnesses = Enumerable.Range(0, populationSize).Select(i => ComputeFitness(currentPopulation[i])).ToArray();

                var nextPopulation = new Ind[populationSize];

                // apply elitism
                int startIndex;
                if (elitism)
                {
                    startIndex = 1;
                    var populationWithFitness = currentPopulation.Select((individual, index) => new Tuple<Ind, double>(individual, fitnesses[index]));
                    var populationSorted = populationWithFitness.OrderByDescending(tuple => tuple.Item2); // item2 is the fitness
                    var bestIndividual = populationSorted.First();
                    nextPopulation[0] = bestIndividual.Item1;
                }
                else
                {
                    startIndex = 0;
                }

                // initialize the selection function given the current individuals and their fitnesses
                var getTwoParents = SelectTwoParents(currentPopulation, fitnesses);

                // create the individuals of the next generation
                for (int newInd = startIndex; newInd < populationSize; newInd++)
                {
                    // select two parents
                    var parents = SelectTwoParents(currentPopulation, fitnesses);

                    // do a crossover between the selected parents to generate two children (with a certain probability, crossover does not happen and the two parents are kept unchanged)
                    Tuple<Ind, Ind> offspring;
                    if (r.NextDouble() < crossoverRate)
                        offspring = Crossover(parents);
                    else
                        offspring = parents;

                    // save the two children in the next population (after mutation)
                    nextPopulation[newInd++] = Mutation(offspring.Item1, mutationRate);
                    if (newInd < populationSize) //there is still space for the second children inside the population
                        nextPopulation[newInd] = Mutation(offspring.Item2, mutationRate);
                }

                //the new population becomes the current one
                currentPopulation = nextPopulation;
            }

            //recompute the fitnesses on the final population and return the best individual
            var finalFitnesses = Enumerable.Range(0, populationSize).Select(i => ComputeFitness(currentPopulation[i])).ToArray();
            Console.WriteLine("Run Time: " + sw.Elapsed);
            Console.WriteLine("Average fitness of last population: " + finalFitnesses.Average());
            Console.WriteLine("Best fitness of last population: " + finalFitnesses.Max());

            return currentPopulation.Select((individual, index) => new Tuple<Ind, double>(individual, finalFitnesses[index])).OrderByDescending(tuple => tuple.Item2).First().Item1;
        }

        private Ind CreateIndividual() 
        {
            Ind newIndividual = new Ind(digits, r);
            return newIndividual;
        }

        private double ComputeFitness(Ind individual)
        {
            var bitsToInt = Convert.ToInt32(individual.bits, 2);
            var fitness = -(Math.Pow(bitsToInt, 2)) + (7 * bitsToInt);
            return fitness;
        }

        private Tuple<Ind,Ind> SelectTwoParents(Ind[] currentPopulation, double[] fitnesses)
        {
            var totalProbability = fitnesses.Sum();
            var probabilitySelection = Enumerable.Range(0, populationSize).Select(i => fitnesses[i]/totalProbability).ToArray();
            var currentProbability = 0.0;

            Ind parent1 = new Ind();
            Ind parent2 = new Ind();

            for (int i = 0; i < currentPopulation.Count(); i++) {
                currentProbability += probabilitySelection[i];
                if (currentProbability >= r.NextDouble()) {
                    parent1 = currentPopulation[i];
                }
                if (currentProbability >= r.NextDouble())
                {
                    parent2 = currentPopulation[i];
                }
            }

            var parents = new Tuple<Ind, Ind>(parent1, parent2);
            return parents;
        }

        //New child is made using singlepoint crossover
        private Tuple<Ind,Ind> Crossover(Tuple<Ind,Ind> parents)
        {
            var crossoverIndex = (digits / 2);
            Ind child1 = new Ind();
            Ind child2 = new Ind();

            child1.bits = parents.Item1.bits.Substring(0, crossoverIndex) + parents.Item2.bits.Substring((crossoverIndex), (digits - crossoverIndex));
            child2.bits = parents.Item2.bits.Substring(0, crossoverIndex) + parents.Item1.bits.Substring((crossoverIndex), (digits - crossoverIndex));

            var newChildren = new Tuple<Ind, Ind>(child1, child2);
            return newChildren;
        }

        private Ind Mutation(Ind child, double mutationRate) 
        {
            StringBuilder sbChild = new StringBuilder(child.bits);

            for (int i = 0; i < digits; i++) {
                if (r.NextDouble() < mutationRate) {
                    sbChild[i] = ('0' == child.bits[i] ? '1' : '0');
                }
            }

            child.bits = sbChild.ToString();
            return child;
        }
    }
}
