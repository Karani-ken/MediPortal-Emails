using MediPortal_Emails.Messaging;

namespace MediPortal_Emails.Extensions
{
    public static class AzureServiceStarter
    {

        public static IMessageBusConsumer ServiceBusConstumerInstance { get; set; }
        public static IApplicationBuilder useAzure(this IApplicationBuilder app)
        {

            //gets an instance of the IAzureMessageBusConsumer service
            ServiceBusConstumerInstance = app.ApplicationServices.GetService<IMessageBusConsumer>();

            var HostLife = app.ApplicationServices.GetService<IHostApplicationLifetime>();

            HostLife.ApplicationStarted.Register(OnStart);
            HostLife.ApplicationStopping.Register(OnStop);

            return app;
        }

        private static void OnStop()
        {
            ServiceBusConstumerInstance.Stop();
        }

        private static void OnStart()
        {
            ServiceBusConstumerInstance.Start();
            Console.WriteLine("Appstartted");
        }
    }
}
