using XsltEditor.ViewModels;

namespace XsltEditor.Views.Windows;

public partial class CompletionView
{
    public CompletionView(CompletionViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}