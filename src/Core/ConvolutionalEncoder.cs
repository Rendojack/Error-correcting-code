using CodingTheory.Utilities;
using System;
using System.Linq;

namespace CodingTheory.Core
{
    // Sąsūkos (konvoliucinis) enkoderis
    public class ConvolutionalEncoder
    {
        public bool[] Memory { get; private set; }

        public const int k = 1;// įeities bit'ų kiekis
        public const int n = 2;// išeities bit'ų kiekis
        public const int m = 6;// atminties blokų kiekis

        public ConvolutionalEncoder() { Memory = new bool[m]; }
   
        public bool[] Encode(bool[] vector)
        {
            bool[] encodedVector = new bool[vector.Length * 2];
            int lastEncodedVectorIndex = 0;

            if (vector.Count() >= 1)
            {
                // Koduojam...
                foreach (var inputBit in vector)
                {
                    // Pirmas iš enkoderio išeinantis bit'as
                    bool firstOutputBit = inputBit;

                    // Iš atminties išstumtas bit'as
                    bool droppedBit = MemoryOperations.InsertBitIntoMemory(inputBit, Memory);

                    // Antras iš enkoderio išeinantis bit'as
                    bool secondOutputBit = LogicGate.XOR(new bool[] { firstOutputBit, Memory[2], Memory[5], droppedBit });

                    encodedVector[lastEncodedVectorIndex] = firstOutputBit;
                    encodedVector[lastEncodedVectorIndex + 1] = secondOutputBit;
                    lastEncodedVectorIndex += 2;
                }
                return encodedVector;
            }
            else
                throw new Exception("Invalid vector size");
        }     
    }
}
