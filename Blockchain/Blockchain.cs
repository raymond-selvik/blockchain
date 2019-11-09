using System;
using System.Collections.Generic;
using System.Text;

namespace BlockchainClient
{
    class Blockchain
    {
        public List<Block> Chain { get; set; }

        public Blockchain()
        {
            Chain = new List<Block>();
            Chain.Add(new Block(null, null));
        }

        public Block GetLatestBlock()
        {
            return Chain[Chain.Count - 1];
        }

        public void AddNewBlock(Block block)
        {
            Block latestBlock = GetLatestBlock();
            block.index = latestBlock.index + 1;
            block.previousHash = latestBlock.GetBlockHash();
            block.blockHash = block.CalculateHash();

            Chain.Add(block);
        }

        public bool IsValid()
        {
            for (int i = 1; i < Chain.Count; i++)
            {
                Block current = Chain[i];
                Block previous = Chain[i - 1];

                if(current.blockHash != current.CalculateHash())
                {
                    return false;
                }

                if(current.previousHash != previous.blockHash)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
