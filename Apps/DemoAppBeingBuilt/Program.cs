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
            var okResult = SortNumbersEvensBeforeOdds.Execute(
                new SortNumbersEvensBeforeOddsParameters()
                {
                    AscendingOrder = false,
                    DataArray = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }
                });
            int[] okData = okResult.SortedArray;
            print10First(okData);

            var supposedToFailPerfReq = SortNumbersEvensBeforeOdds.Execute(
                new SortNumbersEvensBeforeOddsParameters()
                {
                    AscendingOrder = false,
                    DataArray = new int[5000000]
                });
            int[] notSupposedToGetHere = supposedToFailPerfReq.SortedArray;
            print10First(notSupposedToGetHere);
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
