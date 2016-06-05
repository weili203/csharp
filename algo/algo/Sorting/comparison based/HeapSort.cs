/*
 * in place sorting algo
 */ 
using System;
using System.Collections.Generic;

namespace algo.Sorting.comparison_based
{
    #region Heap sort - O(nlgn)
    /*
     * Heap is a data structure. It's an array which reflects a binary tree. 
     * Max heap: the value of a node in the binary tree is not greater than its parent.  A[PARENT(i)] >= A[i]
     * Min heap: the value of a node in the binary tree is not smaller than its parent.  A[PARENT(i)] <= A[i]
     * 
     * An array A that represents a heap is an object with two attributes:
        A:length, which (as usual) gives the number of elements in the array, 
        A:heap-size, which represents how many elements in the heap are stored within array A, the number of nodes on the tree
           heap-size is not greater than array length, and it will be reduce as processing continues.
     * 
     * The binary tree is filled from the left to right, and from the root to bottom
     * So the root is stored as the first element in the array, and then the chiildren of the root
     * Example: max heap
     * 
     * index  0   1   2   3  4  5  6  7  8  9
     * value 16 14  10  8  7  9  3  2  4  1
     * 
     * binary tree
     *                                             16
     *                               |                         |
     *                              14                       10
     *                        |             |            |          |
     *                       8              7            9          3
     *                   |       |      |
     *                   2       4      1
     * 
     * So it's easy to work out the index of node i's parent, left child, right child.
     *                      0-based index               1-based index
     * PARENT(i) =         i  / 2                           i / 2
     * LEFT(i)     =         i  * 2 + 1                     i * 2
     * RIGHT(i)   =       (i + 1) * 2                     i * 2 + 1        
     * 
     * The height of the tree is lgN
     */
    class Heap
    {
        private List<int> _A;
        public Heap(List<int> A)
        {
            _A = A;
            HeapSize = A.Count;
        }

        public int HeapSize
        { get; set; }

        public List<int> Array
        {
            get { return _A; }
        }

        #region Max_Heapify - O(lgN)
        // maintain the max heap property - O(lgN)
        public static void Max_Heapify(List<int> A, int index)
        {
            int left = LeftChildIndex(index);
            int right = RightChildIndex(index);
            int largestIndex = index;

            if ((left < A.Count) && (A[left] > A[largestIndex]))
                largestIndex = left;

            if (((right < A.Count) && (A[right] > A[largestIndex])))
                largestIndex = right;

            if (largestIndex != index)
            {
                SwapValue(A, largestIndex, index);
                Max_Heapify(A, largestIndex);
            }
        }

        public void Max_Heapify(int index)
        {
            int left = LeftChildIndex(index);
            int right = RightChildIndex(index);
            int largestIndex = index;

            if ((left < HeapSize) && (_A[left] > _A[largestIndex]))
                largestIndex = left;

            if (((right < HeapSize) && (_A[right] > _A[largestIndex])))
                largestIndex = right;

            if (largestIndex != index)
            {
                SwapValue(largestIndex, index);
                Max_Heapify(largestIndex);
            }
        }

        public static void Try_Max_Heapify()
        {
            List<int> A = new List<int>() { 16, 4, 10, 14, 7, 9, 3, 2, 8, 1 }; //{ 16, 14,  10,  8,  7,  9,  3,  2,  4,  1}
            Console.WriteLine("-----before max heapify--------");
            foreach (int i in A)
                Console.Write(i + " ");

            Max_Heapify(A, 1);

            Console.WriteLine("\n-----after max heapify--------");
            foreach (int i in A)
                Console.Write(i + " ");
        }
        #endregion

        #region Build Max Heap - O(NlgN)
        /*
         * process from A.Length/2 down to 0
         * As inside Max_Heapify, index will be multiplied by 2, 
         * so the element on the right will be re-evaluate for each loop
         * 
         * Important thing about this is that, it gurantee that the root is the max value in the array
         */
        public static void BuildMaxHeap(List<int> A)
        {
            for (int i = A.Count / 2; i >= 0; --i)
            {
                Max_Heapify(A, i);
            }

        }

        public void BuildMaxHeap()
        {
            HeapSize = _A.Count;
            for (int i = HeapSize / 2; i >= 0; --i)
            {
                Max_Heapify(i);
            }
        }

        public static void Try_BuildMaxHeap()
        {
            List<int> A = new List<int>() { 4, 1, 3, 2, 16, 9, 10, 14, 8, 7 }; //{ 16, 14,  10,  8,  7,  9,  3,  2,  4,  1}
            Console.WriteLine("-----before max heapify--------");
            foreach (int i in A)
                Console.Write(i + " ");

            BuildMaxHeap(A);

            Console.WriteLine("\n-----after max heapify--------");
            foreach (int i in A)
                Console.Write(i + " ");
        }

        public void SwapValue(int a, int b)
        {
            SwapValue(_A, a, b);
        }
        #endregion

        #region private methods
        private static int LeftChildIndex(int i)
        {
            return (i << 1) + 1;
        }

        private static int RightChildIndex(int i)
        {
            return (i + 1) << 1;
        }

        private static int ParentIndex(int i)
        {
            return i >> 1;
        }

        private static void SwapValue(List<int> A, int a, int b)
        {
            int temp = A[a];
            A[a] = A[b];
            A[b] = temp;
        }

        #endregion

    }

    class HeapSort : SortBase
    {
        private Heap _heap;//= new Heap(_array);

        public HeapSort()
        {
            _heap = new Heap(_array);
        }

        protected override void Sort()
        {
            // just make sure the root (first element A[0]) is the max value
            _heap.BuildMaxHeap();

            for (int i = _array.Count - 1; i >= 1; --i)
            {
                // as the first element is the max at each loop
                // swap it to the end of the array
                // it also invalidate the property of the max heap at the first element
                _heap.SwapValue(0, i);

                // do not swap the already sorted parts
                _heap.HeapSize -= 1;

                // restore the first element's max property
                _heap.Max_Heapify(0);
            }
        }
    }

    #endregion

}
