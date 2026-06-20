namespace XsltEditor.Services.Abstractions;

public interface IWindowService
{
    T? ShowWindow<T>(T? context = null, bool dialog = false)
        where T : class;
}