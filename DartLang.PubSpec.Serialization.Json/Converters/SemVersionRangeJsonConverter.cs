using System.Text.Json;
using System.Text.Json.Serialization;
using Semver;

namespace DartLang.PubSpec.Serialization.Json.Converters;

public class SemVersionRangeJsonConverter : JsonConverter<SemVersionRange>
{
	public override SemVersionRange? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		var value = reader.GetString();
		return value is null
			? null
			: SemVersionRange.Parse(value, SemVersionRangeOptions.Strict);
	}

	public override void Write(Utf8JsonWriter writer, SemVersionRange value, JsonSerializerOptions options)
	{
		writer.WriteStringValue(value.ToString());
	}
}
