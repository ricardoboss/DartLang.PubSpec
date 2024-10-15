using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace DartLang.PubSpec.Serialization;

public class PlatformsConverter : IYamlTypeConverter
{
	public static readonly PlatformsConverter Instance = new();

	public bool Accepts(Type type) => type == typeof(Platforms);

	public object ReadYaml(IParser parser, Type type, ObjectDeserializer rootDeserializer)
	{
		parser.Consume<MappingStart>();

		var platforms = Platforms.None;
		while (parser.TryConsume<Scalar>(out var key))
		{
			_ = parser.Consume<Scalar>();

			platforms |= key.Value switch
			{
				"android" => Platforms.AndroidOnly,
				"ios" => Platforms.iOSOnly,
				"linux" => Platforms.LinuxOnly,
				"macos" => Platforms.MacOSOnly,
				"windows" => Platforms.WindowsOnly,
				"web" => Platforms.WebOnly,
				_ => throw new YamlException($"Unknown platform: {key.Value}"),
			};
		}

		parser.Consume<MappingEnd>();

		return platforms;
	}

	public void WriteYaml(IEmitter emitter, object? value, Type type, ObjectSerializer serializer)
	{
		if (value is not Platforms platforms)
			throw new YamlException($"Cannot serialize {value?.GetType().Name}");

		throw new NotImplementedException();
	}
}
