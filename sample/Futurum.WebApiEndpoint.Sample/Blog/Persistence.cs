using Futurum.Core.Option;
using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.Blog;

public interface IBlogStorageBroker
{
    Task<Result<IEnumerable<Blog>>> GetAsync();
    Result<IAsyncEnumerable<Blog>> GetAsAsyncEnumerable();
    Task<Result<Blog>> GetByIdAsync(Id id);

    Task<Result<Blog>> AddAsync(Blog blog);
    Task<Result<IEnumerable<Blog>>> AddAsync(IEnumerable<Blog> blogs);

    Task<Result<Blog>> UpdateAsync(Blog blog);

    Task<Result> DeleteAsync(Id id);
}

public class BlogStorageBroker : IBlogStorageBroker
{
    private readonly List<Blog> _items = new();

    public async Task<Result<IEnumerable<Blog>>> GetAsync() =>
        _items.AsReadOnly().AsEnumerable().ToResultOk();

    public Result<IAsyncEnumerable<Blog>> GetAsAsyncEnumerable() =>
        _items.AsReadOnly().ToAsyncEnumerable().ToResultOk();

    public async Task<Result<Blog>> GetByIdAsync(Id id) =>
        _items.TrySingle(x => x.Id == id)
              .ToResultErrorKeyNotFound(id.ToString(), typeof(Blog).FullName);

    public async Task<Result<Blog>> AddAsync(Blog blog)
    {
        var newBlog = blog with
        {
            Id = ((long)_items.Count).ToId()
        };
        
        _items.Add(newBlog);

        return Result.Ok(newBlog);
    }

    public async Task<Result<IEnumerable<Blog>>> AddAsync(IEnumerable<Blog> blogs)
    {
        _items.AddRange(blogs);

        return Result.Ok(blogs);
    }

    public async Task<Result<Blog>> UpdateAsync(Blog blog)
    {
        return _items.TrySingle(x => x.Id == blog.Id)
                     .ToResult(() => $"Unable to find {nameof(Blog)} with Id : '{blog.Id}'")
                     .Then(existingBlog =>
                     {
                         _items.Remove(existingBlog);
                         _items.Add(blog);

                         return blog.ToResultOk();
                     });
    }

    public async Task<Result> DeleteAsync(Id id)
    {
        return _items.TrySingle(x => x.Id == id)
                     .ToResult(() => $"Unable to find {nameof(Blog)} with Id : '{id}'")
                     .Do(x => _items.Remove(x));
    }
}