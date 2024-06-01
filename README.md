# DartLang.PubSpec

A library to read Dart pubspec.yaml files.

## Usage

```csharp
var yaml = File.ReadAllText("pubspec.yaml");
var pubspec = PubSpec.Deserialize(yaml);

Console.WriteLine($"Name: {pubspec.Name}");
Console.WriteLine($"Version: {pubspec.Version}");
```

### Custom Deserializer

You can also build your own deserializer using [`YamlDotNet`] directly:

```csharp
var deserializer = new DeserializerBuilder()
    .IgnoreUnmatchedProperties()                                // pub.dev will ignore other properties
    .WithNamingConvention(UnderscoredNamingConvention.Instance) // dart uses underscores for properties
    .WithPubSpecConverters()                                    // converters for custom types
    .Build();

return deserializer.Deserialize<PubSpec>(reader);
```

### Serialization

Serialization is currently not supported, but should not be hard to implement yourself.

## License

MIT

[`YamlDotNet`]: https://github.com/aaubry/YamlDotNet
