namespace DartLang.PubSpec.Dependencies;

public class GitDependency : Dependency
{
    public required string Git { get; init; }

    public string? Ref { get; init; }

    public string? Path { get; init; }
}