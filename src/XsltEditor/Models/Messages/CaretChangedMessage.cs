using CommunityToolkit.Mvvm.Messaging.Messages;

namespace XsltEditor.Models.Messages;

internal class CaretChangedMessage(int value) : ValueChangedMessage<int>(value)
{
}