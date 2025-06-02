using CommunityToolkit.Mvvm.Messaging.Messages;

using ICSharpCode.AvalonEdit.Highlighting;

using XsltEditor.Services;

namespace XsltEditor.Models.Messages;

internal class ThemeChangedMessage(ThemeType value, IHighlightingDefinition? highlighting) : ValueChangedMessage<ThemeType>(value)
{
    public IHighlightingDefinition? Highlighting { get; } = highlighting;
}