using System.Windows;

namespace XsltEditor.Views;

public partial class SnippetWindow : Window
{
	public SnippetWindow()
	{
		InitializeComponent();
	}

	private void OnCloseClick(object sender, RoutedEventArgs e)
	{
		Close();
	}
}