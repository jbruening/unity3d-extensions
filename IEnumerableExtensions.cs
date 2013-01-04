using System;
using System.Collections.Generic;

namespace UEx
{
    public static class IEnumerableExtensions
    {
        public static T MaxElement<T, TCompare>(this IEnumerable<T> collection, Func<T, TCompare> func)
        where TCompare : IComparable<TCompare>
        {
            T maxItem = default(T);
            TCompare maxValue = default(TCompare);

            if (collection == null)
                return maxItem;

            foreach (var item in collection)
            {
                TCompare temp = func(item);

                if (maxItem == null || temp.CompareTo(maxValue) > 0)
                {
                    maxValue = temp;
                    maxItem = item;
                }

            }
            return maxItem;
        }

        public static T[] RemoveRange<T>(this T[] array, int index, int count)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException("count", " is out of range");
            if (index < 0 || index > array.Length - 1)
                throw new ArgumentOutOfRangeException("index", " is out of range");

            if (array.Length - count - index < 0)
                throw new ArgumentException("index and count do not denote a valid range of elements in the array", "");

            var newArray = new T[array.Length - count];

            for (int i = 0, ni = 0; i < array.Length; i++)
            {
                if (i < index || i >= index + count)
                {
                    newArray[ni] = array[i];
                    ni++;
                }
            }

            return newArray;
        }
    }
}
