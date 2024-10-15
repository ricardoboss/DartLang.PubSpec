using DartLang.PubSpec.Serialization.Yaml.Converters;
using YamlDotNet.Serialization;

namespace DartLang.PubSpec.Serialization.Yaml.Extensions;

public static class DeserializerBuilderExtensions
{
	public static DeserializerBuilder WithPubSpecConverters(this DeserializerBuilder builder)
	{
		return builder
			.WithTypeConverter(SemVersionYamlConverter.Instance)
			.WithTypeConverter(DependencyMapYamlConverter.Instance)
			.WithTypeConverter(PlatformsYamlConverter.Instance);
	}
}
