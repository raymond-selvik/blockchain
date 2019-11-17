using System;
using System.Collections.Generic;
using System.Text;

namespace BlockchainClient.BlockchainClasses
{
    public class Blockchain
    {
        public List<Block> Chain { get; set; }
        public List<Transaction> PendingTransactions { get; set; }
        public int Difficulty { get; set; } = 2;

        public int MinerReward { get; set; } = 5;

        public Blockchain()
        {
            Chain = new List<Block>();

            PendingTransactions = new List<Transaction>();
        }

        public void AddGenesisBlock()
        {
            Block genesisBlock = new Block(null, null);
            genesisBlock.BlockHash = genesisBlock.CalculateBlockHash();

            Chain.Add(genesisBlock);
        }

        public Block GetLatestBlock()
        {
            return Chain[Chain.Count - 1];
        }

        public void AddNewBlock(Block block)
        {
            Block latestBlock = GetLatestBlock();
            block.Index = latestBlock.Index + 1;
            block.PreviousHash = latestBlock.GetBlockHash();
            block.Mine(this.Difficulty);

            Chain.Add(block);
        }

        public bool IsValid()
        {
            for (int i = 1; i < Chain.Count; i++)
            {
                Block current = Chain[i];
                Block previous = Chain[i - 1];

                if(current.BlockHash != current.CalculateBlockHash())
                {
                    return false;
                }

                if(current.PreviousHash != previous.BlockHash)
                {
                    return false;
                }
            }

            return true;
        }

        public void CreateTransaction(Transaction transaction)
        {
            PendingTransactions.Add(transaction);
        }

        public void ProcessPendingTransactions(string minerAdress)
        {
            Block block = new Block(GetLatestBlock().BlockHash, PendingTransactions);
            AddNewBlock(block);

            PendingTransactions = new List<Transaction>();
            var transaction = new Transaction(null, minerAdress, MinerReward);
            CreateTransaction(transaction);
        }
    }
}
