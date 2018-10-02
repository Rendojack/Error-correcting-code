using CodingTheory.Core;
using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using CodingTheory.Utilities;

namespace CodingTheory
{
    class Program
    {
        static double errorProbability;

        static ConvolutionalEncoder ce;
        static DirectDecoder dd;
        static NoisyChannel nc;

        static Stopwatch watch = null;

        static void Main(string[] args)
        {
            string stringInput;
            bool[] bitsReceived = null;

            Console.WriteLine("#################################################");
            Console.WriteLine("#-----------------------------------------------#");
            Console.WriteLine("#                                               #");
            Console.WriteLine("#   Convolutional encoder & Threshold decoder   #");
            Console.WriteLine("#                                               #");
            Console.WriteLine("#-----------------------------------------------#");
            Console.WriteLine("#################################################");
            Console.WriteLine();

            // Begalinis programos ciklas
            while (true)
            {
                try
                {
                    mainScreen:

                    Console.WriteLine("1    - Adjust error probability");
                    Console.WriteLine("2    - Send binary message");
                    Console.WriteLine("3    - Send .txt or .bmp");
                    Console.WriteLine("cd.. - Return");

                    Console.Write("\n");
                    Console.Write(">>> ");

                    stringInput = Console.ReadLine();
                    Console.Write("\n");

                    switch (stringInput)
                    {
                        // Klaidos tikimybės keitimas
                        case "1":
                            Console.WriteLine("Current error probability: " + errorProbability);
                            Console.WriteLine("Enter new probability [0 - 1]:");

                            double newErrorProbability;

                            do
                            {
                                Console.Write(">>> ");
                                stringInput = Console.ReadLine();

                                // Grįžimas į ankstesnį programos langą
                                if (stringInput.Equals("cd.."))
                                {
                                    Console.Write("\n");
                                    goto mainScreen;
                                }
                            }// Kartojam ciklą, kol įvestis nėra validi
                            while (!double.TryParse(stringInput, out newErrorProbability) || !ChangeErrorProbability(newErrorProbability));


                            Console.WriteLine("Probability updated successfully!\n");
                            break;

                        // Dvejetainio vektoriaus siuntimas kanalu
                        case "2":
                            Console.WriteLine("Enter new binary message:");

                            bool[] inputBits = null;

                            do
                            {
                                Console.Write(">>> ");
                                stringInput = Console.ReadLine();

                                // Grąžins null, jei įvestis nėra validi
                                inputBits = CollectionConverter.StringToBoolArray(stringInput);

                                // Grįžimas į ankstesnį programos langą
                                if (stringInput.Equals("cd.."))
                                {
                                    Console.Write("\n");
                                    goto mainScreen;
                                }
                            }// Kartojam ciklą, kol įvestis nėra validi
                            while (inputBits == null);

                            bitsReceived = TransmitBits(inputBits, true);

                            // Buvo duota komanda cd.. 
                            if (bitsReceived == null)
                                goto mainScreen;

                            var decoderStateBits = ArrayModifier.DeleteValues(bitsReceived, 6, bitsReceived.Length - 1);
                            var bitsReceivedWithoutDecoderStateBits = ArrayModifier.DeleteValues(bitsReceived, 0, 5);

                            Console.Write("\n");
                            Console.Write("Input message:    ");
                            BitPrinter.PrintBitArrayToConsole(inputBits);

                            Console.Write("\n");
                            Console.Write("Received message: ");
                            BitPrinter.PrintBitArrayToConsole(bitsReceivedWithoutDecoderStateBits);

                            Console.Write("\n");
                            PrintTransmissionResult(inputBits, bitsReceivedWithoutDecoderStateBits);
                            Console.Write("\n");

                            break;

                        // .txt ar .bmp failo siuntimas kanalu
                        case "3":
                            Console.WriteLine("Input file path: ");

                            do
                            {
                                Console.Write(">>> ");
                                stringInput = Console.ReadLine();

                                // Grįžimas į ankstesnį programos langą
                                if (stringInput.Equals("cd.."))
                                {
                                    Console.Write("\n");
                                    goto mainScreen;
                                }

                            }// Kartojam ciklą, kol įvestis nėra validi
                            while (!FileComparer.FileExtensionEquals(stringInput, ".txt") &&
                                    !FileComparer.FileExtensionEquals(stringInput, ".bmp"));

                            // Siunčiam failą kanalu
                            try
                            {
                                // Skaidom failą į bit'us
                                var fileBits = CollectionConverter.BitArrayToBoolArray(FileConverter.FileToBinary(stringInput));

                                // Siunčiam bit'us kanalu
                                bitsReceived = TransmitBits(fileBits, false);

                                // Trinam dekoderio būsenos bit'us (pirmus 6-is)
                                bitsReceivedWithoutDecoderStateBits = ArrayModifier.DeleteValues(bitsReceived, 0, 5);

                                // Sukuriam kelią, kur išsaugoti gautąjį failą ir kaip jį užvadinti
                                var outputFilePath = Environment.CurrentDirectory + "/receivedFileWithEncoding" + Path.GetExtension(stringInput);

                                // Konvertuojam gautus bit'us į failą
                                FileConverter.BinaryToFile(new BitArray(bitsReceivedWithoutDecoderStateBits), outputFilePath);

                                Console.Write("\n");
                                // Spausdinam siuntimo rodmenis ir rezultatus
                                PrintTransmissionResult(fileBits, bitsReceivedWithoutDecoderStateBits);
                                Console.WriteLine("File size: " + (fileBits.Length * 0.125) + " bytes");
                                Console.Write("\n");

                                // Siunčiam failo bitus be kodavimo
                                bool[] bitsReceivedWithoutEncoding = new bool[fileBits.Length];
                                int index = 0;
                                nc = new NoisyChannel(errorProbability);

                                foreach (var fileBit in fileBits)
                                {
                                    bitsReceivedWithoutEncoding[index] = nc.TransmitBit(fileBit);
                                    index++;
                                }

                                // Kelias iki failo, persiųsto kanalu nekoduojant
                                var outputFilePath2 = Environment.CurrentDirectory + "/receivedFileWithoutEncoding" + Path.GetExtension(stringInput);
                                // Išsaugom gautą failą, kuris buvo siųstas be kodavimo
                                FileConverter.BinaryToFile(new BitArray(bitsReceivedWithoutEncoding), outputFilePath2);

                                Console.WriteLine("Type O to open 2 files received from channel or type cd.. to return");
                                Console.WriteLine("File 1: /receivedFileWithoutEncoding" + Path.GetExtension(stringInput));
                                Console.WriteLine("File 2: /receivedFileWithEncoding" + Path.GetExtension(stringInput));

                                do
                                {                                
                                    Console.Write(">>> ");
                                    stringInput = Console.ReadLine();

                                    // Grįžimas į ankstesnį programos langą
                                    if (stringInput.Equals("cd.."))
                                    {
                                        Console.Write("\n");
                                        goto mainScreen;
                                    }

                                }// Kartojam ciklą, kol įvestis nėra validi
                                while (!stringInput.Equals("O"));

                                // Atidarom gautą failą, kuris buvo koduotas ir iškoduotas
                                Process.Start(outputFilePath);
                                // Atidarom gautą failą, kuri nebuvo koduotas
                                Process.Start(outputFilePath2);
                            }
                            catch
                            {
                                Console.WriteLine("Invalid file path");
                                break;
                            }
                            break;
                    }
                }
                catch
                {
                    Console.Write("\n");
                    Console.WriteLine("Unknown error occured... Invalid input? Try again");
                    Console.Write("\n");
                }
            }
        }

        // Bit'ų siuntimas nepatikimu kanalu
        // (bool)printIntermediateResults - ar reikia išvesti tarpinius rezultatus (encoded bits, transmitted bits)
        static bool[] TransmitBits(bool[] bits, bool printIntermediateResults)
        {
            // Pradedam skaičiuoti siuntimo trukmę
            watch = Stopwatch.StartNew();

            ce = new ConvolutionalEncoder();
            dd = new DirectDecoder();
            nc = new NoisyChannel(errorProbability);

            // Koduojam, pridėdami papildomu 6 0-ius bit'us
            var encodedBits = ce.Encode(ArrayModifier.AppendValues(new bool[] { false, false, false, false, false, false }, bits));

            // Atvejis siunčiamai žinutei iš konsolės, kai reikia atvaizduoti tarpinius siunčiamus rezultatus
            if (printIntermediateResults)
            {
                Console.Write("\n");
                Console.Write("Encoded bits:     ");
                BitPrinter.PrintBitArrayToConsole(encodedBits);
                Console.Write("\n");
            }

            // Siunčiam nepatikimu kanalu
            var transmittedBits = new bool[encodedBits.Length];
            int index = 0;

            foreach(var encodedBit in encodedBits)
            {
                transmittedBits[index] = nc.TransmitBit(encodedBit);
                index++;
            }

            // Atvejis siunčiamai žinutei iš konsolės, kai reikia atvaizduoti tarpinius siunčiamus rezultatus
            if (printIntermediateResults)
            {
                Console.Write("Transmitted bits: ");
                BitPrinter.PrintBitArrayToConsole(transmittedBits);
                Console.Write("\n");
                Console.Write("Errors occured:   " + nc.ErrorCount + "\n");

                Console.Write("\n");
                Console.WriteLine("Enter D to decode message, enter U to update first, then decode:");
                string stringInput;
                do
                {
                    Console.Write(">>> ");
                    stringInput = Console.ReadLine();
                    if (stringInput.Equals("cd.."))
                    {
                        Console.Write("\n");
                        return null;
                    }
                }
                while (!stringInput.Equals("D") && !stringInput.Equals("U"));

                switch (stringInput)
                {
                    case "D":
                        break;
                    case "U":
                        Console.WriteLine("Enter new updated message:");
                        bool[] inputBits;
                        do
                        {
                            Console.Write(">>> ");
                            stringInput = Console.ReadLine();

                            if (stringInput.Equals("cd.."))
                            {
                                Console.Write("\n");
                                return null;
                            }

                            // Grąžins null, jei įvestis nėra validi
                            inputBits = CollectionConverter.StringToBoolArray(stringInput);

                        }// Kartojam ciklą, kol įvestis nėra validi
                        while (inputBits == null);
                        transmittedBits = inputBits;
                        break;
                }
            }
            // Dekoduojam
            var decodedBits = dd.Decode(transmittedBits);

            // Stabdom siuntimo trukmės skaičiavimą
            watch.Stop();

            return decodedBits;
        }

        // Grąžina true, jei pakeitimas sėkmingas
        static bool ChangeErrorProbability(double newErrorProbability)
        {
            if (newErrorProbability >= 0 && newErrorProbability <= 1)
            {
                errorProbability = newErrorProbability;
                return true;
            }
            else
                return false;
        }

        static void PrintTransmissionResult(bool[] inputBits, bool[] outputBits)
        {
            var bitDifferenceCount = FileComparer.BitDifferenceCount(inputBits, outputBits);

            Console.WriteLine("Error probability " + errorProbability);
            Console.WriteLine("Errors left unfixed: " + bitDifferenceCount);
            Console.WriteLine("File transmission time: " + watch.ElapsedMilliseconds + "ms");
        }
    }
}
