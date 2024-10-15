using System.Text.Json;
using System.Text.Json.Serialization;
using Semver;

namespace DartLang.PubSpec.Serialization.Json.Converters;

public class SemVersionJsonConverter : JsonConverter<SemVersion>
{
	public override SemVersion? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		var value = reader.GetString();
		return value is null
			? null
			: SemVersion.Parse(value, SemVersionStyles.Strict);
	}

	public override void Write(Utf8JsonWriter writer, SemVersion value, JsonSerializerOptions options)
	{
		writer.WriteStringValue(value.ToString());
	}
}
