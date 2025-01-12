using XsltEditor.ViewModels;

namespace XsltEditor.Views;

public partial class MainView
{
    public MainView(MainViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}