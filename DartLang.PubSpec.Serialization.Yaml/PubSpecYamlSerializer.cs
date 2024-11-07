using System.Diagnostics.CodeAnalysis;
using DartLang.PubSpec.Serialization.Yaml.Extensions;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace DartLang.PubSpec.Serialization.Yaml;

[SuppressMessage("ReSharper", "UnusedMember.Global", Justification = "Entrypoints")]
[SuppressMessage("ReSharper", "MemberCanBePrivate.Global", Justification = "Entrypoints")]
public static class PubSpecYamlSerializer
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

	public static PubSpec Deserialize(TextReader reader)
	{
		var deserializer = new DeserializerBuilder()
			.IgnoreUnmatchedProperties()
			.WithNamingConvention(UnderscoredNamingConvention.Instance)
			.WithPubSpecConverters()
			.Build();

		return deserializer.Deserialize<PubSpec>(reader);
	}
}
