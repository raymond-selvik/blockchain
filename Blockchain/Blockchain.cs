using System;
using System.Collections.Generic;
using System.Text;

namespace BlockchainClient
{
    class Blockchain
    {
        public List<Block> Chain { get; set; }
        public int Difficulty { get; set; } = 2;

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

                if(current.BlockHash != current.CalculateHash())
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
    }
}
