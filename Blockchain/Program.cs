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
            blockchain.AddNewBlock(block1);
            blockchain.AddNewBlock(block2);

            foreach(Block b in blockchain.Chain)
            {
                Console.WriteLine(b.blockHash);
            }

            Console.WriteLine("Blockchain is valid: " + blockchain.IsValid());

            blockchain.Chain[1].data = "{sender:Henry,receiver:MaHesh,amount:1000}";
 
            Console.WriteLine("Blockchain is valid: " + blockchain.IsValid());

            blockchain.Chain[1].blockHash = blockchain.Chain[1].CalculateHash();

            Console.WriteLine("Blockchain is valid: " + blockchain.IsValid());
        }
    }
}
