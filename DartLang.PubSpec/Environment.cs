using Semver;

namespace DartLang.PubSpec;

public class Environment
{
	public SemVersionRange? Sdk { get; init; }

	public SemVersionRange? Flutter { get; init; }
}
