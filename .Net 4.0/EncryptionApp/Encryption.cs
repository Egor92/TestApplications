using System;
using System.Collections.Generic;
using System.Text;

namespace EncryptionApp
{
    public static class Encryption
    {
        private static readonly byte[,] Sblocks =
        {
            { 4, 10, 9, 2, 13, 8, 0, 14, 6, 11, 1, 12, 7, 15, 5, 3 },
            { 14, 11, 4, 12, 6, 13, 15, 10, 2, 3, 8, 1, 0, 7, 5, 9 },
            { 5, 8, 1, 13, 10, 3, 4, 2, 14, 15, 12, 7, 6, 0, 9, 11 },
            { 7, 13, 10, 1, 0, 8, 9, 15, 14, 4, 6, 12, 11, 2, 5, 3 },
            { 6, 12, 7, 1, 5, 15, 13, 8, 4, 10, 9, 14, 0, 3, 11, 2 },
            { 4, 11, 10, 0, 7, 2, 1, 13, 3, 6, 8, 5, 9, 12, 15, 14 },
            { 13, 11, 4, 1, 3, 15, 5, 9, 0, 10, 14, 7, 6, 8, 2, 12 },
            { 1, 15, 13, 0, 5, 7, 10, 4, 9, 2, 3, 14, 6, 11, 8, 12 }
        }; //Блок замен

        private static bool Mod64(int length) //Проверка делится ли открытый текст нацело на 64 бита
        {
            return (length % 8) == 0;
        }

        private static List<UInt64> FillingListData(string data) //Возвращает список из 64-х битных блоков данных
        {
            List<UInt64> resultList = new List<UInt64>();
            byte[] temp = Encoding.Default.GetBytes(data);
            int startIndex = 0;
            while (startIndex < temp.Length)
            {
                resultList.Add(BitConverter.ToUInt64(temp, startIndex));
                startIndex += 8;
            }
            return resultList;
        }

        private static List<UInt32> FillingListKey(string key)
        {
            List<UInt32> resultList = new List<UInt32>();
            byte[] temp = Encoding.Default.GetBytes(key);
            resultList.Add(BitConverter.ToUInt32(temp, 0));
            resultList.Add(BitConverter.ToUInt32(temp, 4));
            resultList.Add(BitConverter.ToUInt32(temp, 8));
            resultList.Add(BitConverter.ToUInt32(temp, 12));
            resultList.Add(BitConverter.ToUInt32(temp, 16));
            resultList.Add(BitConverter.ToUInt32(temp, 20));
            resultList.Add(BitConverter.ToUInt32(temp, 24));
            resultList.Add(BitConverter.ToUInt32(temp, 28));
            return resultList;
        }

        //Возвращает список из 32-х битных блоков ключа

        private static string GetPartString(UInt64 partText)
        {
            byte[] temp = BitConverter.GetBytes(partText);
            string result = Encoding.Default.GetString(temp);
            return result;
        }

        //Возврашает часть зашифрованной/расшифрованной строки

        private static UInt32 Mod2_32(UInt32 a, UInt32 b)
        {
            UInt32 result = a + b;
            return result;
        }

        // Сложение по модулю 2^32

        private static UInt32 ShiftN(UInt32 num, int n)
        {
            UInt32 c = num;
            for (int i = 0; i < n; ++i)
            {
                //UInt32 temp = (UInt32)(num / Convert.ToUInt32(Math.Pow(2, 31)));
                UInt32 temp = num >> 31;
                num <<= 1;
                num += temp;
            }
            return num;
        }

        //циклический сдвиг 32-х битового числа на n разрядов 

        private static UInt32 RetL(UInt64 data)
        {
            data >>= 32;
            UInt32 result = (UInt32)data; //temp;
            return result;
        }

        //Возвращает старшую часть 8 байтового блока данных

        private static UInt32 RetR(UInt64 data)
        {
            UInt32 result = (UInt32)data;
            return result;
        }

        //Возвращает младшую часть 8 байтового блока данных

        private static UInt32 Func(UInt32 R, UInt32 Ki)
        {
            UInt32 s = Mod2_32(R, Ki);
            List<UInt32> partsS = new List<UInt32>();
            for (int i = 0; i < 8; ++i)
            {
                UInt32 temp = s >> 28;
                partsS.Add(temp);
                s <<= 4;
            }
            partsS.Reverse();
            for (int i = 0; i < 8; ++i)
            {
                partsS[i] = Sblocks[i, (int)partsS[i]];
            }
            s = 0;
            for (int i = 0; i < partsS.Count; ++i)
            {
                s += partsS[i];
                s <<= 4;
            }
            s = ShiftN(s, 11);
            return s;
        }

        //Функция f(Ri, Ki), используемая в сети Фейстеля

        private static UInt64 EncodePartData(UInt64 partData, List<UInt32> partsKey)
        {
            for (int i = 0; i < 24; ++i)
            {
                partData = Feistel(partData, partsKey[i % 8]);
            }
            for (int i = 7; i >= 0; --i)
            {
                partData = Feistel(partData, partsKey[i]);
            }
            UInt64 result = (partData << 32) + (partData >> 32);
            return result;
        }

        //Шифрует 64-х битный блок данных

        private static UInt64 DecodePartData(UInt64 partData, List<UInt32> partsKey)
        {
            for (int i = 0; i < 8; ++i)
            {
                partData = Feistel(partData, partsKey[i]);
            }
            for (int i = 23; i >= 0; --i)
            {
                partData = Feistel(partData, partsKey[i % 8]);
            }
            UInt64 result = (partData << 32) + (partData >> 32);
            return result;
        }

        //Расшифровывает 64-х битный блок шифрованных данных

        private static UInt64 Feistel(UInt64 partData, UInt32 partKey) //осуществляет шаг в сети Фейстеля
        {
            UInt32 L = RetL(partData);
            UInt32 R = RetR(partData);
            UInt32 temp = Func(R, partKey);
            UInt32 xor = L ^ temp;
            UInt64 result = (UInt64)R;
            result <<= 32;
            result += (UInt64)xor;
            return result;
        }

        public static string Encode(string data, string key)
        {
            if (!Mod64(data.Length))
            {
                while (!Mod64(data.Length))
                {
                    data += "\0";
                }
            }
            List<UInt32> partsKey = FillingListKey(key);
            List<UInt64> partsData = FillingListData(data);
            List<UInt64> encodedData = new List<UInt64>();
            string result = "";
            for (int i = 0; i < partsData.Count; ++i)
            {
                encodedData.Add(EncodePartData(partsData[i], partsKey));
            }
            for (int i = 0; i < encodedData.Count; ++i)
            {
                result += GetPartString(encodedData[i]);
            }
            return Base64Encode(result);
        }

        //Метод шифрования

        public static string Decode(string codedData, string key)
        {
            codedData = Base64Decode(codedData);
            List<UInt32> partsKey = FillingListKey(key);
            List<UInt64> partsData = FillingListData(codedData);
            List<UInt64> decodedData = new List<UInt64>();
            string result = "";
            for (int i = 0; i < partsData.Count; ++i)
            {
                decodedData.Add(DecodePartData(partsData[i], partsKey));
            }
            for (int i = 0; i < decodedData.Count; ++i)
            {
                result += GetPartString(decodedData[i]);
            }
            return result.TrimEnd('\0');
        }

        //Метод расшифровки

        private static string Base64Encode(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        private static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}