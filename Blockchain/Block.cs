using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;

namespace Blockchain
{
    [Serializable]
    class Block
    {
        //private DateTime timestamp;
        private string previousHash;
        private List<string> transactions;

        private string blockHash;

        public Block(string previousHash, List<string> transactions)
        {
            //this.timestamp = DateTime.UtcNow;
            this.previousHash = previousHash;
            this.transactions = transactions;
            this.blockHash = CalculateBlockHash(); 
        }

        string CalculateBlockHash()
        {
            MD5 md5 = MD5.Create();

            using (var memoryStream = new MemoryStream())
            {
                var binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(memoryStream, this);

                var hash = md5.ComputeHash(memoryStream.ToArray());
                blockHash = BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();

                memoryStream.Close();
                return blockHash;
            }
        }

        public string GetBlockHash()
        {
            return blockHash;
        }
    }
}
