/*
 * in place sorting algo
 */ 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace algo.Sorting.comparison_based
{
    #region Quick sort - O(nlgn)   divide-conquer-combine, most used in practice
    /*
     * Quicksort, like merge sort, applies the divide-and-conquer paradigm
     * 
     * Divide: Partition (rearrange) the array A[p .. r] into two (possibly empty) subarrays
        A[p . . q - 1] and A[q + 1 . . r] such that each element of A[p . .  q - 1] is
        less than or equal to A[q], which is, in turn, less than or equal to each element
        of A[q + 1 . .  r]. Compute the index q as part of this partitioning procedure.
     * 
     * Conquer: Sort the two subarrays A[p . . q - 1] and A[q + 1 . .  r] by recursive calls to quicksort.
     * 
     * Combine: Because the subarrays are already sorted, no work is needed to combine
        them: the entire array A[p . .  r] is now sorted.
     * 
     * The key part of quick sort is the Partition (to find the A[q], which meet A[p . .q- 1] <= A[q] <= A[q + 1 . .r])
     * 
     * QUICKSORT.A; p; r
           if  p < r
              q = PARTITION (A, p, r)
              QUICKSORT(A, p, q - 1)
              QUICKSORT(A, q + 1, r)
     */
    class QuickSort : SortBase
    {
        protected override void Sort()
        {
            //Recursive_QuickSort(_array, 0, _array.Count-1);
            Recursive_RandomnizedQuickSort(_array, 0, _array.Count - 1);
        }

        private void Recursive_QuickSort(List<int> A, int low, int high)
        {
            if (low >= high)
                return;

            // get the partition point
            int q = Partition(A, low, high);
            // recursive sort left/smaller half
            Recursive_QuickSort(A, low, q - 1);
            // recursive sort right/bigger half
            Recursive_QuickSort(A, q + 1, high);
        }

        private int Partition(List<int> A, int low, int high)
        {
            int x = A[high];
            int i = low - 1;

            for (int j = low; j <= high - 1; ++j)
            {
                if (A[j] <= x)
                {
                    i += 1;
                    Swap(A, i, j);
                }
            }

            Swap(A, i + 1, high);
            return i + 1;
        }

        public void TryPartition()
        {
            List<int> A = new List<int>() { 2, 8, 7, 1, 3, 5, 6, 4 };
            Print("-----before Partition ----", A);

            int d = Partition(A, 0, A.Count - 1);
            Print("-----after Partition ----", A);
            Console.WriteLine("the partition point is A[{0}] = {1}", d, A[d]);
        }

        private int RandomnizedPartition(List<int> A, int low, int high)
        {
            // pick a random pivot to archieve average-case
            Random rand = new Random();
            int n = rand.Next(high);
            // let the random position be the last one, which will be used as pivot in Partition
            Swap(A, high, n);

            return Partition(A, low, high);
        }

        private void Recursive_RandomnizedQuickSort(List<int> A, int low, int high)
        {
            if (low >= high)
                return;

            // get the partition point
            int q = RandomnizedPartition(A, low, high);
            // recursive sort left/smaller half
            Recursive_QuickSort(A, low, q - 1);
            // recursive sort right/bigger half
            Recursive_QuickSort(A, q + 1, high);
        }

    }
    #endregion

}
