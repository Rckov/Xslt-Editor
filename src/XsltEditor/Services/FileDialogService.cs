using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Ursa.Controls;

using XsltEditor.Services.Interfaces;

namespace XsltEditor.Services;
internal class FileDialogService : IFileDialogService
{
    public void s()
    {
        PathPicker d = new PathPicker();
        d.Title = "123";
        d.UsePickerType = UsePickerTypes.OpenFolder;
    }
}
