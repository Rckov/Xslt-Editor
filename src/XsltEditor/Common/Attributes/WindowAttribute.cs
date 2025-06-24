using System.Windows;

namespace XsltEditor.Common.Attributes;

[AttributeUsage(AttributeTargets.Class)]
internal class WindowAttribute : Attribute
{
    public Type WindowType { get; }

    public WindowAttribute(Type windowType)
    {
        if (!typeof(Window).IsAssignableFrom(windowType))
        {
            throw new ArgumentException($"Type {windowType.Name} must inherit from Window.");
        }

        WindowType = windowType;
    }
}