using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        //Func<Ind> CreateIndividual, Func<Ind,double> ComputeFitness, Func<Ind[],double[],Func<Tuple<Ind,Ind>>> SelectTwoParents, Func<Tuple<Ind, Ind>, Tuple<Ind, Ind>> Crossover, Func< Ind, double, Ind> Mutation
        public Ind Run()
        {
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
                    var parents = getTwoParents();

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

                // the new population becomes the current one
                //currentPopulation = nextPopulation;
                
                //Console.WriteLine("iteration: " + generation);

                //for (int i = 0; i < currentPopulation.Count(); i++) {
                //    Console.WriteLine(currentPopulation[i].bits);
                //    currentPopulation[i] = Mutation(currentPopulation[i], mutationRate);
                //}

                //in case it's needed, check here some convergence condition to terminate the generations loop earlier
            }

            //return currentPopulation.First();
            //recompute the fitnesses on the final population and return the best individual
            var finalFitnesses = Enumerable.Range(0, populationSize).Select(i => ComputeFitness(currentPopulation[i])).ToArray();
            return currentPopulation.Select((individual, index) => new Tuple<Ind, double>(individual, finalFitnesses[index])).OrderByDescending(tuple => tuple.Item2).First().Item1;
        }

        private Ind CreateIndividual() 
        {
            Ind newIndividual = new Ind(digits, r);
            return newIndividual;
        }

        private double ComputeFitness(Ind individual)
        {
            throw new NotImplementedException();
        }

        private Func<Tuple<Ind,Ind>> SelectTwoParents(Ind[] currentPopulation, double[] fitnesses)
        {
            throw new NotImplementedException();
        }

        private Tuple<Ind,Ind> Crossover(Tuple<Ind,Ind> parents)
        {
            throw new NotImplementedException();
        }

        private Ind Mutation(Ind child, double mutationRate) 
        {
            for (int i = 0; i < child.bits.Count(); i++) {
                if (mutationRate > r.NextDouble()) {
                    StringBuilder sb = new StringBuilder(child.bits);
                    sb[i] = ('0' == child.bits[i] ? '1' : '0');
                    child.bits = sb.ToString();
                }
            }
            return child;
        }

        /* FUNCTIONS TO DEFINE (for each problem):
        Func<Ind> createIndividual;                                 ==> input is nothing, output is a new individual
        Func<Ind,double> computeFitness;                            ==> input is one individual, output is its fitness
        Func<Ind[],double[],Func<Tuple<Ind,Ind>>> selectTwoParents; ==> input is an array of individuals (population) and an array of corresponding fitnesses, output is a function which (without any input) returns a tuple with two individuals (parents)
        Func<Tuple<Ind, Ind>, Tuple<Ind, Ind>> crossover;           ==> input is a tuple with two individuals (parents), output is a tuple with two individuals (offspring/children)
        Func<Ind, double, Ind> mutation;                            ==> input is one individual and mutation rate, output is the mutated individual
        */

    }
}
