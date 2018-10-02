using System;
using System.Linq;

namespace CodingTheory.Utilities
{
    // Loginės operacijos su bit'ais
    public static class LogicGate
    {
        // Grąžina true, jei true kiekis nelyginis
        public static bool XOR(bool[] bits)
        {
            int countTrues = bits.Where(bit => bit.Equals(true)).Count();

            if (countTrues % 2 != 0)
                return true;
            else
                return false;
        }

        // Major-decision element
        // Grąžina true, jei true yra nuo 2 iki 4
        public static bool MDE(bool[] bits)
        {
            if (bits.Length == 4)
            {
                int countTrues = bits.Where(bit => bit.Equals(true)).Count();
                if (Enumerable.Range(2, 4).Contains(countTrues))
                    return true;
                else
                    return false;
            }
            else
                throw new Exception("Input bits count must be 4");
        }
    }
}
