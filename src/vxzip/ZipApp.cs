using CommandLine;
using Crack.xSource.Zip;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace vxzip
{
    internal static class ZipApp
    {
        internal static void Run(string[] args)
        {
            var parsedArgs = Parser.Default.ParseArguments<ExtractOptions, CreateOptions>(args);
            parsedArgs.WithParsed(options =>
            {
                if (options is ExtractOptions)
                    ExtractZip(options as ExtractOptions);
                else if (options is CreateOptions)
                    CreateZip(options as CreateOptions);
            });
        }

        private static void ConfigureLogger(ZipOperation ops)
        {
            var config = new LoggingConfiguration();
            LogLevel minLogLevel = ops.Verbose ? LogLevel.Trace : LogLevel.Info;

            // setup console target
            var consoleTarget = new ColoredConsoleTarget()
            {
                Layout = "${longdate}|${level:uppercase=true}|${message}${onexception:${newline}${exception:maxInnerExceptionLevel=10:format=shortType,message}}",
                UseDefaultRowHighlightingRules = false
            };


            consoleTarget.RowHighlightingRules.Add(new ConsoleRowHighlightingRule("level == LogLevel.Info",
                                                                                  ConsoleOutputColor.White,
                                                                                  ConsoleOutputColor.NoChange));
            consoleTarget.RowHighlightingRules.Add(new ConsoleRowHighlightingRule("level == LogLevel.Warn",
                                                                                  ConsoleOutputColor.Yellow,
                                                                                  ConsoleOutputColor.NoChange));
            consoleTarget.RowHighlightingRules.Add(new ConsoleRowHighlightingRule("level == LogLevel.Error",
                                                                                  ConsoleOutputColor.Red,
                                                                                  ConsoleOutputColor.NoChange));
            consoleTarget.RowHighlightingRules.Add(new ConsoleRowHighlightingRule("level == LogLevel.Fatal",
                                                                                  ConsoleOutputColor.Red,
                                                                                  ConsoleOutputColor.Yellow));

            if (ops.Verbose)
            {
                consoleTarget.RowHighlightingRules.Add(new ConsoleRowHighlightingRule("level == LogLevel.Trace",
                                                                                      ConsoleOutputColor.DarkGray,
                                                                                      ConsoleOutputColor.NoChange));
                consoleTarget.RowHighlightingRules.Add(new ConsoleRowHighlightingRule("level == LogLevel.Debug",
                                                                                      ConsoleOutputColor.Gray,
                                                                                      ConsoleOutputColor.NoChange));
            }

            config.AddTarget("console", consoleTarget);
            config.LoggingRules.Add(new LoggingRule("*", minLogLevel, consoleTarget));

            // Apply the configuration
            LogManager.Configuration = config;
        }
        private static void CreateZip(CreateOptions? ops)
        {
            ConfigureLogger(ops);

            try
            {
                XZP2File.Create(ops.ZipPath, ops.WorkingDirectory);
            }
            catch (Exception e)
            {
                LogManager.GetCurrentClassLogger().Fatal(e);
            }
        }

        private static void ExtractZip(ExtractOptions? ops)
        {
            ConfigureLogger(ops);
            try
            {
                new XZP2File(ops.ZipPath).ExtractWildCard(ops.WorkingDirectory, ops.SearchPattern);
            }
            catch (Exception e)
            {
                LogManager.GetCurrentClassLogger().Fatal(e);
            }
        }
    }
}
