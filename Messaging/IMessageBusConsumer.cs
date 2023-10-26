namespace MediPortal_Emails.Messaging
{
    public interface IMessageBusConsumer
    {
        Task Start();

        Task Stop();
    }
}
