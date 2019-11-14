using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace BlockchainClient.BlockchainClasses
{
    public class Block
    {
        public ulong Index { get; set; }
        private DateTime Timtestamp;
        public string PreviousHash {get; set;}
        public List<Transaction> Data { get; set; }
        public int Nonce { get; set; } = 0;
        public string BlockHash { get; set; }

        public Block(string previousHash, List<Transaction> data)
        {
            Index = 0;
            this.Timtestamp = DateTime.UtcNow;
            this.PreviousHash = previousHash;
            this.Data = data;
            this.BlockHash = CalculateHash(); 
        }

        public string CalculateHash()
        {
            MD5 md5 = MD5.Create();

            var serializedData = JsonConvert.SerializeObject(Data);

            byte[] encodedData = Encoding.UTF8.GetBytes($"{Timtestamp}-{PreviousHash}-{serializedData}-{Nonce}");
            var hash = md5.ComputeHash(encodedData);
            BlockHash = BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();

            return BlockHash;
        }

        public void Mine(int difficulty)
        {
            var leadingZeros = new string('0', difficulty);

            while(this.BlockHash == null || this.BlockHash.Substring(0, difficulty) != leadingZeros)
            {
                this.Nonce++;
                this.BlockHash = CalculateHash(); 
            }
        }

        public string GetBlockHash()
        {
            return BlockHash;
        }
    }
}
