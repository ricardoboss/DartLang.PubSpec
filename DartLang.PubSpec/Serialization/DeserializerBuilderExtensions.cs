using YamlDotNet.Serialization;

namespace DartLang.PubSpec.Serialization;

public static class DeserializerBuilderExtensions
{
    public static DeserializerBuilder WithPubSpecConverters(this DeserializerBuilder builder)
    {
        return builder
            .WithTypeConverter(SemVersionConverter.Instance)
            .WithTypeConverter(DependencyMapConverter.Instance)
            .WithTypeConverter(PlatformsConverter.Instance);
    }
}