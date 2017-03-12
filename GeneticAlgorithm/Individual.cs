using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithm
{
    public class Ind
    {
        public string bits;

        public Ind() {
        }

        public Ind(double size, Random r) {
            bits = Convert.ToString(r.Next(0, (int)Math.Pow(2, size) - 1), 2).PadLeft(5, '0');
        }
    }
}
