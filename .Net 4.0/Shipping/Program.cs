using System;

namespace Shipping
{
    public class Program
    {
        public static int MinimalNumberOfPackages(int items, int availableLargePackages, int availableSmallPackages)
        {
            const int itemCountInsideLargePackage = 5;

            int usedLargePackageCount;
            int remainingItemCount;

            var largePackegesRequiredAmount = items/itemCountInsideLargePackage;
            if (largePackegesRequiredAmount <= availableLargePackages)
            {
                usedLargePackageCount = items/itemCountInsideLargePackage;
                remainingItemCount = items%itemCountInsideLargePackage;
            }
            else
            {
                usedLargePackageCount = availableLargePackages;
                remainingItemCount = items - availableLargePackages*itemCountInsideLargePackage;
            }

            if (remainingItemCount <= availableSmallPackages)
            {
                var usedSmallPackageCount = remainingItemCount;
                return usedLargePackageCount + usedSmallPackageCount;
            }
            else
            {
                return -1;
            }
        }

        public static void Main(string[] args)
        {
            Console.WriteLine(MinimalNumberOfPackages(16, 2, 10) == 8);
            Console.WriteLine(MinimalNumberOfPackages(0, 0, 0) == 0);
            Console.WriteLine(MinimalNumberOfPackages(1, 1, 0) == -1);
            Console.WriteLine(MinimalNumberOfPackages(5, 1, 0) == 1);
            Console.WriteLine(MinimalNumberOfPackages(1, 0, 1) == 1);
            Console.WriteLine(MinimalNumberOfPackages(2, 0, 2) == 2);
            Console.WriteLine(MinimalNumberOfPackages(9, 2, 0) == -1);
            Console.WriteLine(MinimalNumberOfPackages(10, 2, 0) == 2);
            Console.ReadLine();
        }
    }
}