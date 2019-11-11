using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace BlockchainClient
{
    public class P2PServer : WebSocketBehavior
    {
        bool blockchainSynced = false;
        WebSocketServer wss = null;

        public void Start()
        {
            wss = new WebSocketServer($"ws://localhost:{Program.Port}");
            wss.AddWebSocketService<P2PServer>("/Blockchain");
            wss.Start();
            Console.WriteLine($"Started server at ws://127.0.0.1:{Program.Port}");
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            if(e.Data == "Hi Server")
            {
                Console.WriteLine(e.Data);
                Send("Hi Client");
            }
            else
            {
                Blockchain receivedBlockchain = JsonConvert.DeserializeObject<Blockchain>(e.Data);

                if(receivedBlockchain.IsValid() && receivedBlockchain.Chain.Count > Program.blockchain.Chain.Count)
                {
                    List<Transaction> newTransactions = new List<Transaction>();
                    newTransactions.AddRange(receivedBlockchain.PendingTransactions);
                    newTransactions.AddRange(Program.blockchain.PendingTransactions);

                    receivedBlockchain.PendingTransactions = newTransactions;
                    Program.blockchain = receivedBlockchain;
                }
                if (!blockchainSynced)
                {
                    Send(JsonConvert.SerializeObject(Program.blockchain));
                    blockchainSynced = true;
                }
            }
        }
    }
}
