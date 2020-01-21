using Microsoft.AspNet.SignalR.Client;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AuctionJobs
{
    // To learn more about Microsoft Azure WebJobs SDK, please see https://go.microsoft.com/fwlink/?LinkID=320976
    class Program
    {
        // Please set the following connection strings in app.config for this WebJob to run:
        // AzureWebJobsDashboard and AzureWebJobsStorage
        static void Main()
        {
            Task.Run(async () => await SetupSignalRClient()).Wait();

           while(true)
            {
                Task.Run(async () =>
                {
                    await Functions.NotifyOfExpiringAuctions(Console.Out);
                    Thread.Sleep(30000);
                }).Wait();
            }
        }

        private static async Task SetupSignalRClient()
        {
            var connection = new HubConnection("https://pswjauction.azurewebsites.net");
            Functions.auctionHubClient = connection.CreateHubProxy("auctionHub");
            try
            {
                await connection.Start();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
