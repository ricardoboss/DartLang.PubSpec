using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace DartLang.PubSpec.Serialization.Json;

[SuppressMessage("ReSharper", "UnusedMember.Global", Justification = "Entrypoints")]
[SuppressMessage("ReSharper", "MemberCanBePrivate.Global", Justification = "Entrypoints")]
public static class PubSpecJsonConverter
{
	public static PubSpec? Deserialize(string json)
	{
		return JsonSerializer.Deserialize(json, PubSpecJsonSerializerContext.Default.PubSpec);
	}

	public static PubSpec? Deserialize(ReadOnlySpan<char> json)
	{
		return JsonSerializer.Deserialize(json, PubSpecJsonSerializerContext.Default.PubSpec);
	}

	public static PubSpec? Deserialize(Stream json)
	{
		return JsonSerializer.Deserialize(json, PubSpecJsonSerializerContext.Default.PubSpec);
	}

	public static async Task<PubSpec?> DeserializeAsync(Stream utf8Json, CancellationToken cancellationToken = default)
	{
		return await JsonSerializer.DeserializeAsync(utf8Json, PubSpecJsonSerializerContext.Default.PubSpec,
			cancellationToken);
	}

	public static string Serialize(PubSpec pubspec)
	{
		return JsonSerializer.Serialize(pubspec, PubSpecJsonSerializerContext.Default.PubSpec);
	}

	public static async Task SerializeAsync(Stream utf8Json, PubSpec pubspec,
		CancellationToken cancellationToken = default)
	{
		await JsonSerializer.SerializeAsync(utf8Json, pubspec, PubSpecJsonSerializerContext.Default.PubSpec,
			cancellationToken);
	}
}
