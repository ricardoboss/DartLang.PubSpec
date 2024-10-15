using System.Text.Json;

namespace DartLang.PubSpec.Serialization.Json;

public static class PubSpecJsonSerializer
{
	public static PubSpec? Deserialize(string json)
	{
		return JsonSerializer.Deserialize(json, PubSpecJsonSerializerContext.Default.PubSpec);
	}

	public static string Serialize(PubSpec pubspec)
	{
		return JsonSerializer.Serialize(pubspec, PubSpecJsonSerializerContext.Default.PubSpec);
	}
}
