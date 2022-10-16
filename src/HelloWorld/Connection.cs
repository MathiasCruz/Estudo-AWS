using System;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;

namespace HelloWorld
{
    public class Connection
    {
        public AmazonDynamoDBConfig clientConfig;
        public Connection()
        {
            clientConfig = new AmazonDynamoDBConfig();
            clientConfig.ServiceURL = "http://localhost:8000";
        }
        private AmazonDynamoDBClient Connect()
        {
            var client = new AmazonDynamoDBClient(clientConfig);
            return client;
        }

        public async void GetInformation(string table)
        {
            var client = Connect();
            Console.WriteLine("\n*** Retrieving table information ***");
            var request = new DescribeTableRequest
            {
                TableName = table
            };

            var response = await client.DescribeTableAsync(request);
            TableDescription description = response.Table;
            Console.WriteLine("Name: {0}", description.TableName);
            Console.WriteLine("# of items: {0}", description.ItemCount);
            Console.WriteLine("Provision Throughput (reads/sec): {0}",
                      description.ProvisionedThroughput.ReadCapacityUnits);
            Console.WriteLine("Provision Throughput (writes/sec): {0}",
                      description.ProvisionedThroughput.WriteCapacityUnits);
        }

    }
}