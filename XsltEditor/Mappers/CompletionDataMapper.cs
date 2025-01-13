using System.Runtime.Versioning;

using XsltEditor.DTO;
using XsltEditor.Models;

namespace XsltEditor.Mappers;

[SupportedOSPlatform("windows")]
public static class CompletionDataMapper
{
    public static CompletionDataDto ToDto(CompletionData completionData)
    {
        return new CompletionDataDto
        {
            Text = completionData.Text
        };
    }

    public static CompletionData ToModel(CompletionDataDto dto)
    {
        return new CompletionData(dto.Text);
    }

    public static IEnumerable<CompletionDataDto> ToDtoList(IEnumerable<CompletionData> completionDataList)
    {
        return completionDataList.Select(ToDto);
    }

    public static IEnumerable<CompletionData> ToModelList(IEnumerable<CompletionDataDto> dtoList)
    {
        return dtoList.Select(ToModel);
    }
}