# Unreleased

* Added `workspaces` property
* Unmapped members should be skipped (JSON)

# 3.0.1

* (internal) Upgraded test packages
* Updated SemVer to 3.0.0
* `PubSpecYamlConverter::DeserializeAsync` now leaves the stream open by default (configurable using `leaveOpen` parameter)

# 3.0.0

* Breaking: Renamed `PubSpecJsonSerializer` to `PubSpecJsonConverter`
* Breaking: Renamed `PubSpecYamlSerializer` to `PubSpecYamlConverter`
* Added more and also async overloads for de-/serialization in `PubSpecJsonConverter` and `PubSpecYamlConverter`

# 2.0.0

* Split de-/serialization for YAML into a separate package: `DartLang.PubSpec.Serialization.Yaml`
* Added support for de-/serializing JSON files using `DartLang.PubSpec.Serialization.Json`

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
