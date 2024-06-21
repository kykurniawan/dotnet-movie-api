using MovieApi.Domain.Movie.Repositories;
using MovieApi.Exceptions;
using MovieApi.Http.Requests.Tag;
using MovieApi.Infrastructure.Database;

namespace MovieApi.Domain.Movie.Services;

public class TagService(TagRepository tagRepository, MovieTagRepository movieTagRepository)
{
    protected readonly TagRepository _tagRepository = tagRepository;
    protected readonly MovieTagRepository _movieTagRepository = movieTagRepository;

    public Models.Tag Create(TagCreateRequest tagCreateRequest)
    {
        var tag = new Models.Tag
        {
            Name = tagCreateRequest.Name
        };

        return _tagRepository.Create(tag);
    }

    public Models.Tag Update(TagUpdateRequest tagUpdateRequest, Guid id)
    {
        var tag = Find(id);

        tag.Name = tagUpdateRequest.Name;

        return _tagRepository.Update(tag);
    }

    public Models.Tag Find(Guid id)
    {
        Models.Tag tag = _tagRepository.Find(id) ?? throw new DataNotFoundException("Tag not found");

        return tag;
    }

    public IPaginationResult<Models.Tag> FindAll(TagIndexRequest request)
    {
        return _tagRepository.Paginate(request);
    }

    public void Delete(Guid id)
    {
        var tag = Find(id);

        if (tag.MovieTags.Count > 0)
        {
            _movieTagRepository.DeleteMany(tag.MovieTags);
        }

        _tagRepository.Delete(tag);
    }
}