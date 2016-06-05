//
// Code Test: Text Matching Problem
// By Wei Li
//
// Description
// The algorithm is based on Boyer-Moore algorithm, with the extra support for 
// case-insensitive matching. 
//
// Assumption
// It only supports ASCII codes as the alphabet.
// 
//

using System;
using System.Collections.Generic;
using System.Text;

namespace stringMatch
{
    class Test
    {
        static void _Main(string[] args)
        {
            string text = "Polly put the kettle on, polly put the kettle on, polly put the kettle on we'll all have tea";
            string subtext = "Polly";
            string ret = CodeTest.MatchText(text, subtext);
            Console.WriteLine("**********************************************");
            Console.WriteLine("The input text: {0}\nSubtext: {1}\nOutput: {2}", text, subtext, ret);

            subtext = "polly";
            ret = CodeTest.MatchText(text, subtext);
            Console.WriteLine("\n**********************************************");
            Console.WriteLine("The input text: {0}\nSubtext: {1}\nOutput: {2}", text, subtext, ret);

            subtext = "ll";
            ret = CodeTest.MatchText(text, subtext);
            Console.WriteLine("\n**********************************************");
            Console.WriteLine("The input text: {0}\nSubtext: {1}\nOutput: {2}", text, subtext, ret);

            subtext = "Ll";
            ret = CodeTest.MatchText(text, subtext);
            Console.WriteLine("\n**********************************************");
            Console.WriteLine("The input text: {0}\nSubtext: {1}\nOutput: {2}", text, subtext, ret);

            subtext = "X";
            ret = CodeTest.MatchText(text, subtext);
            Console.WriteLine("\n**********************************************");
            Console.WriteLine("The input text: {0}\nSubtext: {1}\nOutput: {2}", text, subtext, ret);

            subtext = "Polx";
            ret = CodeTest.MatchText(text, subtext);
            Console.WriteLine("\n**********************************************");
            Console.WriteLine("The input text: {0}\nSubtext: {1}\nOutput: {2}", text, subtext, ret);

            subtext = "eTtLe ON";
            ret = CodeTest.MatchText(text, subtext);
            Console.WriteLine("\n**********************************************");
            Console.WriteLine("The input text: {0}\nSubtext: {1}\nOutput: {2}", text, subtext, ret);

            subtext = null;
            ret = CodeTest.MatchText(text, subtext);
            Console.WriteLine("\n**********************************************");
            Console.WriteLine("The input text: {0}\nSubtext: {1}\nOutput: {2}", text, subtext, ret);
        }
    }

    public class CodeTest
    {
        #region private fields
        private int[] _badCharacterShift;
        private int[] _goodSuffixShift;
        private int[] _suffixes;
        private int[] _caseInsensitiveTable;
        private string _pattern;
        private const int ALPHABET_SIZE = 256;    // ASCII codes
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pattern">subtext for search</param>
        private CodeTest(string subtext)
        {
            _pattern = subtext;
            _caseInsensitiveTable = preCreateCaseInsensitiveTable();
            _badCharacterShift = preCreateBadCharacterShift(subtext);
            _suffixes = preCreateSuffixes(subtext);
            _goodSuffixShift = preCreateGoodSuffixShift(subtext, _suffixes);
        }
        #endregion

        #region public static method
        /// <summary>
        /// Validate against NULL or empty string inputs. 
        /// Perform the text matching, and return all matches of the subtext in the input text using the Boyer-Moore algorithm, or otherwise "no matches".
        /// </summary>
        /// <param name="text">text to be searched</param>
        /// <param name="subText">pattern to search for</param>
        /// <returns>The character positions of the beginning of each match for the subtext within the text, or "no matches" if no match is found.</returns>
        public static string MatchText(string text, string subText)
        {
            if (String.IsNullOrEmpty(text) || String.IsNullOrEmpty(subText))
                return "null or empty string is not valid input";

            CodeTest ct = new CodeTest(subText);
            var res = ct.BoyerMooreMatchAlgo(text);

            StringBuilder sb = new StringBuilder();
            foreach (var c in res)
            {
                sb.Append(c);
                sb.Append(", ");
            }

            string ret = "<no matches>";
            if (sb.Length > 0)
            {
                ret = sb.Remove(sb.Length - 2, 2).ToString();
            }

            return ret;
        }

        #endregion

        #region algorithm method
        /// <summary>
        /// Return all matches of the pattern in specified text using the Boyer-Moore algorithm
        /// When comparing two letters, it will compare the values from lookup table _caseInsensitiveTable, in which
        /// lowercase letter and the corresponding uppercase letter have the same value.
        /// </summary>
        /// <param name="text">text to be searched</param>
        /// <param name="startingIndex">Index at which search begins</param>
        /// <returns>IEnumerable which returns the indexes of pattern matches</returns>
        private IEnumerable<int> BoyerMooreMatchAlgo(string text)
        {
            int patternLength = _pattern.Length;
            int textLength = text.Length;

            int textIndex = 0;
            while (textIndex <= textLength - patternLength)
            {
                int patternIndex;
                for (patternIndex = patternLength - 1;
                     patternIndex >= 0 && (_caseInsensitiveTable[_pattern[patternIndex]] == _caseInsensitiveTable[text[patternIndex + textIndex]]);
                     --patternIndex)
                    ;

                if (patternIndex < 0)
                {
                    yield return textIndex + 1;
                    textIndex += _goodSuffixShift[0];
                }
                else
                    textIndex += Math.Max(_goodSuffixShift[patternIndex],
                      _badCharacterShift[text[textIndex + patternIndex]] - patternLength + 1 + patternIndex);
            }
        }

        #endregion

        #region private pre-processing methods
        /// <summary>
        /// Pre-processing: create the bad-character shift table.
        /// </summary>
        /// <param name="pattern">Pattern to search for</param>
        /// <returns>bad character shift table</returns>
        private int[] preCreateBadCharacterShift(string pattern)
        {
            int[] badCharacterShift = new int[ALPHABET_SIZE];

            // initialize table
            for (int c = 0; c < badCharacterShift.Length; ++c)
                badCharacterShift[c] = pattern.Length;

            for (int i = 0; i < pattern.Length - 1; ++i)
            {
                badCharacterShift[pattern[i]] = pattern.Length - i - 1;

                // try to set the opposite letter case with the same shift distance
                int d;
                if (IsAlpha(pattern[i], out d))
                    badCharacterShift[d] = pattern.Length - i - 1;
            }

            return badCharacterShift;
        }


        /// <summary>
        /// Pre-processing: find suffixes in the pattern
        /// </summary>
        /// <param name="pattern">Pattern search for</param>
        /// <returns>Suffix table</returns>
        private int[] preCreateSuffixes(string pattern)
        {
            int f = 0, g;

            int patternLength = pattern.Length;
            int[] suffixes = new int[pattern.Length + 1];

            suffixes[patternLength - 1] = patternLength;
            g = patternLength - 1;
            for (int i = patternLength - 2; i >= 0; --i)
            {
                if (i > g && suffixes[i + patternLength - 1 - f] < i - g)
                    suffixes[i] = suffixes[i + patternLength - 1 - f];
                else
                {
                    if (i < g)
                        g = i;
                    f = i;
                    while (g >= 0 && (_caseInsensitiveTable[pattern[g]] == _caseInsensitiveTable[pattern[g + patternLength - 1 - f]]))
                        --g;
                    suffixes[i] = f - g;
                }
            }

            return suffixes;
        }

        /// <summary>
        /// Pre-processing: create the good suffix table.
        /// </summary>
        /// <param name="pattern">Pattern to search for</param>
        /// <returns>Good suffix shift table</returns>
        private int[] preCreateGoodSuffixShift(string pattern, int[] suffixes)
        {
            int patternLength = pattern.Length;
            int[] goodSuffixShift = new int[pattern.Length + 1];

            for (int i = 0; i < patternLength; ++i)
                goodSuffixShift[i] = patternLength;
            int j = 0;
            for (int i = patternLength - 1; i >= -1; --i)
            {
                if (i == -1 || suffixes[i] == i + 1)
                {
                    for (; j < patternLength - 1 - i; ++j)
                        if (goodSuffixShift[j] == patternLength)
                            goodSuffixShift[j] = patternLength - 1 - i;
                }
            }

            for (int i = 0; i <= patternLength - 2; ++i)
                goodSuffixShift[patternLength - 1 - suffixes[i]] = patternLength - 1 - i;

            return goodSuffixShift;
        }

        /// <summary>
        /// Pre-processing: create a lookup table, such that lowercase letters and the 
        /// corresponding uppercase letters will have the same value in the table.  
        /// The table facilitates the case-insensitive comparison between two characters.
        /// </summary>
        /// <returns>The lookup table for case-insensitive comparison </returns>
        private int[] preCreateCaseInsensitiveTable()
        {
            int[] caseInsensitiveTable = new int[ALPHABET_SIZE];

            for (int i = 0; i < caseInsensitiveTable.Length; ++i)
            {
                if (i >= 97 && i <= 122)
                    caseInsensitiveTable[i] = i - 32;
                else
                    caseInsensitiveTable[i] = i;
            }

            return caseInsensitiveTable;
        }
        #endregion

        #region private helper method
        /// <summary>
        /// Return a boolean flag to indicate if parameter c is either an uppercase or a lowercase alphabetic letter.
        /// The character of opposite case is also returned via parameter 'oppositeCase'.
        /// </summary>
        /// <param name="c">Character to be checked</param>
        /// <param name="oppositeCase">The output character of opposite case</param>
        /// <returns>If parameter c is either an uppercase or a lowercase alphabetic letter.</returns>
        private bool IsAlpha(int c, out int oppositeCase)
        {
            // lower case
            if (c >= 65 && c <= 90)
            {
                oppositeCase = c + 32;
                return true;
            }

            // upper case
            if (c >= 97 && c <= 122)
            {
                oppositeCase = c - 32;
                return true;
            }

            // non alphabetic letter
            oppositeCase = c;
            return false;
        }

        #endregion

    }
}
