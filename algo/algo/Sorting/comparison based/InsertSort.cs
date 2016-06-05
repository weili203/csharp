/*
 * in-place sorting algo, as the output is the input array, just rearrange the ordering of the elements
 */ 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace algo.Sorting.comparison_based
{
    #region Insert sort - O(n^2)   brute force
    class InsertSort : SortBase
    {
        /**  O(n^2) - incremental approach
         * work from left to right 
         * a number on the right is used to compare with all the numbers left to it
         * the number will be inserted at the position which its left is smaller, and 
         * the right is bigger
         */
        protected override void Sort()
        {
            for (int current = 1; current < _array.Count; ++current)
            {
                // version 1 
                // all the element on current's left
                //int j = current - 1;
                //for (; j > -1 && (_array[current] < _array[j]); --j)
                //    ;

                // version 2
                // as the left numbers are sorted, so if it find one smaller one,
                // current one will be inserted to that position
                int j = 0;
                for (; j < current; ++j)
                {
                    if (_array[current] < _array[j])
                        break;
                }

                int temp = _array[current];
                _array.RemoveAt(current);
                _array.Insert(j, temp);
            }

        }
    }
    #endregion

}
