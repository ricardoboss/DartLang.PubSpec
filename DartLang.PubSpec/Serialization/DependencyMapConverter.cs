using DartLang.PubSpec.Dependencies;
using Semver;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace DartLang.PubSpec.Serialization;

public class DependencyMapConverter : IYamlTypeConverter
{
    public static readonly DependencyMapConverter Instance = new();

    public bool Accepts(Type type) => type == typeof(DependencyMap);

    public object? ReadYaml(IParser parser, Type type)
    {
        if (!Accepts(type))
            throw new YamlException($"Cannot convert type {type.Name} to {nameof(DependencyMap)}");

        // check the following things:
        // 1. dependencies are always a map and the keys are strings
        // 2. the values are either a scalar or a map
        // 3. if the value is a scalar => VersionDependency
        // 4. depending on the first key of the map:
        //    a. 'hosted' => HostedDependency
        //    b. 'git' => GitDependency
        //    c. 'path' => PathDependency
        //    d. 'sdk' => SdkDependency

        if (!parser.TryConsume<MappingStart>(out _))
            return null;

        var dependencies = new DependencyMap();

        while (parser.Consume<Scalar>() is { } name)
        {
            var dependency = ReadDependencyValue(parser);

            dependencies.Add(name.Value, dependency);

            if (parser.TryConsume<MappingEnd>(out _))
                break;
        }

        return dependencies;
    }

    private static Dependency ReadDependencyValue(IParser parser)
    {
        if (parser.TryConsume<Scalar>(out var version))
            return new VersionDependency
            {
                Version = InterpretVersion(version)!,
            };

        if (!parser.TryConsume<MappingStart>(out _))
            throw new YamlException("Expected a mapping");

        var firstKey = parser.Consume<Scalar>().Value;
        Dependency dependency = firstKey switch
        {
            "hosted" => ReadHostedDependency(parser),
            "git" => ReadGitDependency(parser),
            "path" => ReadPathDependency(parser),
            "sdk" => ReadSdkDependency(parser),
            _ => throw new YamlException($"Unknown dependency type: {firstKey}"),
        };

        parser.Consume<MappingEnd>();

        return dependency;
    }

    private static SemVersionRange? InterpretVersion(Scalar scalar)
    {
        return scalar.Value.Equals("any", StringComparison.InvariantCultureIgnoreCase)
            ? SemVersionRange.All
            : SemVersionRange.Parse(scalar.Value, SemVersionRangeOptions.Loose);
    }

    private static HostedDependency ReadHostedDependency(IParser parser)
    {
        var rawUrl = parser.Consume<Scalar>().Value;
        var url = new Uri(rawUrl);

        SemVersionRange? version = null;
        if (parser.TryConsume<Scalar>(out var versionKey) && versionKey.Value == "version")
            version = InterpretVersion(parser.Consume<Scalar>());

        return new()
        {
            Hosted = url,
            Version = version,
        };
    }

    private static GitDependency ReadGitDependency(IParser parser)
    {
        string url;
        string? refSpec = null;
        string? path = null;

        if (parser.TryConsume<MappingStart>(out _))
        {
            var urlKey = parser.Consume<Scalar>();
            if (urlKey.Value != "url")
                throw new YamlException("Expected a 'url' key");

            url = parser.Consume<Scalar>().Value;

            while (parser.TryConsume<Scalar>(out var key))
            {
                switch (key.Value)
                {
                    case "ref":
                        refSpec = parser.Consume<Scalar>().Value;
                        break;
                    case "path":
                        path = parser.Consume<Scalar>().Value;
                        break;
                    default:
                        throw new YamlException($"Unknown key: {key.Value}");
                }
            }

            parser.Consume<MappingEnd>();
        }
        else
        {
            url = parser.Consume<Scalar>().Value;
        }

        return new()
        {
            Git = url,
            Ref = refSpec,
            Path = path,
        };
    }

    private static PathDependency ReadPathDependency(IParser parser)
    {
        var path = parser.Consume<Scalar>().Value;

        return new()
        {
            Path = path,
        };
    }

    private static SdkDependency ReadSdkDependency(IParser parser)
    {
        var sdk = parser.Consume<Scalar>().Value;

        return new()
        {
            Sdk = sdk,
        };
    }

    public void WriteYaml(IEmitter emitter, object? value, Type type)
    {
        if (value is not DependencyMap map)
            throw new YamlException($"Cannot serialize {value?.GetType().Name}");

        throw new NotImplementedException();
    }
}