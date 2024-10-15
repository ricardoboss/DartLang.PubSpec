using System.ComponentModel.DataAnnotations;

namespace DartLang.PubSpec;

public class Screenshot
{
	[Required]
	public required string Description { get; init; }

	[Required]
	public required string Path { get; init; }
}
