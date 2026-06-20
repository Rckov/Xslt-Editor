# XSLT Editor

[![.NET Version](https://img.shields.io/badge/.NET-10.0-blue)](https://dotnet.microsoft.com/)
[![Build Status](https://github.com/Rckov/Xslt-Editor/actions/workflows/dotnet-desktop.yml/badge.svg)](https://github.com/Rckov/Xslt-Editor/actions/workflows/dotnet-desktop.yml)
[![Latest Release](https://img.shields.io/github/v/release/Rckov/Xslt-Editor)](https://github.com/Rckov/Xslt-Editor/releases/latest)

A lightweight editor for working with XSLT.

## About

Supports XSLT 1.0, 2.0, and 3.0 with real-time compilation, tag autocompletion, and plugin support.

**XSLT engines**

- XSLT 1.0 via .NET engine
- XSLT 2.0 and 3.0 via Saxon-HE

---

[![Download Latest](https://img.shields.io/badge/Download_Latest-2ea44f?style=for-the-badge&logo=github&logoColor=white)](https://github.com/Rckov/Xslt-Editor/releases/latest)

![Editor Interface Preview](https://github.com/Rckov/Xslt-Editor/raw/master/images/preview-hero.png)

---

## Plugins

The editor ships with built-in plugins. Copy `out/plugins/{plugin-name}/` next to the editor executable.

| Plugin | Description |
| --- | --- |
| XML Formatter | Formats XML and XSL documents with indentation |
| XPath Validator | Validates XPath expressions against an XML document |

## SDK

Plugins are built against the SDK in [`src/SDK/`](src/SDK/). Reference it as a project dependency, inherit [`PluginBase`](src/SDK/PluginBase.cs), and implement `Execute`.

```csharp
using XsltEditor.Sdk;
using XsltEditor.Sdk.Abstractions;
using XsltEditor.Sdk.Extensions;

public class MyPlugin : PluginBase
{
    public override string Name => "My Plugin";
    public override string Description => "Does something useful";

    public override void Execute(IDocumentContext context)
    {
        var xml = context.GetDocument(DocumentType.Xml);
        var xsl = context.GetDocument(DocumentType.Xsl);

        xml.Content = ProcessXml(xml.Content);
    }
}
```

Output goes to `plugins/{plugin-name}/` next to the editor executable.

---

[MIT License](LICENSE) · [Report an Issue](https://github.com/Rckov/Xslt-Editor/issues)
