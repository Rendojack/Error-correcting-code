using System;

namespace CodingTheory.Utilities
{
    // Generinė klasė, skirta atlikti veiksmams su masyvais
    public static class ArrayModifier
    {
        // Elementų, esančių intervale [starIndex; endIndex], trinimas
        public static T[] DeleteValues<T>(T[] array, int startIndex, int endIndex)
        {
            // Tikrinam argumentų tinkamumą
            if (startIndex < endIndex &&
                    startIndex >= 0 &&
                        !(endIndex > array.Length - 1))
            {
                T[] newArray = new T[array.Length - 1 - (endIndex - startIndex)];
                int newArrayIndex = 0;

                bool isDeletingValues = false;

                for(int i=0; i<array.Length; i++)
                {
                    // Randam startIndex ir pasižymime, kaip trinimo pradžią
                    if (i == startIndex)
                        isDeletingValues = true;
                    else if (i == endIndex)// Nutraukiam trinimą
                    {
                        isDeletingValues = false;
                        continue;
                    }

                    // Trinam == nerašom elemento į naują masyvą
                    if (isDeletingValues)
                    {
                        continue;
                    }
                    else// Visus kitus rašom
                    {
                        newArray[newArrayIndex] = array[i];
                        newArrayIndex++;
                    }
                }
                return newArray;
            }
            else
                throw new ArgumentException("Invalid arguments");
        }

        // Naujų elementų prijungimas į masyvo pabaigą
        public static T[] AppendValues<T>(T[] values, T[] array)
        {
            T[] newArray = new T[array.Length + values.Length];
            int index = 0;

            foreach (var bit in array)
            {
                newArray[index] = bit;
                index++;
            }

            foreach (var value in values)
            {
                newArray[index] = value;
                index++;
            }
            return newArray;
        }
    }
}
