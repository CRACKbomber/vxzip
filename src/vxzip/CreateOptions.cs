
using CommandLine;

[Verb("create", HelpText = "Create a new XZP pack file")]
internal class CreateOptions : ZipOperation
{
    [Option('p', "pad", HelpText = "Size to pad to (in bytes) when creating the zip file.")]
    public int PadSize { get; set; }
}
