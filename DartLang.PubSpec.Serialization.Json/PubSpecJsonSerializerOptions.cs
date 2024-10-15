using System.Text.Json.Serialization;
using DartLang.PubSpec.Dependencies;
using DartLang.PubSpec.Serialization.Json.Converters;

namespace DartLang.PubSpec.Serialization.Json;

[JsonSerializable(typeof(PubSpec))]
[JsonSerializable(typeof(DependencyMap))]
[JsonSerializable(typeof(VersionDependency))]
[JsonSerializable(typeof(SdkDependency))]
[JsonSerializable(typeof(HostedDependency))]
[JsonSerializable(typeof(GitDependency))]
[JsonSerializable(typeof(PathDependency))]
[JsonSerializable(typeof(Environment))]
[JsonSerializable(typeof(Platforms))]
[JsonSerializable(typeof(Screenshot))]
[JsonSourceGenerationOptions(
	Converters =
	[
		typeof(SemVersionJsonConverter),
		typeof(SemVersionRangeJsonConverter),
		typeof(DependencyMapJsonConverter),
		typeof(PlatformsJsonConverter),
	],
	PropertyNameCaseInsensitive = true,
	PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase
)]
public partial class PubSpecJsonSerializerOptions : JsonSerializerContext;
