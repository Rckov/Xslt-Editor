namespace XsltEditor.Services.Abstractions;

internal interface IWindowService
{
	T? ShowWindow<T>(T? context = null, bool dialog = false)
		where T : class;
}