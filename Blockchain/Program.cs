using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BlockchainClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var startTime = DateTime.Now;

            Blockchain blockchain = new Blockchain();
            blockchain.CreateTransaction(new Transaction("Ole", "Dole", 10));
            blockchain.ProcessPendingTransactions("Doffen");
            Console.WriteLine(JsonConvert.SerializeObject(blockchain, Formatting.Indented));

            blockchain.CreateTransaction(new Transaction("Dole", "Ole", 100));
            blockchain.CreateTransaction(new Transaction("Dole", "Ole", 500));
            blockchain.ProcessPendingTransactions("DOnald");
            Console.WriteLine(JsonConvert.SerializeObject(blockchain, Formatting.Indented));

            var endTime = DateTime.Now;
        }
    }
}
