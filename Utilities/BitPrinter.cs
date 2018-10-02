using System;
using System.IO;

namespace CodingTheory.Utilities
{
    // Bool elementų spausdinimas (true, false) verčiant (1, 0)
    public static class BitPrinter
    {
        public static void PrintBitArrayToConsole(bool[] bitArray)
        {
            foreach (var bit in bitArray)
            {
                switch (bit)
                {
                    case true:
                        Console.Write(1);
                        break;
                    case false:
                        Console.Write(0);
                        break;
                }
            }
        }

        public static void PrintBit(bool bit)
        {
            switch (bit)
            {
                case true:
                    Console.Write(1);
                    break;
                case false:
                    Console.Write(0);
                    break;
            }
        }

        public static void PrintBitArrayToTextFile(bool[] bitArray, string pathToSave)
        {
            using (FileStream fs = File.Create(pathToSave))
            {
                StreamWriter sw = new StreamWriter(fs);

                foreach (var bit in bitArray)
                {
                    switch (bit)
                    {
                        case true:
                            sw.Write(1);
                            break;
                        case false:
                            sw.Write(0);
                            break;
                    }
                }
            }
        }
    }
}
