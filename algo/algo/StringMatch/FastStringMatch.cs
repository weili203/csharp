//
// Code Test: Text Matching Problem
// By Wei Li
//
// Description
// 1. Align the subtext to the text. Check the last character of the subtext
// 2. If the last symbol doesn't match, and if the corresponding symbol in the text is not present in the subtext,
//    the process can skip (subtext.Length) symbols, otherwise skip 1 symbol.
// 3. If the last symbol matches, then continue to search for symbol matches in the subtext from left to right
// 4. If a symbol in the text is not present in the subtext, it possible to skip N symbols (N depends on _lastSymbolShiftDistance).
//    _lastSymbolShiftDistance is calculated based on the distribution of the last symbol in the subtext
// 5. If a match is found, the process will move by one to the next symbol
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
    class Program
    {

        static void _Main(string[] args)
        {
            string text = "Polly put the kettle on, polly put the kettle on, polly put the kettle on we'll all have tea";
            string subtext = "PoLLy";
            string ret = FastStringMatch.MatchText(text, subtext);
            Console.WriteLine("**********************************************");
            Console.WriteLine("The input text: {0}\nSubtext: {1}\nOutput: {2}", text, subtext, ret);

            subtext = "polly";
            ret = FastStringMatch.MatchText(text, subtext);
            Console.WriteLine("\n**********************************************");
            Console.WriteLine("The input text: {0}\nSubtext: {1}\nOutput: {2}", text, subtext, ret);

            subtext = "ll";
            ret = FastStringMatch.MatchText(text, subtext);
            Console.WriteLine("\n**********************************************");
            Console.WriteLine("The input text: {0}\nSubtext: {1}\nOutput: {2}", text, subtext, ret);

            subtext = "Ll";
            ret = FastStringMatch.MatchText(text, subtext);
            Console.WriteLine("\n**********************************************");
            Console.WriteLine("The input text: {0}\nSubtext: {1}\nOutput: {2}", text, subtext, ret);

            subtext = "X";
            ret = FastStringMatch.MatchText(text, subtext);
            Console.WriteLine("\n**********************************************");
            Console.WriteLine("The input text: {0}\nSubtext: {1}\nOutput: {2}", text, subtext, ret);

            subtext = "Polx";
            ret = FastStringMatch.MatchText(text, subtext);
            Console.WriteLine("\n**********************************************");
            Console.WriteLine("The input text: {0}\nSubtext: {1}\nOutput: {2}", text, subtext, ret);

            subtext = "eTtLe ON";
            ret = FastStringMatch.MatchText(text, subtext);
            Console.WriteLine("\n**********************************************");
            Console.WriteLine("The input text: {0}\nSubtext: {1}\nOutput: {2}", text, subtext, ret);

            subtext = null;
            ret = FastStringMatch.MatchText(text, subtext);
            Console.WriteLine("\n**********************************************");
            Console.WriteLine("The input text: {0}\nSubtext: {1}\nOutput: {2}", text, subtext, ret);
        }
    }

    public class FastStringMatch
    {
        #region private fields
        private const int ALPHABET_SIZE = 256;    // ASCII codes
        private int[] _caseInsensitiveTable;
        private string _pattern;
        private int[] _possiblePatternSymbol;
        private int _lastSymbolShiftDistance;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pattern">subtext for search</param>
        private FastStringMatch(string subtext)
        {
            _pattern = subtext;
            _caseInsensitiveTable = preCreateCaseInsensitiveTable();
            _possiblePatternSymbol = CreatePossiblePatternSymbolsTable(subtext);
            _lastSymbolShiftDistance = CreateLastSymbolShiftDistance(subtext);
        }
        #endregion

        #region public static method
        /// <summary>
        /// Validate against NULL or empty string inputs. 
        /// Perform the text matching, and return all matches of the subtext in the input text, or "no matches".
        /// </summary>
        /// <param name="text">text to be searched</param>
        /// <param name="subText">pattern to search for</param>
        /// <returns>The character positions of the beginning of each match for the subtext within the text, or "no matches" if no match is found.</returns>
        public static string MatchText(string text, string subText)
        {
            if (String.IsNullOrEmpty(text) || String.IsNullOrEmpty(subText))
                return "null or empty string is not valid input";

            FastStringMatch ct = new FastStringMatch(subText);
            var res = ct.MatchAlgo(text);

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
        /// Return all matches of the pattern in specified text
        /// When comparing two letters, it will compare the values from lookup table _caseInsensitiveTable, in which
        /// lowercase letter and the corresponding uppercase letter have the same value.
        /// </summary>
        /// <param name="text">text to be searched</param>
        /// <param name="startingIndex">Index at which search begins</param>
        /// <returns>IEnumerable which returns the indexes of pattern matches</returns>
        private IEnumerable<int> MatchAlgo(string text)
        {
            int lastPatternSymbol = _pattern[_pattern.Length - 1];
            for(int i = _pattern.Length - 1; i <= text.Length - 1;)
            {
                if (_caseInsensitiveTable[lastPatternSymbol] == _caseInsensitiveTable[text[i]])
                {
                    // last character matched, then try to match the rest
                    // work from left to right in the pattern
                    int j = 0;
                    for (int skip = 0; j < _pattern.Length - 1; ++j)
                    {
                        if (_caseInsensitiveTable[_pattern[j]] != _caseInsensitiveTable[text[i - _pattern.Length + 1 + j]])
                        {
                            // check if the mismatched symbol is present in the pattern at all
                            if (_possiblePatternSymbol[text[i - _pattern.Length + 1 + j]] < 0)
                                skip = j + 1;   // not present
                            else
                                skip = 1;
                            // skip the maximum distance
                            i += Math.Max(_lastSymbolShiftDistance, skip);
                            break;
                        }
                    }
                    if (j >= _pattern.Length - 1)  
                    {
                        // find a match
                        i += 1;
                        yield return i - _pattern.Length + 1; 
                    }
                }
                else
                {
                    // check if the mismatched symbol is present in the pattern at all
                    if (_possiblePatternSymbol[text[i]] < 0)
                        i += _pattern.Length; // not present
                    else
                        i += 1;
                }
            }
        }

        #endregion

        #region private helper method
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

        private int[] CreatePossiblePatternSymbolsTable(string pattern)
        {
            int[] possiblePatternSymbols = new int[ALPHABET_SIZE];

            // initialize table
            for (int c = 0; c < possiblePatternSymbols.Length; ++c)
                possiblePatternSymbols[c] = -1;

            for (int i = 0; i < pattern.Length; ++i)
            {
                possiblePatternSymbols[pattern[i]] = 1;

                // try to set the opposite letter case with the same shift distance
                int d;
                if (IsAlpha(pattern[i], out d))
                    possiblePatternSymbols[d] = 1;
            }

            return possiblePatternSymbols;
        }

        private int CreateLastSymbolShiftDistance(string pattern)
        { 
            int lastSymbol = pattern[ pattern.Length - 1 ];
            int d = pattern.Length - 1;

            // try to look for the same symbol as the last one in the pattern from left to right
            for (int i = pattern.Length - 2; i >= 0; --i)
            {
              if (_caseInsensitiveTable[lastSymbol] == _caseInsensitiveTable[pattern[i]])
              {
                  d = pattern.Length - 1 - i;
                  break;
              }
            }

            return d;
        }

        #endregion

    }

     
}
