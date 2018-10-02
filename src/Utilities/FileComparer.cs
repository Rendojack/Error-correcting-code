using System;
using System.IO;
using System.Linq;

namespace CodingTheory.Utilities
{
    public static class FileComparer
    {
        public static bool FilesAreEqual(string path1, string path2)
        {
            return (new FileInfo(path1).Length == new FileInfo(path2).Length && File.ReadAllBytes(path1).SequenceEqual(File.ReadAllBytes(path2)));
        }

        public static bool FileExtensionEquals(string path, string extension)
        {
            try
            {
                var ext = Path.GetExtension(path);
                return extension.Equals(ext);
            }
            catch
            {
                return false;
            }
        }

        public static int BitDifferenceCount(bool[] input1, bool[] input2)
        {
            if (input1.Length == input2.Length)
            {
                int count = 0;

                for (int i = 0; i < input1.Length; i++)
                {
                    if (input1[i] != input2[i])
                        count++;
                }
                return count;
            }
            else
                throw new Exception("Input1 & input2 length must equal");
        }

        // Overload
        public static int BitDifferenceCount(string path1, string path2)
        {
            var bits1 = CollectionConverter.BitArrayToBoolArray(FileConverter.FileToBinary(path1));
            var bits2 = CollectionConverter.BitArrayToBoolArray(FileConverter.FileToBinary(path2));

            return BitDifferenceCount(bits1, bits2);
        }
    }
}
