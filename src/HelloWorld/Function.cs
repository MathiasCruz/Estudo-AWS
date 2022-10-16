using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;
using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using System;
using Amazon.DynamoDBv2;
using Amazon.Runtime;
using Amazon;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace HelloWorld
{

    public class Function
    {

        private static readonly HttpClient client = new HttpClient();

        private static async Task<string> GetCallingIP()
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("User-Agent", "AWS Lambda .Net Client");

            var msg = await client.GetStringAsync("http://checkip.amazonaws.com/").ConfigureAwait(continueOnCapturedContext: false);

            return msg.Replace("\n", "");
        }
        public static async Task CallTableDescriptionAsync()
        {
            var clientConfig = new AmazonDynamoDBConfig();
            clientConfig.RegionEndpoint = RegionEndpoint.SAEast1;
            var client = new AmazonDynamoDBClient(clientConfig);
            Console.WriteLine("\n*** Retrieving table information ***");
            var request = new DescribeTableRequest
            {
                TableName = "RestricaoCVM"
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


        public async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest apigProxyEvent, ILambdaContext context)
        {
            await CallTableDescriptionAsync();
            var location = await GetCallingIP();
            var body = new Dictionary<string, string>
            {
                { "message", "hello world" },
                { "location", location }
            };

            return new APIGatewayProxyResponse
            {
                Body = JsonSerializer.Serialize(body),
                StatusCode = 200,
                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
            };
        }

    }
}

