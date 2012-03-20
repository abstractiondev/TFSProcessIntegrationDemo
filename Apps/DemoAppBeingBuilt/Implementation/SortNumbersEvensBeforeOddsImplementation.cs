using System;
using System.Collections.Generic;
using System.Linq;

namespace Demo.Operations
{
    internal static class SortNumbersEvensBeforeOddsImplementation
    {
        public static void ParameterValidation_DataArrayNotNull_ThrowsException(int[] dataArray)
        {
            if (dataArray == null)
                throw new ArgumentNullException("dataArray");
        }

        public static int[] GetTarget_Workingset(int[] dataArray)
        {
            return dataArray.ToArray();
        }

        internal class OrderedComparer : Comparer<int>
        {
            private bool AscendingOrder;

            public OrderedComparer(bool ascendingOrder)
            {
                AscendingOrder = ascendingOrder;
            }

            public override int Compare(int x, int y)
            {
                return AscendingOrder ? Comparer<int>.Default.Compare(x, y) : Comparer<int>.Default.Compare(y, x);
            }
        }

        internal class EvensBeforeOddsComparer : Comparer<int>
        {
            public override int Compare(int x, int y)
            {
                bool xIsEven = x % 2 == 0;
                bool yIsEven = y % 2 == 0;

                if (xIsEven == yIsEven)
                    return 0;
                if (xIsEven)
                    return -1;
                return 1;
            }
        }

        public static SortNumbersEvensBeforeOddsReturnValue Get_ReturnValue(int[] workingset)
        {
            return new SortNumbersEvensBeforeOddsReturnValue { SortedArray = workingset };
        }

        public static int[] ExecuteMethod_SortNumbers(int[] dataArray, bool ascendingOrder)
        {
            OrderedComparer orderedComparer = new OrderedComparer(ascendingOrder);
            return dataArray.OrderBy(i => i, orderedComparer).ToArray();
        }

        public static int[] ExecuteMethod_SortEvensBeforeOdds(int[] sortNumbersOutput)
        {
            EvensBeforeOddsComparer evensBeforeOddsComparer = new EvensBeforeOddsComparer();
            return sortNumbersOutput.OrderBy(i => i, evensBeforeOddsComparer).ToArray();
        }
    }
}