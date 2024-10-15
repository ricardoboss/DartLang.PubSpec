using DartLang.PubSpec.Dependencies;
using DartLang.PubSpec.Serialization;
using Semver;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace DartLang.PubSpec.Tests.Serialization;

public sealed class DependencyMapConverterTest
{
	[Test]
	public void TestConvertSingleVersionDependency()
	{
		// Arrange
		const string yaml = "path: ^1.8.0";

		var input = new StringReader(yaml);
		var deserializer = new DeserializerBuilder()
			.IgnoreUnmatchedProperties()
			.WithNamingConvention(UnderscoredNamingConvention.Instance)
			.WithPubSpecConverters()
			.Build();

		// Act
		var map = deserializer.Deserialize<DependencyMap>(input);

		// Assert
		Assert.That(map, Is.Not.Null);
		Assert.That(map, Is.Not.Empty);
		Assert.That(map.First().Key, Is.EqualTo("path"));
		Assert.Multiple(() =>
		{
			Assert.That(map.First().Value, Is.InstanceOf<VersionDependency>()!);
			Assert.That(((VersionDependency)map.First().Value).Version, Is.EqualTo(SemVersionRange.Parse("^1.8.0")!));
		});
	}

	[Test]
	public void TestConvertMultipleVersionDependencies()
	{
		// Arrange
		const string yaml = """
                            path: ^1.8.0
                            intl: ^2.0.0
                            """;

		var input = new StringReader(yaml);
		var deserializer = new DeserializerBuilder()
			.IgnoreUnmatchedProperties()
			.WithNamingConvention(UnderscoredNamingConvention.Instance)
			.WithPubSpecConverters()
			.Build();

		// Act
		var map = deserializer.Deserialize<DependencyMap>(input);

		// Assert
		Assert.That(map, Is.Not.Null);
		Assert.That(map, Is.Not.Empty);

		Assert.Multiple(() =>
		{
			Assert.That(map.First().Key, Is.EqualTo("path"));
			Assert.That(map.Last().Key, Is.EqualTo("intl"));
		});

		Assert.Multiple(() =>
		{
			Assert.That(map.First().Value, Is.InstanceOf<VersionDependency>()!);
			Assert.That(((VersionDependency)map.First().Value).Version, Is.EqualTo(SemVersionRange.Parse("^1.8.0")!));
		});

		Assert.Multiple(() =>
		{
			Assert.That(map.Last().Value, Is.InstanceOf<VersionDependency>()!);
			Assert.That(((VersionDependency)map.Last().Value).Version, Is.EqualTo(SemVersionRange.Parse("^2.0.0")!));
		});
	}
}
