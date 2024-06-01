using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using DartLang.PubSpec.Dependencies;
using Semver;

namespace DartLang.PubSpec;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global", Justification = "Instantiated by deserializer.")]
public class PubSpec
{
    [Required]
    public required string Name { get; init; }

    public SemVersion? Version { get; init; }

    public string? Description { get; init; }

    public Uri? Homepage { get; init; }

    public Uri? Repository { get; init; }

    public Uri? IssueTracker { get; init; }

    public Uri? Documentation { get; init; }

    public DependencyMap? Dependencies { get; init; }

    public DependencyMap? DevDependencies { get; init; }

    public Dictionary<string, string>? Executables { get; init; }

    public Platforms? Platforms { get; init; }

    public string? PublishTo { get; init; }

    public Uri[]? Funding { get; init; }

    public string[]? FalseSecrets { get; init; }

    public Screenshot[]? Screenshots { get; init; }

    public string[]? Topics { get; init; }

    public string[]? IgnoredAdvisories { get; init; }

    public Environment? Environment { get; init; }
}
