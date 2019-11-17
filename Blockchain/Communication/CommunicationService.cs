using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

using BlockchainClient.BlockchainClasses;
using Microsoft.Azure.ServiceBus;
using System.Threading.Tasks;
using System.Threading;

namespace BlockchainClient.Communication
{
    public class CommunicationService
    {
        const string ServiceBusConnectionString = "Endpoint=sb://rdsblockchain.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=+7+b5ZfR32ak6g3KEdBISz/u+LpOfCQnqcLvYwZuEsg=";
        const string TopicName = "blockchain";
        const string SubscriptionName = "blockchain-sub";

        static ITopicClient topicClient;
        static ISubscriptionClient subscriptionClient;

        static bool blockchainSynced = false;

        public CommunicationService()
        {
            topicClient = new TopicClient(ServiceBusConnectionString, TopicName);
            subscriptionClient = new SubscriptionClient(ServiceBusConnectionString, TopicName, SubscriptionName);

            RegisterOnMessageHandlerAndReceiveMessages();
        }

        static async Task ProcessMessagesAsync(Message message, CancellationToken token)
        {
            // Process the message
            //Console.WriteLine($"Received message: SequenceNumber:{message.SystemProperties.SequenceNumber} Body:{Encoding.UTF8.GetString(message.Body)}");
            var messageString = Encoding.UTF8.GetString(message.Body);

            Blockchain receivedBlockchain = JsonConvert.DeserializeObject<Blockchain>(messageString);

            if (receivedBlockchain.IsValid() && receivedBlockchain.Chain.Count > Program.blockchain.Chain.Count)
            {
                List<Transaction> newTransactions = new List<Transaction>();
                newTransactions.AddRange(receivedBlockchain.PendingTransactions);
                newTransactions.AddRange(Program.blockchain.PendingTransactions);

                receivedBlockchain.PendingTransactions = newTransactions;
                Program.blockchain = receivedBlockchain;
            }
            if (!blockchainSynced)
            {
                //Send(JsonConvert.SerializeObject(Program.blockchain));
                blockchainSynced = true;
            }

            // Complete the message so that it is not received again.
            // This can be done only if the subscriptionClient is opened in ReceiveMode.PeekLock mode (which is default).
            await subscriptionClient.CompleteAsync(message.SystemProperties.LockToken);
        }

        static Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            Console.WriteLine($"Message handler encountered an exception {exceptionReceivedEventArgs.Exception}.");
            return Task.CompletedTask;
        }

        static void RegisterOnMessageHandlerAndReceiveMessages()
        {
            // Configure the MessageHandler Options in terms of exception handling, number of concurrent messages to deliver etc.
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                // Maximum number of Concurrent calls to the callback `ProcessMessagesAsync`, set to 1 for simplicity.
                // Set it according to how many messages the application wants to process in parallel.
                MaxConcurrentCalls = 1,

                // Indicates whether MessagePump should automatically complete the messages after returning from User Callback.
                // False below indicates the Complete will be handled by the User Callback as in `ProcessMessagesAsync` below.
                AutoComplete = false
            };

            // Register the function that will process messages
            subscriptionClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);
        }

        public async void BroadcastBlockchain(Blockchain blockchain)
        {
            try
            {
                // Create a new message to send to the topic
                string messageBody = JsonConvert.SerializeObject(blockchain);
                var message = new Message(Encoding.UTF8.GetBytes(messageBody));

                // Write the body of the message to the console
                Console.WriteLine($"Sending message: {messageBody}");

                // Send the message to the topic
                await topicClient.SendAsync(message);
            }
            catch (Exception exception)
            {
                Console.WriteLine($"{DateTime.Now} :: Exception: {exception.Message}");
            }
        }
    }
}
