using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using XsltEditor.Models;
using XsltEditor.Models.Enums;

namespace XsltEditor.Services.Interfaces;
internal interface ICompletionDataService
{
    IReadOnlyList<CompletionData> Data { get; }

    void LoadData(DocumentType documentType);

    void SaveData();

    void Add(CompletionData data);

    void Remove(CompletionData data);
}
