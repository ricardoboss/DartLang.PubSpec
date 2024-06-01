using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using DartLang.PubSpec.Dependencies;
using DartLang.PubSpec.Serialization;
using Semver;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace DartLang.PubSpec;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global", Justification = "Instantiated by deserializer.")]
public class PubSpec
{
    public static PubSpec Deserialize(string yaml)
    {
        using var reader = new StringReader(yaml);

        return Deserialize(reader);
    }

    public static PubSpec Deserialize(StringReader reader)
    {
        var deserializer = new DeserializerBuilder()
            .IgnoreUnmatchedProperties()
            .WithNamingConvention(UnderscoredNamingConvention.Instance)
            .WithPubSpecConverters()
            .Build();

        return deserializer.Deserialize<PubSpec>(reader);
    }

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
