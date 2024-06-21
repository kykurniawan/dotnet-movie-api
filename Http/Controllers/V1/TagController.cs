using System.Net;
using Microsoft.AspNetCore.Mvc;
using MovieApi.Domain.Movie.Services;
using MovieApi.Helpers;
using MovieApi.Http.Requests.Tag;
using MovieApi.Http.Responses;
using MovieApi.Http.Responses.Tag;
using MovieApi.Infrastructure.Database;

namespace MovieApi.Http.Controllers.V1;

[Route("api/v1/tags")]
[ApiController]
public class TagController(TagService tagService)
{
    protected readonly TagService _tagService = tagService;

    [HttpGet()]
    public ApiResponse<IPaginationResult<TagResponse>> Index([FromQuery] TagIndexRequest request)
    {
        var result = _tagService.FindAll(request);

        return ApiResponseData.Create(HttpStatusCode.OK, TagResponse.MapPagination(result), "Tags retrieved successfully.");
    }

    [HttpGet("{id}")]
    public ApiResponse<TagResponse> Show([FromRoute] Guid id)
    {
        var tag = _tagService.Find(id);

        return ApiResponseData.Create(HttpStatusCode.OK, TagResponse.MapSingle(tag), "Tag retrieved successfully.");
    }

    [HttpPost()]
    public ApiResponse<TagResponse> Store([FromBody] TagCreateRequest request)
    {
        var tag = _tagService.Create(request);

        return ApiResponseData.Create(HttpStatusCode.Created, TagResponse.MapSingle(tag), "Tag created successfully.");
    }

    [HttpPut("{id}")]
    public ApiResponse<TagResponse> Update([FromBody] TagUpdateRequest request, [FromRoute] Guid id)
    {
        var tag = _tagService.Find(id);

        tag = _tagService.Update(request, tag.Id);

        return ApiResponseData.Create(HttpStatusCode.OK, TagResponse.MapSingle(tag), "Tag updated successfully.");
    }

    [HttpDelete("{id}")]
    public ApiResponse<object> Destroy([FromRoute] Guid id)
    {
        _tagService.Delete(id);

        return ApiResponseData.Create<object>(HttpStatusCode.NoContent, null, "Tag deleted successfully.");
    }
}