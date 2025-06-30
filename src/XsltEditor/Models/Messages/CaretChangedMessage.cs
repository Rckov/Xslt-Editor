using CommunityToolkit.Mvvm.Messaging.Messages;

namespace XsltEditor.Models.Messages;

internal class CaretChangedMessage(Guid? id, int value) : ValueChangedMessage<int>(value)
{
    public Guid? Id => id;
}