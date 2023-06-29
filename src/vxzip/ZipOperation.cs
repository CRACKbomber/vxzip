
using CommandLine;

internal abstract class ZipOperation
{
    [Option('z', "zip", HelpText = "Zip file path", Required = true)]
    public string? ZipPath { get; set; }

    [Option('d', "dir", HelpText = "Directory to create or extract to", Required = true)]
    public string? WorkingDirectory { get; set; }

    [Option('v', "verbose", HelpText = "Verbose output", Default = false)]
    public bool Verbose { get; set; }
}
