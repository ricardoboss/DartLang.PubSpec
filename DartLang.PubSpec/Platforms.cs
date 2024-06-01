using System.Diagnostics.CodeAnalysis;

namespace DartLang.PubSpec;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public record Platforms(
    bool Android = false,
    bool iOS = false,
    bool Linux = false,
    bool MacOS = false,
    bool Windows = false,
    bool Web = false)
{
    public static Platforms operator &(Platforms a, Platforms b) => new(
        a.Android & b.Android,
        a.iOS & b.iOS,
        a.Linux & b.Linux,
        a.MacOS & b.MacOS,
        a.Windows & b.Windows,
        a.Web & b.Web);
    
    public static Platforms operator |(Platforms a, Platforms b) => new(
        a.Android | b.Android,
        a.iOS | b.iOS,
        a.Linux | b.Linux,
        a.MacOS | b.MacOS,
        a.Windows | b.Windows,
        a.Web | b.Web);

    public static Platforms AndroidOnly => new(Android: true);
    
    public static Platforms iOSOnly => new(iOS: true);
    
    public static Platforms LinuxOnly => new(Linux: true);
    
    public static Platforms MacOSOnly => new(MacOS: true);
    
    public static Platforms WindowsOnly => new(Windows: true);
    
    public static Platforms WebOnly => new(Web: true);

    public static Platforms None => new();
}
