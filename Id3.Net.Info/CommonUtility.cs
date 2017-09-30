#region --- License & Copyright Notice ---
/*
Copyright (c) 2005-2012 Jeevan James
All rights reserved.

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/
#endregion

using System;

namespace Id3.Info
{
    public static partial class Utility
    {
        public static string UrlEncode(string str)
        {
            return Uri.EscapeDataString(str);
        }

        public static string EncodeSearchTermsForUrl(string searchTerms)
        {
            return UrlEncode(searchTerms.Replace(' ', '+'));
        }

        public static bool TolerantEquals(string s1, string s2, int tolerance)
        {
            if (tolerance == 0)
                return String.Compare(s1, s2, StringComparison.OrdinalIgnoreCase) == 0;
            return Levenshtein(s1, s2) <= tolerance;
        }

        private static int Levenshtein(string str1, string str2)
        {
            str1 = str1.ToLowerInvariant();
            str2 = str2.ToLowerInvariant();

            int len1 = str1.Length;
            int len2 = str2.Length;

            if (len1 == 0)
                return len2;
            if (len2 == 0)
                return len1;

            //Create matrix
            var matrix = new int[len1 + 1,len2 + 1]; // matrix
            for (int i = 0; i <= len1; matrix[i, 0] = i++)
                ;
            for (int j = 0; j <= len2; matrix[0, j] = j++)
                ;

            for (int i = 1; i <= len1; i++)
            {
                for (int j = 1; j <= len2; j++)
                {
                    int cost = (str2.Substring(j - 1, 1) == str1.Substring(i - 1, 1) ? 0 : 1);
                    matrix[i, j] = Math.Min(Math.Min(matrix[i - 1, j] + 1, matrix[i, j - 1] + 1), matrix[i - 1, j - 1] + cost);
                }
            }
            return matrix[len1, len2];
        }
    }
}