namespace XsltEditor.Models;

public record ThemeDescriptor(
    ThemeType Type,
    string BrushesUri,
    string HighlightingUri);