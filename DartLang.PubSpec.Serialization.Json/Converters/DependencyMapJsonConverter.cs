using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;
using DartLang.PubSpec.Dependencies;
using Semver;

namespace DartLang.PubSpec.Serialization.Json.Converters;

public class DependencyMapJsonConverter : JsonConverter<DependencyMap>
{
	public override DependencyMap? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		if (typeToConvert != typeof(DependencyMap))
			throw new JsonException($"Unable to deserialize objects of type {typeToConvert}");

		if (reader.TokenType != JsonTokenType.StartObject)
			throw new JsonException($"Expected start of object, got {reader.TokenType}");

		var dependencies = new DependencyMap();

		while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
		{
			if (reader.TokenType != JsonTokenType.PropertyName)
				throw new JsonException("Expected property name");

			var name = reader.GetString()!;

			reader.Read();

			var dependency = ReadDependencyValue(ref reader);

			dependencies.Add(name, dependency);
		}

		return dependencies;
	}

	private static Dependency ReadDependencyValue(ref Utf8JsonReader reader)
	{
		if (reader.TokenType == JsonTokenType.String)
			return new VersionDependency
			{
				Version = InterpretVersion(reader.GetString()),
			};

		if (reader.TokenType != JsonTokenType.StartObject)
			throw new JsonException("Expected a string or an object");

		reader.Read(); // Move to the first property inside the object

		if (reader.TokenType != JsonTokenType.PropertyName)
			throw new JsonException("Expected a property name");

		var firstKey = reader.GetString()!;

		reader.Read();

		Dependency dependency = firstKey switch
		{
			"hosted" => ReadHostedDependency(ref reader),
			"git" => ReadGitDependency(ref reader),
			"path" => ReadPathDependency(ref reader),
			"sdk" => ReadSdkDependency(ref reader),
			_ => throw new JsonException($"Unknown dependency type: {firstKey}"),
		};

		reader.Read();

		if (reader.TokenType != JsonTokenType.EndObject)
			throw new JsonException("Expected end of object");

		return dependency;
	}

	private static SemVersionRange InterpretVersion(string? version)
	{
		return version == null || string.Equals(version, "any", StringComparison.InvariantCultureIgnoreCase)
			? SemVersionRange.All
			: SemVersionRange.Parse(version, SemVersionRangeOptions.Loose);
	}

	private static HostedDependency ReadHostedDependency(ref Utf8JsonReader reader)
	{
		Debug.Assert(reader.TokenType == JsonTokenType.String);

		var url = new Uri(reader.GetString()!);

		SemVersionRange? version = null;
		if (reader.Read() && reader.TokenType == JsonTokenType.PropertyName && reader.GetString() == "version")
		{
			reader.Read(); // Move to the version value
			version = InterpretVersion(reader.GetString());
		}

		return new()
		{
			Hosted = url,
			Version = version,
		};
	}

	private static GitDependency ReadGitDependency(ref Utf8JsonReader reader)
	{
		string? url = null;
		string? refSpec = null;
		string? path = null;

		if (reader.TokenType == JsonTokenType.StartObject)
		{
			while (reader.Read() && reader.TokenType == JsonTokenType.PropertyName)
			{
				switch (reader.GetString())
				{
					case "url":
						reader.Read(); // Move to url value
						url = reader.GetString()!;
						break;
					case "ref":
						reader.Read(); // Move to ref value
						refSpec = reader.GetString();
						break;
					case "path":
						reader.Read(); // Move to path value
						path = reader.GetString();
						break;
					default:
						throw new JsonException($"Unknown key: {reader.GetString()}");
				}
			}

			if (url == null)
				throw new JsonException("Expected a object to have a url property");

			if (reader.TokenType != JsonTokenType.EndObject)
				throw new JsonException("Expected end of object");
		}
		else if (reader.TokenType == JsonTokenType.String)
		{
			url = reader.GetString()!;
		}
		else
		{
			throw new JsonException("Expected a string or an object");
		}

		return new()
		{
			Git = url,
			Ref = refSpec,
			Path = path,
		};
	}

	private static PathDependency ReadPathDependency(ref Utf8JsonReader reader)
	{
		if (reader.TokenType != JsonTokenType.String)
			throw new JsonException("Expected a string");

		var path = reader.GetString()!;
		return new() { Path = path };
	}

	private static SdkDependency ReadSdkDependency(ref Utf8JsonReader reader)
	{
		if (reader.TokenType != JsonTokenType.String)
			throw new JsonException("Expected a string");

		var sdk = reader.GetString()!;
		return new() { Sdk = sdk };
	}

	public override void Write(Utf8JsonWriter writer, DependencyMap value, JsonSerializerOptions options)
	{
		throw new NotImplementedException();
	}
}
