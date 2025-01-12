using XsltEditor.ViewModels;

namespace XsltEditor.Views;

public partial class CompletionView
{
    public CompletionView(CompletionViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}