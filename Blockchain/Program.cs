using System;
using System.Collections.Generic;

namespace Blockchain
{
    class Program
    {
        static void Main(string[] args)
        {
            List<String> transactions = new List<string>();
            transactions.Add("Hfhgfello");
            transactions.Add("dddd");

            Block genesisBlock = new Block(null, transactions);

            List<String> transactions2 = new List<string>();
            transactions2.Add("øøok,e");

            Block block2 = new Block(genesisBlock.GetBlockHash(), transactions2);

            Console.WriteLine(genesisBlock.GetBlockHash());
            Console.WriteLine(block2.GetBlockHash());
        }
    }
}
