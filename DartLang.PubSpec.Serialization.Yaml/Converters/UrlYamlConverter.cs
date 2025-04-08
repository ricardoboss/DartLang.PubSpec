using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace DartLang.PubSpec.Serialization.Yaml.Converters;

public class UrlYamlConverter : IYamlTypeConverter
{
	public static readonly UrlYamlConverter Instance = new();

	/// <inheritdoc />
	public bool Accepts(Type type) => type == typeof(Uri);

	/// <inheritdoc />
	public object? ReadYaml(IParser parser, Type type, ObjectDeserializer rootDeserializer)
	{
		if (type != typeof(Uri))
			throw new NotSupportedException("Cannot deserialize to type " + type);

		if (!parser.TryConsume<Scalar>(out var raw))
			return null;

		return new Uri(raw.Value);
	}

	/// <inheritdoc />
	public void WriteYaml(IEmitter emitter, object? value, Type type, ObjectSerializer serializer)
	{
		if (type != typeof(Uri))
			throw new NotSupportedException("Cannot serialize type " + type);

		if (value is Uri uri)
			emitter.Emit(new Scalar(uri.ToString()));
		else
			emitter.Emit(new Scalar(string.Empty));
	}
}
