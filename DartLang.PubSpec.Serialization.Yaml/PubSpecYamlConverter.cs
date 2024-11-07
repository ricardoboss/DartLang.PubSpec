using System.Diagnostics.CodeAnalysis;
using DartLang.PubSpec.Serialization.Yaml.Extensions;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace DartLang.PubSpec.Serialization.Yaml;

[SuppressMessage("ReSharper", "UnusedMember.Global", Justification = "Entrypoints")]
[SuppressMessage("ReSharper", "MemberCanBePrivate.Global", Justification = "Entrypoints")]
public static class PubSpecYamlConverter
{
	public static PubSpec Deserialize(string yaml)
	{
		using var reader = new StringReader(yaml);

		return Deserialize(reader);
	}

	public static PubSpec Deserialize(Stream yaml)
	{
		using var reader = new StreamReader(yaml);

		return Deserialize(reader);
	}

	public static async Task<PubSpec> DeserializeAsync(Stream utf8Yaml, CancellationToken cancellationToken = default)
	{
		using var reader = new StreamReader(utf8Yaml);

		var yamlString = await reader.ReadToEndAsync(cancellationToken);

		return Deserialize(yamlString);
	}

	public static PubSpec Deserialize(TextReader reader)
	{
		var deserializer = new DeserializerBuilder()
			.IgnoreUnmatchedProperties()
			.WithNamingConvention(UnderscoredNamingConvention.Instance)
			.WithPubSpecConverters()
			.Build();

		return deserializer.Deserialize<PubSpec>(reader);
	}

	public static string Serialize(PubSpec pubspec)
	{
		var serializer = new SerializerBuilder()
			.WithNamingConvention(UnderscoredNamingConvention.Instance)
			.WithPubSpecConverters()
			.Build();

		return serializer.Serialize(pubspec);
	}

	public static async Task SerializeAsync(Stream utf8Yaml, PubSpec pubspec, CancellationToken cancellationToken = default)
	{
		cancellationToken.ThrowIfCancellationRequested();

		await using var writer = new StreamWriter(utf8Yaml);

		var yamlString = Serialize(pubspec);

		await writer.WriteAsync(yamlString);
	}
}
