using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DartLang.PubSpec.Serialization.Json.Converters;

public class PlatformsJsonConverter : JsonConverter<Platforms>
{
	public override Platforms Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		if (typeToConvert != typeof(Platforms))
			throw new JsonException($"Unable to deserialize objects of type {typeToConvert}");

		if (reader.TokenType != JsonTokenType.StartObject)
			throw new JsonException($"Expected start of object, got {reader.TokenType}");

		var platforms = Platforms.None;
		while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
		{
			if (reader.TokenType != JsonTokenType.PropertyName)
				throw new JsonException("Expected property name");

			var name = reader.GetString()!;

			reader.Read();

			Debug.Assert(reader.TokenType == JsonTokenType.Null, "Platforms values should currently always be null");

			platforms |= name switch
			{
				"android" => Platforms.AndroidOnly,
				"ios" => Platforms.iOSOnly,
				"linux" => Platforms.LinuxOnly,
				"macos" => Platforms.MacOSOnly,
				"windows" => Platforms.WindowsOnly,
				"web" => Platforms.WebOnly,
				_ => throw new JsonException($"Unknown platform: {name}"),
			};
		}

		return platforms;
	}

	public override void Write(Utf8JsonWriter writer, Platforms value, JsonSerializerOptions options)
	{
		throw new NotImplementedException();
	}
}
