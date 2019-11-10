using System;
using System.Collections.Generic;

namespace BlockchainClient
{
    class Program
    {
        static void Main(string[] args)
        {
           
            Block block1 = new Block(null, "Hello");
            Block block2 = new Block(null, "World");

            Blockchain blockchain = new Blockchain();


            var startTime = DateTime.Now;

            blockchain.AddNewBlock(block1);
            blockchain.AddNewBlock(block2);

            var endTime = DateTime.Now;


            Console.WriteLine($"Duration: {endTime - startTime}");

            foreach (Block b in blockchain.Chain)
            {
                Console.WriteLine(b.BlockHash);
            }

            Console.WriteLine("Blockchain is valid: " + blockchain.IsValid());

            blockchain.Chain[1].Data = "{sender:Henry,receiver:MaHesh,amount:1000}";
 
            Console.WriteLine("Blockchain is valid: " + blockchain.IsValid());

            blockchain.Chain[1].BlockHash = blockchain.Chain[1].CalculateHash();

            Console.WriteLine("Blockchain is valid: " + blockchain.IsValid());
        }
    }
}
