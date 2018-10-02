using CodingTheory.Utilities;
using System;

namespace CodingTheory.Core
{
    // Nepatikimas kanalas
    public class NoisyChannel
    {
        // Klaidos kanale tikimybė
        private readonly double ErrorProbability;
        // Padarytų klaidų kiekis
        public int ErrorCount { get; private set; } = 0;

        public NoisyChannel(double errorProbability)
        {
            // Jei klaidos tikimybė intervale [0 ; 1]
            if (errorProbability >= 0 && errorProbability <= 1)
                ErrorProbability = errorProbability;
            else
                throw new ArgumentException("Probability must be between 0 and 1");
        }

        // Bit'o siuntimas kanalu
        public bool TransmitBit(bool bit)
        {
            if (DistortBit())
            {
                ErrorCount++;
                return !bit;
            }             
            else
                return bit;      
        }

        // Ar iškraipyti bit'ą?
        private bool DistortBit()
        {
            // Jei atsitiktinis realusis skaičius intervale (0 ; 1) mažesnis už klaidos tikimybę - iškraipom bit'ą
            if (RandomGenerator.GetRandomDouble() < ErrorProbability)
                return true;    
            else
                return false;
        }
    }
}
