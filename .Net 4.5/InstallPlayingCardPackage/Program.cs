using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Egor92.PlayingCards;

namespace InstallPlayingCardPackage
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.Unicode;
            var deck = DeckFactory.Create36Cards();
            deck.Shuffle();
            foreach (var card in deck)
            {
                Console.WriteLine(card.Code);
            }
            Console.Read();
        }
    }
}
