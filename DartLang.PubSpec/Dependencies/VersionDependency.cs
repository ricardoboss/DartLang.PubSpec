using Semver;

namespace DartLang.PubSpec.Dependencies;

public class VersionDependency : Dependency
{
	public required SemVersionRange Version { get; init; }
}
