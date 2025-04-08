using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace DartLang.PubSpec.Serialization.Yaml.Converters;

public class PlatformsYamlConverter : IYamlTypeConverter
{
	public static readonly PlatformsYamlConverter Instance = new();

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
		if (type != typeof(Platforms))
			throw new YamlException($"Cannot serialize {value?.GetType().Name}");

		if (value is not Platforms platforms)
		{
			emitter.Emit(new Scalar(string.Empty));
			return;
		}

		emitter.Emit(new MappingStart());

		if (platforms.Android)
		{
			emitter.Emit(new Scalar("android"));
			emitter.Emit(new MappingStart());
			emitter.Emit(new MappingEnd());
		}

		if (platforms.iOS)
		{
			emitter.Emit(new Scalar("ios"));
			emitter.Emit(new MappingStart());
			emitter.Emit(new MappingEnd());
		}

		if (platforms.Linux)
		{
			emitter.Emit(new Scalar("linux"));
			emitter.Emit(new MappingStart());
			emitter.Emit(new MappingEnd());
		}

		if (platforms.MacOS)
		{
			emitter.Emit(new Scalar("macos"));
			emitter.Emit(new MappingStart());
			emitter.Emit(new MappingEnd());
		}

		if (platforms.Windows)
		{
			emitter.Emit(new Scalar("windows"));
			emitter.Emit(new MappingStart());
			emitter.Emit(new MappingEnd());
		}

		if (platforms.Web)
		{
			emitter.Emit(new Scalar("web"));
			emitter.Emit(new MappingStart());
			emitter.Emit(new MappingEnd());
		}

		emitter.Emit(new MappingEnd());
	}
}
