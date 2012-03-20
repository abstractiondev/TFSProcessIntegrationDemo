using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Demo.Operations;

namespace DemoAppBeingBuilt
{
    class Program
    {
        static void Main(string[] args)
        {
            var parameters = new SortNumbersEvensBeforeOddsParameters();
            parameters.DataArray = new int[] {4, 7, 8, 12, 55, 23};
            parameters.AscendingOrder = true;
            var result = SortNumbersEvensBeforeOdds.Execute(parameters);
            var sortedArray = result.SortedArray;
            print10First(sortedArray);
            Console.WriteLine("Press enter to continue...");
            Console.ReadLine();
        }

        private static void print10First(int[] intArray)
        {
            string printStr =
                String.Join(", ", intArray.Take(10).Select(i => i.ToString()).ToArray());
            Console.WriteLine(printStr);
        }
    }
}
