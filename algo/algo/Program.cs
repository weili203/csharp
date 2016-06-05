using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using algo.divide_conquer_combine;
using algo.Sorting.comparison_based;
using algo.Sorting.linear_time;
using algo.Selection;
using System.Text.RegularExpressions;


namespace algo
{
    class Program
    {
        static void Main(string[] args)
        {

            bool b = Regex.IsMatch("0x0001", "^0x[0-9a-fA-F]{4}$");

            b = Regex.IsMatch("0x0010001", "^0x[0-9a-fA-F]{4}([0-9a-fA-F]{4})?$");

            b = Regex.IsMatch("0x00010001", "^0x[0-9a-fA-F]{4|8}$");

            b = Regex.IsMatch("0x00010001", "^0x([0-9a-fA-F]{4}|[0-9a-fA-F]{8})$");

            b = Regex.IsMatch("0x0001", "^0x([0-9a-fA-F]{4}|[0-9a-fA-F]{8})$");

            b = Regex.IsMatch("0x0001001", "^0x([0-9a-fA-F]{4}|[0-9a-fA-F]{8})$");

            b = Regex.IsMatch("0x001", "^0x([0-9a-fA-F]{4}|[0-9a-fA-F]{8})$");



        }

        #region
        private static void TrySort()
        {          
            SortBase sort = new InsertSort();
            sort.Print("-------- before insert sort ----------");
            sort.DoSort();
            sort.Print("-------- after insert sort ----------");

            SortBase ms = new MergeSort();
            ms.Print("-------- before merge sort ----------");
            ms.DoSort();
            ms.Print("-------- after merge sort ----------");

            SortBase hs = new HeapSort();
            hs.Print("-------- before heap sort ----------");
            hs.DoSort();
            hs.Print("-------- after heap sort ----------");

            QuickSort qs = new QuickSort();
            qs.Print("-------- before quick sort ----------");
            qs.DoSort();
            qs.Print("-------- after quick sort ----------");

            CountingSort cs = new CountingSort();
            cs.Print("-------- before quick sort ----------");
            cs.DoSort();
            cs.Print("-------- after quick sort ----------");
        }

        private static void TryDivideAndConquer()
        {
            Console.WriteLine("-----find max crossing subarray-----");
            Max_Subarray ms = new Max_Subarray();
            ms.TryFindMaxCrossingSubArray();

            Console.WriteLine("-----find max subarray-----");
            ms.TryFind_Max_Subarray();
        }

        private static void TrySelection()
        {
            Console.WriteLine("-----get the n-th smallest number-----");
            SelectNthMin.Try_SelectNSmallest();
        }

        private static void TryBinarySearchTree()
        {
            BinarySearchTree.TryDelete();
        }
        #endregion
    }

}
