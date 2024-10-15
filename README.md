# DartLang.PubSpec

A library to serialize and deserialize Dart pubspec.yaml files.

## Usage

```csharp
var yaml = File.ReadAllText("pubspec.yaml");
var pubspec = PubSpecYamlSerializer.Deserialize(yaml);

Console.WriteLine($"Name: {pubspec.Name}");
Console.WriteLine($"Version: {pubspec.Version}");
```

You can also use the JSON serializer:

```csharp
var json = File.ReadAllText("pubspec.json");
var pubspec = PubSpecJsonSerializer.Deserialize(json);
```

### Custom Deserializer

#### YAML

You can also build your own deserializer using [`YamlDotNet`] directly:

```csharp
var deserializer = new DeserializerBuilder()
    .IgnoreUnmatchedProperties()                                // pub.dev will ignore other properties
    .WithNamingConvention(UnderscoredNamingConvention.Instance) // dart uses underscores for properties
    .WithPubSpecConverters()                                    // converters for custom types
    .Build();

return deserializer.Deserialize<PubSpec>(reader);
```

#### JSON

In case you need to use custom `JsonSerializerOptions`, you can reuse the converters from `DartLang.PubSpec.Serialization.Json`:

```csharp
var json = File.ReadAllText("pubspec.json");
var options = new JsonSerializerOptions
{
    PropertyNameCaseInsensitive = true,
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    Converters =
    {
        new SemVersionJsonConverter(),
        new SemVersionRangeJsonConverter(),
        new DependencyMapJsonConverter(),
        new PlatformsJsonConverter(),
    },
};

return JsonSerializer.Deserialize<PubSpec>(json, options);
```

### Serialization

Serialization is currently not supported, but should not be hard to implement yourself.

## License

MIT

[`YamlDotNet`]: https://github.com/aaubry/YamlDotNet
