using System.Text.Json;

namespace DartLang.PubSpec.Serialization.Json;

public static class PubSpecJsonSerializer
{
	public static PubSpec? Deserialize(string json)
	{
		return JsonSerializer.Deserialize(json, PubSpecJsonSerializerOptions.Default.PubSpec);
	}
}
