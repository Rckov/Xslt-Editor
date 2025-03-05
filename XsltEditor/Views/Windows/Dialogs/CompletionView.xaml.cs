using XsltEditor.ViewModels.Dialogs;

namespace XsltEditor.Views.Windows.Dialogs;

public partial class CompletionView
{
    public CompletionView(CompletionViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}