
using CommandLine;

[Verb("extract", HelpText = "Extract the contents of an existing XZP pack file")]
internal class ExtractOptions : ZipOperation
{
    [Option('f', "filter", Default = "*")]
    public string SearchPattern { get; set; }
}
