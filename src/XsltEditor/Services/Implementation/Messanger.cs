using XsltEditor.Services.Interfaces;

namespace XsltEditor.Services.Implementation;

internal class Messanger : IMessenger
{
    private readonly Dictionary<Type, List<Delegate>> _handlers = [];

    public void Send<TMessage>(TMessage message)
    {
        if (!_handlers.TryGetValue(typeof(TMessage), out var handlers))
        {
            return;
        }

        foreach (var handler in handlers)
        {
            ((Action<TMessage>)handler)(message);
        }
    }

    public void Subscribe<TMessage>(Action<TMessage> handler)
    {
        var messageType = typeof(TMessage);

        if (!_handlers.TryGetValue(messageType, out var handlers))
        {
            handlers = [];
            _handlers[messageType] = handlers;
        }

        handlers.Add(handler);
    }

    public void Unsubscribe<TMessage>(Action<TMessage> handler)
    {
        var messageType = typeof(TMessage);

        if (!_handlers.TryGetValue(messageType, out var handlers))
        {
            return;
        }

        handlers.RemoveAll(h => ReferenceEquals(h, handler));
    }
}