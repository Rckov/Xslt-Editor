using Dock.Avalonia.Controls;
using Dock.Model.Controls;
using Dock.Model.Core;
using Dock.Model.Mvvm;
using Dock.Model.Mvvm.Controls;

using System;
using System.Collections.Generic;

using XsltEditor.Models;
using XsltEditor.ViewModels.Documents;

namespace XsltEditor.ViewModels;

internal class DockFactory : Factory
{
    private IRootDock? _rootDock;
    private IDocumentDock? _documentDock;

    public override IRootDock CreateLayout()
    {
        var doc1 = CreateDocumentViewModel("XSL");
        var doc2 = CreateDocumentViewModel("XML");

        _documentDock = new DocumentDock()
        {
            CanFloat = false,
            VisibleDockables = CreateList<IDockable>(doc1, doc2),
        };

        _rootDock = CreateRootDock();
        _rootDock.VisibleDockables = CreateList<IDockable>(_documentDock);
        _rootDock.ActiveDockable = _documentDock;

        return _rootDock;
    }

    public override void InitLayout(IDockable layout)
    {
        ContextLocator = new Dictionary<string, Func<object?>>
        {
            ["Root"] = () => _rootDock,
            ["Documents"] = () => _documentDock
        };

        DockableLocator = new Dictionary<string, Func<IDockable?>>
        {
            ["Root"] = () => _rootDock,
            ["Documents"] = () => _documentDock
        };

        HostWindowLocator = new Dictionary<string, Func<IHostWindow?>>
        {
            [nameof(IDockWindow)] = () => new HostWindow()
        };

        base.InitLayout(layout);
    }

    private Document CreateDocumentViewModel(string name)
    {
        return new DocumentViewModel()
        {
            Id = name,
            Title = name,
            CanClose = false
        };
    }
}