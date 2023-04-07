using KeyGenerator;
using NLog;
using NLog.Config;
using NLog.Targets;
using System.Reflection;
using System.Text.Json;

class Program
{
    private static string logo = @"
██████╗ ███████╗ █████╗     ██╗  ██╗███████╗██╗   ██╗                                                                 
██╔══██╗██╔════╝██╔══██╗    ██║ ██╔╝██╔════╝╚██╗ ██╔╝                                                                 
██████╔╝███████╗███████║    █████╔╝ █████╗   ╚████╔╝                                                                  
██╔══██╗╚════██║██╔══██║    ██╔═██╗ ██╔══╝    ╚██╔╝                                                                   
██║  ██║███████║██║  ██║    ██║  ██╗███████╗   ██║                                                                    
╚═╝  ╚═╝╚══════╝╚═╝  ╚═╝    ╚═╝  ╚═╝╚══════╝   ╚═╝                                                                    
                                                                                                                      
 ██████╗ ███████╗███╗   ██╗███████╗██████╗  █████╗ ████████╗ ██████╗ ██████╗                                          
██╔════╝ ██╔════╝████╗  ██║██╔════╝██╔══██╗██╔══██╗╚══██╔══╝██╔═══██╗██╔══██╗                                         
██║  ███╗█████╗  ██╔██╗ ██║█████╗  ██████╔╝███████║   ██║   ██║   ██║██████╔╝                                         
██║   ██║██╔══╝  ██║╚██╗██║██╔══╝  ██╔══██╗██╔══██║   ██║   ██║   ██║██╔══██╗                                         
╚██████╔╝███████╗██║ ╚████║███████╗██║  ██║██║  ██║   ██║   ╚██████╔╝██║  ██║                                         
 ╚═════╝ ╚══════╝╚═╝  ╚═══╝╚══════╝╚═╝  ╚═╝╚═╝  ╚═╝   ╚═╝    ╚═════╝ ╚═╝  ╚═╝                                         
                                                                                                                      
██████╗ ██╗   ██╗    ██╗   ██╗ ██████╗ ██╗     ██████╗ ███████╗██████╗ ████████╗██╗███╗   ██╗ ██████╗ ███████╗██████╗ 
██╔══██╗╚██╗ ██╔╝    ██║   ██║██╔═══██╗██║     ██╔══██╗██╔════╝██╔══██╗╚══██╔══╝██║████╗  ██║██╔════╝ ██╔════╝██╔══██╗
██████╔╝ ╚████╔╝     ██║   ██║██║   ██║██║     ██████╔╝█████╗  ██████╔╝   ██║   ██║██╔██╗ ██║██║  ███╗█████╗  ██████╔╝
██╔══██╗  ╚██╔╝      ╚██╗ ██╔╝██║   ██║██║     ██╔═══╝ ██╔══╝  ██╔══██╗   ██║   ██║██║╚██╗██║██║   ██║██╔══╝  ██╔══██╗
██████╔╝   ██║        ╚████╔╝ ╚██████╔╝███████╗██║     ███████╗██║  ██║   ██║   ██║██║ ╚████║╚██████╔╝███████╗██║  ██║
╚═════╝    ╚═╝         ╚═══╝   ╚═════╝ ╚══════╝╚═╝     ╚══════╝╚═╝  ╚═╝   ╚═╝   ╚═╝╚═╝  ╚═══╝ ╚═════╝ ╚══════╝╚═╝  ╚═╝
                                                                                                                                   
                                                                                                                                      
";

    private static LoggingConfiguration config = new LoggingConfiguration();
    private static Logger logger = NLog.LogManager.GetCurrentClassLogger();

    private static int Success = 0;
    private static int SettingsError = 1;
    private static int JsonFormatError = 2;
    private static int OutputFileExistsError = 3;
    static int Main(string[] args)
    {
        // logger settings
        config.AddRule(LogLevel.Debug, LogLevel.Fatal, new ConsoleTarget
        {
            Name = "console",
            Layout = "${longdate}|${level:uppercase=true}|${logger}|${message}",
        }, "*");
        LogManager.Configuration = config;

        Console.WriteLine(logo);
        Settings? settings = null;
        string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? "", @"Settings.json");
        try
        {
            string jsonString = File.ReadAllText(path);
            settings = JsonSerializer.Deserialize<Settings>(jsonString);
        }
        catch
        {
            logger.Error(String.Format("Can`t find Settings.json at path {0}", path));
            return SettingsError;
        }

        if (settings is null)
        {
            logger.Error("Invalid Settings.json file. Check Readme!");
            return JsonFormatError;
        }

        logger.Info("Settings loaded successfully. Starting generation");

        if (File.Exists(settings.outputPath))
        {
            logger.Error(String.Format("File with path {0} Already exists!", settings.outputPath));
            return OutputFileExistsError;
        }

        using (var fsi = new StreamWriter(settings.outputPath))
        {
            var firstPrime = 0ul;
            var secondPrime = 0ul;
            Generator.GeneratePrimesTuple(out firstPrime, out secondPrime, settings.minPrimeValue,
                settings.maxPrimeValue);
            var openKeyNumber = firstPrime * secondPrime;
            var euler = RSA.Utils.EulerByFactoriation(new List<RSA.NumberFactor> {
                new(firstPrime, 1), new(secondPrime, 1)});
            var openKeyRelPrime = Generator.GenerateRelativelyPrime((ulong)euler, settings.minPrimeValue);
            var secretKey = RSA.Utils.GetReverse(openKeyRelPrime, (ulong)euler);

            fsi.WriteLine(String.Format("First prime: {0}\nSecond prime: {1}\n" +
                "OpenKey [Number]: {2}\nEuler: {3}\nOpenKey [RelativePrime]: {4}\nSecret key: {5}",
                firstPrime, secondPrime, openKeyNumber, euler, openKeyRelPrime, secretKey));
            fsi.Close();
        }

        logger.Info("Generation finished successfully!");
        return Success;
    }
}