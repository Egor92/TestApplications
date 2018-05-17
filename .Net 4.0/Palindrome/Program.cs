using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Palindrome
{
    public class Program
    {
        public static bool IsPalindrome(string word)
        {
            if (word.Length == 0)
                return true;

            for (int i = 0; i < word.Length / 2 + 1; i++)
            {
                char leftChar = Char.ToLower(word[i]);
                char rightChar = Char.ToLower(word[word.Length - 1 - i]);
                if (leftChar != rightChar)
                    return false;
            }
            return true;
        }

        public static void Main(string[] args)
        {
            Console.WriteLine(IsPalindrome("Deleveled"));
            Console.WriteLine(IsPalindrome(string.Empty));
            Console.WriteLine(IsPalindrome("a"));
            Console.WriteLine(IsPalindrome("aa"));
            Console.WriteLine(IsPalindrome("aaa"));
            Console.WriteLine(IsPalindrome("      "));
            Console.WriteLine(!IsPalindrome("afsgffrg"));
            Console.ReadLine();
        }
    }
}
