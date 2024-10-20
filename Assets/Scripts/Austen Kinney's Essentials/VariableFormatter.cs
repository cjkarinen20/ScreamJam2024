using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AustenKinney.Essentials
{
    public static class VariableFormatter
    {

        /// <summary>
        /// Converts an integer into an array of integers seperated by each digit.
        /// </summary>
        /// <param name="num">The integer to be converted into an array.</param>
        /// <returns>int[]</returns>
        public static int[] IntToIntArray(int num)
        {
            List<int> listOfInts = new List<int>();
            while (num > 0)
            {
                listOfInts.Add(num % 10);
                num = num / 10;
            }
            listOfInts.Reverse();
            return listOfInts.ToArray();
        }
    }
}
