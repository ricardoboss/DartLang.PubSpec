namespace DartLang.PubSpec.Serialization.Json.Tests;

public sealed class DeserializesExamples
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
	public void TestDeserializesExample(string packageName, string yaml)
	{
		var pubspec = PubSpecJsonSerializer.Deserialize(yaml);

		Assert.That(pubspec, Is.Not.Null);
		Assert.That(pubspec.Name, Is.EqualTo(packageName));
	}
}
