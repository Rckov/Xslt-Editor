using ICSharpCode.AvalonEdit.Highlighting;

namespace XsltEditor.Models.Messages;

internal record ThemeChangedMessage(ThemeType ThemeType, IHighlightingDefinition? Highlighting);