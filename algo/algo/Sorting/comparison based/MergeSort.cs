/*
 * in place sorting algo
 */ 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace algo.Sorting.comparison_based
{
    #region Merge sort - O(nlgn)   divide-conquer-combine
    /*  O(nlgn) -  divide-conquer-combine
     * - divide the problem into subproblems
     * - conquer each subproblems
     * - merge all the sorted subproblems to produce the final answer
     *   merge steps:  O(n)
     *   for example, there are two subset which are sorted,
     *   it always compares the top two which are the samllest,
     *   take the smaller one and place into the result buffer, until one set 
     *   is empty, then put the reamining set into the buffer
     * */
    class MergeSort : SortBase
    {
        protected override void Sort()
        {
            Recursive_MergeSort(_array, 0, _array.Count - 1);
        }

        public void TryCombineSortedSubArray()
        {
            List<int> array = new List<int>() { 2, 4, 5, 6, 1, 3, 5 };
            CombineSortedSubArray(array, 0, array.Count / 2, array.Count - 1);
            Print("-----after merge ----", array);
        }

        // array should contain two sorted subarrays, [low, mid] and [mid+1, high]
        private void CombineSortedSubArray(List<int> array, int low, int mid, int high)
        {
            // get the subarrays
            List<int> leftArray = array.GetRange(low, mid - low + 1);
            List<int> rightArray = array.GetRange(mid + 1, high - mid);

            // introduce an infinity number as the last one for the two subarrays
            // so that we dont need to check if any subarray is empty
            leftArray.Add(int.MaxValue);
            rightArray.Add(int.MaxValue);

            int l = 0, r = 0;
            for (int k = low; k <= high; ++k)
            {
                if (leftArray[l] <= rightArray[r])
                {
                    array[k] = leftArray[l];
                    l++;
                }
                else
                {
                    array[k] = rightArray[r];
                    r++;
                }
            }
        }

        private void Recursive_MergeSort(List<int> array, int low, int high)
        {
            if (low >= high)
                return;

            int mid = (low + high) / 2;

            Recursive_MergeSort(array, low, mid);

            Recursive_MergeSort(array, mid + 1, high);

            CombineSortedSubArray(array, low, mid, high);

        }
    }
    #endregion

}
