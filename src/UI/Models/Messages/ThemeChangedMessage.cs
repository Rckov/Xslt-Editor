using ICSharpCode.AvalonEdit.Highlighting;

namespace XsltEditor.Models.Messages;

public record ThemeChangedMessage(ThemeType ThemeType, IHighlightingDefinition? Highlighting);