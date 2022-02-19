namespace Futurum.WebApiEndpoint;

public abstract class MapFromMultipartAttribute : Attribute
{
    protected MapFromMultipartAttribute(int sectionPosition, MapFromMultipart mapFromMultipart)
    {
        SectionPosition = sectionPosition;
        MapFromMultipart = mapFromMultipart;
    }

    public int SectionPosition { get; }
    public MapFromMultipart MapFromMultipart { get; }
}

/// <summary>
/// Specify property value should be mapped from multipart File
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public sealed class MapFromMultipartFileAttribute : MapFromMultipartAttribute
{
    public MapFromMultipartFileAttribute(int sectionPosition)
        : base(sectionPosition, MapFromMultipart.File)
    {
    }
}

/// <summary>
/// Specify property value should be mapped from multipart Json
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public sealed class MapFromMultipartJsonAttribute : MapFromMultipartAttribute
{
    public MapFromMultipartJsonAttribute(int sectionPosition)
        : base(sectionPosition, MapFromMultipart.Json)
    {
    }
}