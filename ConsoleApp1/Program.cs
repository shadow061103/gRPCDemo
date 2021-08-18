using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Grpc.Net.Client;
using GrpcServer.HumanResource;
using GrpcService1;

namespace ConsoleApp1
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            // 等待 Server 啟動
            await Task.Delay(5000);

            var httpHandler = new HttpClientHandler();
            // Return `true` to allow certificates that are untrusted/invalid
            httpHandler.ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

            // 建立連接到 https://localhost:5001 的通道
            var channel = GrpcChannel.ForAddress("https://localhost:5001", new GrpcChannelOptions { HttpHandler = httpHandler });

            // 建立 EmployeeClient
            var client = new Employee.EmployeeClient(channel);

            // 呼叫 GetEmployee()

            var employee = await client.GetEmployeeAsync(new EmployeeRequest { Id = 1 });
            // 輸出 EmployeeModel 的序列化結果
            Console.WriteLine(JsonSerializer.Serialize(employee, new JsonSerializerOptions { WriteIndented = true }));

            //預設的
            var client2 = new Greeter.GreeterClient(channel);
            var response = await client2.SayHelloAsync(
                     new HelloRequest { Name = "World" });

            Console.WriteLine(response.Message);

            Console.ReadKey();
        }
    }
}