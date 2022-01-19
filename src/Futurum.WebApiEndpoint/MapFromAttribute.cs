namespace Futurum.WebApiEndpoint;

public abstract class MapFromAttribute : Attribute
{
    protected MapFromAttribute(string name, MapFrom mapFrom)
    {
        Name = name;
        MapFrom = mapFrom;
    }

    public string Name { get; }
    public MapFrom MapFrom { get; }
}

/// <summary>
/// Specify property value should be mapped from Path
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public sealed class MapFromPathAttribute : MapFromAttribute
{
    public MapFromPathAttribute(string name)
        : base(name, MapFrom.Path)
    {
    }
}

/// <summary>
/// Specify property value should be mapped from Query
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public sealed class MapFromQueryAttribute : MapFromAttribute
{
    public MapFromQueryAttribute(string name)
        : base(name, MapFrom.Query)
    {
    }
}

/// <summary>
/// Specify property value should be mapped from Cookie
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public sealed class MapFromCookieAttribute : MapFromAttribute
{
    public MapFromCookieAttribute(string name)
        : base(name, MapFrom.Cookie)
    {
    }
}

/// <summary>
/// Specify property value should be mapped from Header
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public sealed class MapFromHeaderAttribute : MapFromAttribute
{
    public MapFromHeaderAttribute(string name)
        : base(name, MapFrom.Header)
    {
    }
}

/// <summary>
/// Specify property value should be mapped from Form
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public sealed class MapFromFormAttribute : MapFromAttribute
{
    public MapFromFormAttribute(string name)
        : base(name, MapFrom.Form)
    {
    }
}