using XsltEditor.Views.UserControls;

namespace XsltEditor.Services.Interfaces;

public interface IWindowService
{
    void ShowWindow<TWindow>() where TWindow : Window;

    void ShowDialogWindow<TWindow>() where TWindow : Window;
}