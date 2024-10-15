using Semver;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace DartLang.PubSpec.Serialization.Yaml.Converters;

public class SemVersionYamlConverter : IYamlTypeConverter
{
	public static readonly SemVersionYamlConverter Instance = new();

	public bool Accepts(Type type) => type == typeof(SemVersion) || type == typeof(SemVersionRange);

	public object? ReadYaml(IParser parser, Type type, ObjectDeserializer rootDeserializer)
	{
		if (type == typeof(SemVersion))
			return ReadSemVersion(parser);

		if (type == typeof(SemVersionRange))
			return ReadSemVersionRange(parser);

		throw new NotSupportedException("Cannot deserialize to type " + type);
	}

	private static SemVersion? ReadSemVersion(IParser parser)
	{
		if (!parser.TryConsume<Scalar>(out var raw))
			return null;

		if (SemVersion.TryParse(raw.Value, SemVersionStyles.Any, out var semVersion))
			return semVersion;

		throw new YamlException("Invalid semantic version: " + raw.Value);
	}

	private static SemVersionRange? ReadSemVersionRange(IParser parser)
	{
		if (!parser.TryConsume<Scalar>(out var raw))
			return null;

		if (SemVersionRange.TryParse(raw.Value, SemVersionRangeOptions.Loose, out var semVersionRange))
			return semVersionRange;

		throw new YamlException("Invalid semantic version range: " + raw.Value);
	}

	public void WriteYaml(IEmitter emitter, object? value, Type type, ObjectSerializer serializer)
	{
		switch (value)
		{
			case SemVersion semVersion:
				WriteSemVersion(emitter, semVersion);
				break;
			case SemVersionRange semVersionRange:
				WriteSemVersionRange(emitter, semVersionRange);
				break;
			default:
				throw new NotSupportedException("Cannot serialize type " + type);
		}
	}

	private static void WriteSemVersion(IEmitter emitter, SemVersion value)
	{
		emitter.Emit(new Scalar(value.ToString() ?? string.Empty));
	}

	private static void WriteSemVersionRange(IEmitter emitter, SemVersionRange value)
	{
		emitter.Emit(new Scalar(value.ToString() ?? string.Empty));
	}
}
