using Dock.Model.Controls;
using Dock.Model.Core;
using Dock.Model.Mvvm;
using Dock.Model.Mvvm.Controls;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using XsltEditor.ViewModels.Documents;

namespace XsltEditor.ViewModels;
internal class DockFactory : Factory
{
    private readonly object _context;
    private IRootDock? _rootDock;
    private IDocumentDock? _documentDock;

    public DockFactory(object context)
    {
        _context = context;
    }

    public override IRootDock CreateLayout()
    {
        var document1 = new DocumentViewModel { Id = "Document1", Title = "Document1" };
        var document2 = new DocumentViewModel { Id = "Document2", Title = "Document2" };

        var rootDock = CreateRootDock();
        rootDock.VisibleDockables = [document1, document2];

        _rootDock = rootDock;

        return rootDock;
    }
}
