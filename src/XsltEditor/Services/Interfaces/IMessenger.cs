namespace XsltEditor.Services.Interfaces;

public interface IMessenger
{
    void Send<TMessage>(TMessage message);

    void Subscribe<TMessage>(Action<TMessage> handler);

    void Unsubscribe<TMessage>(Action<TMessage> handler);
}