using NLog;
using NLog.Config;
using NLog.Targets;
using RSA;
using System.Numerics;
using System.Reflection;
using System.Text.Json;

class Program
{
    private static string logo = @"
██████╗ ███████╗ █████╗                                                                                               
██╔══██╗██╔════╝██╔══██╗                                                                                              
██████╔╝███████╗███████║                                                                                              
██╔══██╗╚════██║██╔══██║                                                                                              
██║  ██║███████║██║  ██║                                                                                              
╚═╝  ╚═╝╚══════╝╚═╝  ╚═╝                                                                                              
                                                                                                                      
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

        // key parsing from string representation
        BigInteger? openKey = null, secretKey = null;
        if (settings.SKey.OpenKey != null)
            openKey = BigInteger.Parse(settings.SKey.OpenKey!);
        if (settings.SKey.SecretKey != null)
            secretKey = BigInteger.Parse(settings.SKey.SecretKey!);
        var key = new Key(BigInteger.Parse(settings.SKey.ModNumber), openKey, secretKey);

        var rsa = new RSA.RSA(key);

        logger.Info("RSA processing started");

        foreach (var setting in settings.Operations)
        {
            if (File.Exists(setting.PathOutput))
            {
                logger.Error(String.Format("File with path {0} Already exists!", setting.PathInput));
                continue;
            }

            using (FileStream fsi = File.OpenRead(setting.PathInput))
            {
                using (FileStream fso = File.OpenWrite(setting.PathOutput))
                {
                    switch (setting.Operation)
                    {
                        case Operations.Encrypt:
                            logger.Info(String.Format("Start file {0} encryption, writing result to {1}",
                                setting.PathInput, setting.PathOutput));
                            rsa.Encrypt(fsi, fso);
                            logger.Info(String.Format("Encryption of {0} finished successfuly, result writed to {1}",
                                setting.PathInput, setting.PathOutput));
                            break;
                        case Operations.Decrypt:
                            logger.Info(String.Format("Start file {0} decryption, writing result to {1}",
                                setting.PathInput, setting.PathOutput));
                            rsa.Decrypt(fsi, fso);
                            logger.Info(String.Format("Decryption of {0} finished successfuly, result writed to {1}",
                                setting.PathInput, setting.PathOutput));
                            break;
                        default:
                            logger.Error("Something went wrong. Better pray.");
                            continue;
                    };
                    fso.Close();
                }
                fsi.Close();
            }
        }
        logger.Info("DES processing finished ");
        return Success;
    }
}