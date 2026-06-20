using System.Windows;

namespace XsltEditor.Common.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class WindowAttribute(Type windowType) : Attribute
{
    public Type WindowType { get; } = typeof(Window).IsAssignableFrom(windowType)
        ? windowType
        : throw new ArgumentException($"Type {windowType.Name} must inherit from Window.");
}