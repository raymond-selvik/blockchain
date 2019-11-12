using BlockchainClient.BlockchainClasses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using WebSocketSharp;

namespace BlockchainClient
{
    class P2PClient
    {
        IDictionary<string, WebSocket> wsDict = new Dictionary<string, WebSocket>();

        public void Connect(string url)
        {
            WebSocket ws = new WebSocket(url);
            ws.OnMessage += (sender, e) =>
            {
                if (e.Data == "Hi Client")
                {
                    Console.WriteLine(e.Data);
                }
                else
                {
                    Blockchain receivedBlockchain = JsonConvert.DeserializeObject<Blockchain>(e.Data);

                    if (receivedBlockchain.IsValid() && receivedBlockchain.Chain.Count > Program.blockchain.Chain.Count)
                    {
                        List<Transaction> newTransactions = new List<Transaction>();

                        newTransactions.AddRange(receivedBlockchain.PendingTransactions);
                        newTransactions.AddRange(Program.blockchain.PendingTransactions);

                        receivedBlockchain.PendingTransactions = newTransactions;
                        Program.blockchain = receivedBlockchain;

                    }
                }
            };
            ws.Connect();
            ws.Send("Hi Server");
            ws.Send(JsonConvert.SerializeObject(Program.blockchain));
            wsDict.Add(url, ws);
        }

        public void Send(string url, string data)
        {
            foreach(var item in wsDict)
            {
                if(item.Key == url)
                {
                    item.Value.Send(data);
                }
            }
        }

        public void Broadcast(string data)
        {
            foreach (var item in wsDict)
            {
                item.Value.Send(data);
            }
        }

        public IList<string> GetServers()
        {
            IList<string> servers = new List<string>();
            foreach (var item in wsDict)
            {
                servers.Add(item.Key);
            }
            return servers;
        }

        public void Close()
        {
            foreach (var item in wsDict)
            {
                item.Value.Close();
            }
        }
    }
}
