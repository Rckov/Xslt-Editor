using System.Windows;

namespace XsltEditor.Common.Attributes;

[AttributeUsage(AttributeTargets.Class)]
internal class WindowAttribute : Attribute
{
    public WindowAttribute(Type windowType)
    {
        if (!typeof(Window).IsAssignableFrom(windowType))
        {
            throw new ArgumentException($"Type {windowType.Name} must inherit from Window.");
        }

        WindowType = windowType;
    }

    public Type WindowType { get; }
}