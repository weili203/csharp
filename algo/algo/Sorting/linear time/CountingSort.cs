/*
 * Limitations/Assumption:
 * the number to be sorted must be within a range [0 . . k], k is the max possible value
 * so that the value is suitable to be used as index in temp array C[].
 * 
 * NOT in-place sort, as output array is different from input
 * NOT recursive
 * 
 * arrays involved in the algo: 
 * A[] - original input
 * B[] - output 
 * C[] = temp storage. use A[i] as its index, its value is number of elements less than or equal to A[i]
 * 
 * 
 * algo:
 * store number of elements less than or equal to A[i] in C[A[i]], so just put A[i] at position C[A[i]] in output B[]
 * 
 * An important property of counting sort is that it is stable: numbers with the same
    value appear in the output array in the same order as they do in the input array.
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace algo.Sorting.linear_time
{
    class CountingSort : SortBase
    {
        protected override void Sort()
        {
            int max = _array.Max();
            List<int> B;
            CountSort(_array, max, out B);
            _array = B;
        }

        private void CountSort(List<int> A, int maxValue, out List<int> B)
        {
            // just to make B has the same number of elements as A
            B = GenerateZeroList(A.Count);

            // index and count array
            List<int> C = GenerateZeroList(maxValue+1); // max value is inclusive, so can have it as a index to C[]

            // C[i] now contains the number of elements equal to i (from A[]).
            foreach (int i in A)
                C[i]++;

            // C[i] now contains the number of elements less than or equal to i.
            for (int i = 1; i <= maxValue; ++i)
            {
                C[i] = C[i] + C[i - 1];
            }

            for (int j = A.Count - 1; j >= 0; --j)
            {
                B[C[A[j]]-1] = A[j];

                // make sure the elements with the same value will not be put at the same index in B[]
                C[A[j]]--; 
            }

        }

        private List<int> GenerateZeroList(int count)
        {
            List<int> ret = new List<int>();

            for (int i = 0; i < count; ++i )
            {
                ret.Add(0);
            }

            return ret;
        }
    }
}
