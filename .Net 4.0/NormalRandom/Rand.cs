using System;
using System.Collections.Generic;
using System.Linq;

namespace NormalRandom
{
    public class Rand
    {
        private static readonly Random Random = new Random();

        public static KeyValuePair<int, double>? GetProbabilitiesByValue(string line)
        {
            try
            {
                var list = line.Split(';')
                               .Select(x => x.Trim())
                               .Where(x => !string.IsNullOrWhiteSpace(x))
                               .ToList();
                if (list.Count != 2)
                    return null;

                var value = Convert.ToInt32(list[0]);
                var probability = Convert.ToDouble(list[1]);
                return new KeyValuePair<int, double>(value, probability);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static double GetDouble(double min, double max)
        {
            return min + (max - min)*Random.NextDouble();
        }

        public static double GetInt32(int min, int max)
        {
            return Random.Next(min, max);
        }

        public static T GetValue<T>(IDictionary<T, double> probabilitiesByValue)
        {
            if (probabilitiesByValue.Values.Any(x => x < 0.0))
                throw new ArgumentException("Values must be not negative", "probabilitiesByValue");
            var max = probabilitiesByValue.Values.Sum();
            if (!(max > 0.0))
                throw new ArgumentException("Summa of values must be positive", "probabilitiesByValue");
            var random = GetDouble(0.0, max);
            var probabilitySum = 0.0;
            foreach (var pair in probabilitiesByValue)
            {
                var value = pair.Key;
                var probability = pair.Value;

                probabilitySum += probability;
                if (probabilitySum > random)
                    return value;
            }
            throw new Exception("Algorithm fail");
        }
    }
}