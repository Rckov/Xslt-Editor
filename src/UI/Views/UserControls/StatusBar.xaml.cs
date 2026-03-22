using System.Windows;
using System.Windows.Controls;

namespace XsltEditor.Views.UserControls;

public partial class StatusBar : UserControl
{
	public static readonly DependencyProperty LeftContentProperty =
		DependencyProperty.Register(nameof(LeftContent), typeof(object), typeof(StatusBar));

	public static readonly DependencyProperty RightContentProperty =
		DependencyProperty.Register(nameof(RightContent), typeof(object), typeof(StatusBar));

	public StatusBar()
	{
		InitializeComponent();
	}

	public object LeftContent
	{
		get => GetValue(LeftContentProperty);
		set => SetValue(LeftContentProperty, value);
	}

	public object RightContent
	{
		get => GetValue(RightContentProperty);
		set => SetValue(RightContentProperty, value);
	}
}