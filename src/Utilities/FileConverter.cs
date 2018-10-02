using System;
using System.Collections;
using System.IO;

namespace CodingTheory.Utilities
{
    public static class FileConverter
    {
        public static BitArray FileToBinary(string path)
        {
            BitArray bits = null;

            try
            {
                bits = new BitArray(File.ReadAllBytes(path));
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
            return bits;
        }

        public static void BinaryToFile(BitArray bits, string pathToSave)
        {
            try
            {
                byte[] bytes = new byte[((bits.Length - 1) / 8) + 1];
                bits.CopyTo(bytes, 0);

                using (BinaryWriter binaryWriter = new BinaryWriter(File.Open(pathToSave, FileMode.Create)))
                {
                    binaryWriter.Write(bytes);
                    binaryWriter.Close();
                }
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
