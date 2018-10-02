namespace CodingTheory.Utilities
{
    public static class MemoryOperations
    {
        // Į atmintį įdeda naują bit'a, o grąžina iš atminties išstumtą bit'ą
        public static bool InsertBitIntoMemory(bool newBit, bool[] memory)
        {
            bool droppedBit = memory[memory.Length - 1];

            // Prastumiam visus bit'us viena pozicija į priekį
            for (int i = (memory.Length - 1); i >= 1; i--)
                memory[i] = memory[i - 1];

            // Įdedam naują bit'ą į atmintį
            memory[0] = newBit;

            // Grąžinam iš atminties išstumtą bit'ą
            return droppedBit;
        }
    }
}
