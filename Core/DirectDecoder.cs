using CodingTheory.Utilities;
using System;

namespace CodingTheory.Core
{
    // Tiesioginis dekoderis
    public class DirectDecoder
    {
        public bool[] RegularMemory { get; private set; }
        public bool[] SyndromeMemory { get; private set; }

        public const int m = 6;// Atminties blokai

        public DirectDecoder()
        {
            RegularMemory = new bool[m];
            SyndromeMemory = new bool[m];
        }

        public bool[] Decode(bool[] vector)
        {
            // Patikrinam ar visi vektoriaus bit'ai porose
            if (vector.Length % 2 == 0)
            {
                bool[] decodedVector = new bool[vector.Length / 2];
                int lastDecodedVectorIndex = 0;

                // Einam per vektorių, indeksą didindami po 2
                for (int i = 0; i < vector.Length; i += 2)
                {
                    var firstInputBit = vector[i];
                    var secondInputBit = vector[i + 1];

                    var droppedBitFromRegularMemory = MemoryOperations.InsertBitIntoMemory(firstInputBit, RegularMemory);
                    var syndromeBit = GetSyndromeBit(droppedBitFromRegularMemory, secondInputBit);
                    var decodedBit = LogicGate.XOR(new bool[] { droppedBitFromRegularMemory, syndromeBit });

                    decodedVector[lastDecodedVectorIndex] = decodedBit;
                    lastDecodedVectorIndex++;
                }
                return decodedVector;
            } 
            else
                throw new Exception("Vector length must be even number");
        }

        // Metodas sindromo bit'ui gauti
        private bool GetSyndromeBit(bool droppedBitFromRegularMemory, bool secondInputBit)
        {
            var syndromeMemoryInputBit = LogicGate.XOR(new bool[] { secondInputBit, RegularMemory[0], RegularMemory[2], RegularMemory[5], droppedBitFromRegularMemory });
            var droppedBitFromSyndromeMemory = MemoryOperations.InsertBitIntoMemory(syndromeMemoryInputBit, SyndromeMemory);

            return LogicGate.MDE(new bool[] { syndromeMemoryInputBit, SyndromeMemory[1], SyndromeMemory[4], droppedBitFromSyndromeMemory });
        }
    }
}
