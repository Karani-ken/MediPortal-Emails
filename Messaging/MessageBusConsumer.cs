using Azure.Messaging.ServiceBus;
using MediPortal_Emails.Models.Dtos;
using MediPortal_Emails.Services;
using Newtonsoft.Json;
using System.Text;

namespace MediPortal_Emails.Messaging
{
    public class MessageBusConsumer : IMessageBusConsumer
    {
        private readonly IConfiguration _configuration;
        private readonly string ConnectionString;
        private readonly string QueueName;
        private readonly ServiceBusProcessor _registrationProcessor;
        private readonly EmailSendService _emailSend;

        public MessageBusConsumer(IConfiguration configuration)
        {
            _configuration = configuration;
            ConnectionString = _configuration.GetSection("ServiceBus:ConnectionString").Get<string>();
            QueueName = _configuration.GetSection("QueuesandTopics:RegisterUser").Get<string>();

            //connect to the service bus client
            var serviceBusClient = new ServiceBusClient(ConnectionString);
            _registrationProcessor = serviceBusClient.CreateProcessor(QueueName);
            Console.WriteLine("Queue" + QueueName);
            _emailSend = new EmailSendService(_configuration);
        }
        public async Task Start()
        {
            _registrationProcessor.ProcessMessageAsync += OnRegistration;
            _registrationProcessor.ProcessErrorAsync += ErrorHandler;

            await _registrationProcessor.StartProcessingAsync();
        }
        private Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.ToString());
            return Task.CompletedTask;
        }

        public async Task Stop()
        {
            //stop processing
            await _registrationProcessor.StopProcessingAsync();
            await _registrationProcessor.DisposeAsync();
        }
        private async Task OnRegistration(ProcessMessageEventArgs args)
        {
            //get th message from arguments
            var message = args.Message;
            //convert the data to a string before deserializing to an object
            var body = Encoding.UTF8.GetString(message.Body);
            //deserialize email to an object
            var userMessage = JsonConvert.DeserializeObject<EmailMessage>(body);
            //send the email
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("<img src=\"https://cdn.pixabay.com/photo/2017/03/14/03/20/woman-2141808_640.jpg\" width=\"1000\" height=\"600\">");
                stringBuilder.Append("<h1> Hello " + userMessage.Name + "</h1>");
                stringBuilder.AppendLine("<br/>" + userMessage.Content);

                stringBuilder.Append("<br/>");
                stringBuilder.Append('\n');                
                await _emailSend.SendMail(userMessage, stringBuilder.ToString());
                //delete the email from the queue
                await args.CompleteMessageAsync(message);
            }
            catch (Exception ex) { }
        }
    }
}
