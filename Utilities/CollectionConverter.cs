using System.Collections;

namespace CodingTheory.Utilities
{
    public static class CollectionConverter
    {
        public static bool[] BitArrayToBoolArray(BitArray bitArray)
        {
            bool[] boolArray = new bool[bitArray.Length];
            int index = 0;

            foreach(var bit in bitArray)
            {
                boolArray[index] = (bool)bit;
                index++;
            }
            return boolArray;
        }

        public static bool[] StringToBoolArray(string binaryString)
        {
            if (IsLegalBinaryMessage(binaryString))
            {
                var boolArray = new bool[binaryString.Length];
                int index = 0;

                foreach(var bit in binaryString)
                {
                    switch(bit)
                    {
                        case '1':
                            boolArray[index] = true;
                            break;
                        case '0':
                            boolArray[index] = false;
                            break;
                    }
                    index++;
                }
                return boolArray;
            }
            else
                return null;
        }

        private static bool IsLegalBinaryMessage(string binaryMessage)
        {
            foreach (var ch in binaryMessage)
            {
                if (ch != '0' && ch != '1')
                    return false;
            }
            return true;
        }
    }
}
