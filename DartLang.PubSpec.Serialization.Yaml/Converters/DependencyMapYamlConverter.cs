using DartLang.PubSpec.Dependencies;
using Semver;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace DartLang.PubSpec.Serialization.Yaml.Converters;

public class DependencyMapYamlConverter : IYamlTypeConverter
{
	public static readonly DependencyMapYamlConverter Instance = new();

	public bool Accepts(Type type) => type == typeof(DependencyMap);

	public object? ReadYaml(IParser parser, Type type, ObjectDeserializer rootDeserializer)
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

	private static SemVersionRange InterpretVersion(Scalar scalar)
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

	public void WriteYaml(IEmitter emitter, object? value, Type type, ObjectSerializer serializer)
	{
		if (type != typeof(DependencyMap))
			throw new YamlException($"Cannot serialize {value?.GetType().Name}");

		if (value is not DependencyMap map)
		{
			emitter.Emit(new Scalar(string.Empty));

			return;
		}

		emitter.Emit(new MappingStart());

		foreach (var dependency in map)
		{
			emitter.Emit(new Scalar(dependency.Key));

			switch (dependency.Value)
			{
				case VersionDependency versionDependency:
					emitter.Emit(new Scalar(versionDependency.Version.ToString()));
					break;

				case HostedDependency hostedDependency:
					WriteHostedDependency(emitter, hostedDependency);
					break;

				case GitDependency gitDependency:
					WriteGitDependency(emitter, gitDependency);
					break;

				case PathDependency pathDependency:
					WritePathDependency(emitter, pathDependency);
					break;

				case SdkDependency sdkDependency:
					WriteSdkDependency(emitter, sdkDependency);
					break;

				default:
					throw new YamlException($"Unsupported dependency type: {dependency.Value.GetType()}");
			}
		}

		emitter.Emit(new MappingEnd());
	}

	private static void WriteSdkDependency(IEmitter emitter, SdkDependency sdkDependency)
	{
		emitter.Emit(new MappingStart());
		emitter.Emit(new Scalar("sdk"));
		emitter.Emit(new Scalar(sdkDependency.Sdk));
		emitter.Emit(new MappingEnd());
	}

	private static void WritePathDependency(IEmitter emitter, PathDependency pathDependency)
	{
		emitter.Emit(new MappingStart());
		emitter.Emit(new Scalar("path"));
		emitter.Emit(new Scalar(pathDependency.Path));
		emitter.Emit(new MappingEnd());
	}

	private static void WriteGitDependency(IEmitter emitter, GitDependency gitDependency)
	{
		emitter.Emit(new MappingStart());
		emitter.Emit(new Scalar("git"));

		if (gitDependency.Ref is not null && gitDependency.Path is not null)
		{
			// Complex git dependency with ref or path
			emitter.Emit(new MappingStart());
			emitter.Emit(new Scalar("url"));
			emitter.Emit(new Scalar(gitDependency.Git));
			if (gitDependency.Ref is not null)
			{
				emitter.Emit(new Scalar("ref"));
				emitter.Emit(new Scalar(gitDependency.Ref));
			}

			if (gitDependency.Path is not null)
			{
				emitter.Emit(new Scalar("path"));
				emitter.Emit(new Scalar(gitDependency.Path));
			}

			emitter.Emit(new MappingEnd());
		}
		else if (gitDependency.Ref is not null)
		{
			// Simple git URL
			emitter.Emit(new Scalar(gitDependency.Git));
			emitter.Emit(new Scalar(gitDependency.Ref));
		}
		else
		{
			// Simple git URL
			emitter.Emit(new Scalar(gitDependency.Git));
		}

		emitter.Emit(new MappingEnd());
	}

	private static void WriteHostedDependency(IEmitter emitter, HostedDependency hostedDependency)
	{
		emitter.Emit(new MappingStart());
		emitter.Emit(new Scalar("hosted"));
		emitter.Emit(new Scalar(hostedDependency.Hosted.ToString()));
		if (hostedDependency.Version is not null)
		{
			emitter.Emit(new Scalar("version"));
			emitter.Emit(new Scalar(hostedDependency.Version.ToString()));
		}

		emitter.Emit(new MappingEnd());
	}
}
