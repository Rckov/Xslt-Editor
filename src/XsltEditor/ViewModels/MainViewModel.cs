using Avalonia.Controls;

using CommunityToolkit.Mvvm.ComponentModel;

using Dock.Model.Controls;
using Dock.Model.Core;

using System.Linq;

namespace XsltEditor.ViewModels;

internal partial class MainViewModel : ObservableObject
{
    [ObservableProperty] private IRootDock? _layout;
    [ObservableProperty] private IFactory? _factory;

    public MainViewModel()
    {
        Factory = new DockFactory();
        Layout = Factory.CreateLayout();
        Factory.InitLayout(Layout);

        if (Layout is IRootDock root && root.VisibleDockables?.FirstOrDefault() is IDock dock)
        {
            root.ActiveDockable = dock;

            if (dock.VisibleDockables?.FirstOrDefault() is IDockable doc)
                dock.ActiveDockable = doc;
        }
    }
}