using DartLang.PubSpec.Serialization.Yaml.Converters;
using YamlDotNet.Serialization;

namespace DartLang.PubSpec.Serialization.Yaml.Extensions;

public static class SerializerSkeletonExtensions
{
	public static T WithPubSpecConverters<T>(this BuilderSkeleton<T> builder) where T : BuilderSkeleton<T>
	{
		return builder
			.WithTypeConverter(SemVersionYamlConverter.Instance)
			.WithTypeConverter(DependencyMapYamlConverter.Instance)
			.WithTypeConverter(PlatformsYamlConverter.Instance);
	}
}
