using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using XsltEditor.Models.Base;

namespace XsltEditor.Models;
public class Settings : ObservableObject
{
    public Settings()
    {
        FontSize = 14;
        FontFamily = "Consolas";
    }

    public int FontSize
    {
        get;
        set => Set(ref field, value);
    }

    public string? FontFamily
    {
        get;
        set => Set(ref field, value);
    }
}
