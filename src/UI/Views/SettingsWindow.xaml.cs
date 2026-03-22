using System.Windows;

namespace XsltEditor.Views;

public partial class SettingsWindow : Window
{
	public SettingsWindow()
	{
		InitializeComponent();
	}

	private void OnCloseClick(object sender, RoutedEventArgs e)
	{
		Close();
	}
}