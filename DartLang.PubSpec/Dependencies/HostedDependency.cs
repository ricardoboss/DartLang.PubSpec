using Semver;

namespace DartLang.PubSpec.Dependencies;

public class HostedDependency : Dependency
{
	public required Uri Hosted { get; init; }

	public SemVersionRange? Version { get; init; }
}
