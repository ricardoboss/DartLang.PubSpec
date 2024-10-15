namespace DartLang.PubSpec.Serialization.Json.Tests;

public sealed class ExamplesTests
{
	private static IEnumerable<TestCaseData> ExamplesProvider()
	{
		var fileWalker = Directory.EnumerateFiles("Examples", "*.json", new EnumerationOptions
		{
			IgnoreInaccessible = true,
			RecurseSubdirectories = true,
			MatchCasing = MatchCasing.CaseInsensitive,
			MaxRecursionDepth = 2,
		});

		foreach (var path in fileWalker)
		{
			var contents = File.ReadAllText(path);
			var packageName = Path.GetFileName(path)[..^".json".Length];

			var example = new TestCaseData(packageName, contents).SetName(Path.GetFileName(path));

			yield return example;
		}
	}

	[Test]
	[TestCaseSource(nameof(ExamplesProvider))]
	public void TestDeserializeSerialize(string packageName, string json)
	{
		var pubspec = PubSpecJsonSerializer.Deserialize(json);

		Assert.That(pubspec, Is.Not.Null);

		var serialized = PubSpecJsonSerializer.Serialize(pubspec);

		Assert.That(serialized, Is.Not.Null);
	}
}
