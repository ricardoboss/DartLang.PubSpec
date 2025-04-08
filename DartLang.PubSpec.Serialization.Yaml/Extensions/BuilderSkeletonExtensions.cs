using DartLang.PubSpec.Serialization.Yaml.Converters;
using YamlDotNet.Serialization;

namespace DartLang.PubSpec.Serialization.Yaml.Extensions;

public static class BuilderSkeletonExtensions
{
	public static T WithPubSpecConverters<T>(this BuilderSkeleton<T> builder) where T : BuilderSkeleton<T>
	{
		return builder
			.WithTypeConverter(UrlYamlConverter.Instance)
			.WithTypeConverter(SemVersionYamlConverter.Instance)
			.WithTypeConverter(DependencyMapYamlConverter.Instance)
			.WithTypeConverter(PlatformsYamlConverter.Instance);
	}
}
