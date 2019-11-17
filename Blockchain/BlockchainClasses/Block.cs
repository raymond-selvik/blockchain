using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public string BlockHash { get; set; } = null;

        public Block(string previousHash, List<Transaction> data)
        {
            Index = 0;
            this.Timtestamp = DateTime.UtcNow;
            this.PreviousHash = previousHash;
            this.Data = data;
        }

        public string CalculateBlockHash()
        {
            SHA256 sha256 = SHA256.Create();
            
            var serializedData = JsonConvert.SerializeObject(Data);

            byte[] encodedData = Encoding.UTF8.GetBytes($"{Timtestamp}-{Index}-{PreviousHash}-{serializedData}-{Nonce}");
            var hash = sha256.ComputeHash(encodedData);

            BlockHash = Convert.ToBase64String(hash);

            return BlockHash;
        }

        public void Mine(int difficulty)
        {
            SHA256 sha256 = SHA256.Create();

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            while (true)
            {
                string hashedData = CalculateBlockHash();

                if(hashedData.StartsWith(DifficultyString(difficulty), StringComparison.Ordinal))
                {
                    this.BlockHash = hashedData;

                    stopwatch.Stop();
                    TimeSpan ts = stopwatch.Elapsed;

                    string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds);

                    Console.WriteLine("Mined block with difficult level: {0}, Nonce: {1}, Elapsed Time: {2}", difficulty, Nonce, elapsedTime);

                    return;
                }

                Nonce++;
            }
        }

        private string DifficultyString(int difficulty)
        {
            string difficultyString = string.Empty;

            for (int i = 0; i < difficulty; i++)
            {
                difficultyString += "0";
            }

            return difficultyString;
        }

        public string GetBlockHash()
        {
            return BlockHash;
        }
    }
}
