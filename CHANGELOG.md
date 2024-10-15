# Unreleased

* Split de-/serialization for YAML into a separate package: `DartLang.PubSpec.Serialization.Yaml`
* Added support for deserializing JSON files using `DartLang.PubSpec.Serialization.Json`

# 1.1.0

* Upgraded to YamlDotNet 16.1.3

# 1.0.0

* Initial release 🎉
* Support for deserializing pubspec.yaml files using `PubSpec.Deserialize(String yaml)` and `PubSpec.Deserialize(StringReader reader)`.
  * You can also build your own deserializer using:
    ```csharp
    var deserializer = new DeserializerBuilder()
        .IgnoreUnmatchedProperties()                                // pub.dev will ignore other properties
        .WithNamingConvention(UnderscoredNamingConvention.Instance) // convention
        .WithPubSpecConverters()                                    // converters for custom types
        .Build();

    return deserializer.Deserialize<PubSpec>(reader);
    ```
