using DartLang.PubSpec.Serialization;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace DartLang.PubSpec.Tests.Serialization;

public sealed class PlatformsConverterTest
{
    [Test]
    public void TestConvertPlatforms()
    {
        // Arrange
        const string yaml = """
                            android:
                            ios:
                            web:
                            """;

        var input = new StringReader(yaml);
        var deserializer = new DeserializerBuilder()
            .IgnoreUnmatchedProperties()
            .WithNamingConvention(UnderscoredNamingConvention.Instance)
            .WithPubSpecConverters()
            .Build();

        // Act
        var platforms = deserializer.Deserialize<Platforms>(input);

        // Assert
        Assert.That(platforms, Is.Not.EqualTo(Platforms.None));
        Assert.Multiple(() =>
        {
            Assert.That(platforms & Platforms.AndroidOnly, Is.EqualTo(Platforms.AndroidOnly));
            Assert.That(platforms & Platforms.iOSOnly, Is.EqualTo(Platforms.iOSOnly));
            Assert.That(platforms & Platforms.WebOnly, Is.EqualTo(Platforms.WebOnly));
            Assert.That(platforms & Platforms.WindowsOnly, Is.EqualTo(Platforms.None));
            Assert.That(platforms & Platforms.MacOSOnly, Is.EqualTo(Platforms.None));
        });
    }

    [Test]
    public void TestConvertPlatformsFromPubSpec()
    {
        const string yaml = """
                            name: platforms

                            platforms:
                              android:
                              ios:
                              web:
                            """;

        var input = new StringReader(yaml);
        var deserializer = new DeserializerBuilder()
            .IgnoreUnmatchedProperties()
            .WithNamingConvention(UnderscoredNamingConvention.Instance)
            .WithPubSpecConverters()
            .Build();
        
        // Act
        var pubspec = deserializer.Deserialize<PubSpec>(input);

        // Assert
        Assert.That(pubspec.Platforms, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(pubspec.Platforms & Platforms.AndroidOnly, Is.EqualTo(Platforms.AndroidOnly));
            Assert.That(pubspec.Platforms & Platforms.iOSOnly, Is.EqualTo(Platforms.iOSOnly));
            Assert.That(pubspec.Platforms & Platforms.WebOnly, Is.EqualTo(Platforms.WebOnly));
            Assert.That(pubspec.Platforms & Platforms.WindowsOnly, Is.EqualTo(Platforms.None));
            Assert.That(pubspec.Platforms & Platforms.MacOSOnly, Is.EqualTo(Platforms.None));
        });
    }
}