using Iceni.Lib.Models.Dto;

namespace Iceni.Admin.Wasm.Services;

public class LessonService
{
    private readonly ApiClient _client;

    private IEnumerable<LessonDto> _lessons;

    public LessonService(ApiClient client)
    {
        _client = client;
        _lessons = Enumerable.Empty<LessonDto>();
    }
}