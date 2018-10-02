using System;

namespace CodingTheory.Utilities
{
    public static class RandomGenerator
    {
        private static readonly Random GetRandom = new Random();

        // Grąžina atsitiktinį realų skaičių intervale (0 ; 1)
        public static double GetRandomDouble()
        {
            lock (GetRandom)// Kad vienu metu nebūtų kelis kartus kviečiamas NextDouble() metodas. Tokiu atveju gautumėme vienodas reikšmes
            {
                return GetRandom.NextDouble();
            }
        }
    }
}
