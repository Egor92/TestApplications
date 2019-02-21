using System;

namespace ProxyClass
{
    public class Singer : ISinger
    {
        public void SingLala()
        {
            Console.WriteLine("La-la-la-la");
        }

        public void SingMacarena()
        {
            Console.WriteLine("Ooooooo, Macarena! Yeah!");
        }

        public void ThrowException()
        {
            throw new Exception("This is exception that you want");
        }
    }
}