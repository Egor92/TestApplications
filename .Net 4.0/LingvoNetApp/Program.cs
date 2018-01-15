using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Morpher;
using Morpher.Russian;
using Morpher.WebService.V2;

namespace LingvoNetApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new Client(new Parameters());
            var parse = client.Parse("Один", Category.Other);
        }
    }
}
