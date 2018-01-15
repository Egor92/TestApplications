using System;
using System.Collections.Generic;
using System.Linq;
using EncryptionApp.Common;

namespace EncryptionApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var key = "OoXDykCPAmpAHpmDfefWcOghXAbCbjHP";
            var encode = Encryption.Encode("vsnbob", key);

            //string key = RandomStringHelper.GetWord(32, WordCase.Mixed);
            /*List<string> passwords = Enumerable.Range(0, 100).Select(x => RandomStringHelper.GetWord(5, 18, WordCase.Mixed)).ToList();
            var encodedValues = passwords.Select(x => Encode(x, key)).ToList();
            var decodedValues = encodedValues.Select(x => Decode(x, key)).ToList();

            for (int i = 0; i < passwords.Count; i++)
            {
                bool result = (passwords[i] == decodedValues[i]);
                Console.WriteLine("result: {0,5}, password: {1,10}, encoded: {2}, decoded {3}", result, passwords[i], encodedValues[i], decodedValues[i]);
            }*/
            Console.ReadLine();
        }

        private static string Encode(string text, string key)
        {
            //return AesEncryptor.Encrypt256(text, key);
            return Encryption.Encode(text, key);
        }

        private static string Decode(string hash, string key)
        {
            //return AesEncryptor.Decrypt256(hash, key);
            return Encryption.Decode(hash, key);
        }
    }
}