using System;
using System.Security.Cryptography;
using System.Text;

namespace BlockchainClient
{
    class Block
    {
        public ulong index { get; set; }
        private DateTime timestamp;
        public string previousHash {get; set;}
        public string data;

        public string blockHash { get; set; }

        public Block(string previousHash, string data)
        {
            index = 0;
            this.timestamp = DateTime.UtcNow;
            this.previousHash = previousHash;
            this.data = data;
            this.blockHash = CalculateHash(); 
        }

        public string CalculateHash()
        {
            MD5 md5 = MD5.Create();

            byte[] encodedData = Encoding.UTF8.GetBytes($"{timestamp}-{previousHash}-{data}");
            var hash = md5.ComputeHash(encodedData);
            blockHash = BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();

            return blockHash;
            
        }

        public string GetBlockHash()
        {
            return blockHash;
        }
    }
}
