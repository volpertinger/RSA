﻿using KeyGenerator;
using NLog;
using NLog.Config;
using NLog.Targets;
using System.Numerics;
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

        if (File.Exists(settings.OutputPath))
        {
            logger.Error(String.Format("File with path {0} Already exists!", settings.OutputPath));
            return OutputFileExistsError;
        }

        logger.Info("Settings loaded successfully. Starting generation");

        // main generation
        var firstPrime = new BigInteger(0);
        var secondPrime = new BigInteger(0);
        Generator.GeneratePrimesTuple(out firstPrime, out secondPrime, settings.MinPrimeValue,
            settings.MaxPrimeValue);
        logger.Info("Primes generated successfully!");

        var openKeyNumber = firstPrime * secondPrime;
        logger.Info("Open key number calculated successfully!");

        var euler = RSA.Utils.EulerByFactoriation(new List<RSA.NumberFactor> {
                new(firstPrime, 1), new(secondPrime, 1)});
        logger.Info("Euler function calculated successfully!");

        var openKeyRelPrime = Generator.GenerateRelativelyPrime(euler, settings.MinPrimeValue);
        logger.Info("Open key relative prime generated successfully!");

        var secretKey = RSA.Utils.GetReverse(openKeyRelPrime, euler);
        logger.Info("Generation finished successfully!");

        using (var fsi = new StreamWriter(settings.OutputPath))
        {
            fsi.WriteLine(String.Format("Modulo number: {0}\nOpen key [Relative prime]: {1}\n" +
                "Secret key [Reverse to relative prime]: {2}",
                openKeyNumber, openKeyRelPrime, secretKey));
            fsi.Close();
        }

        logger.Info("Generation finished successfully!");
        return Success;
    }
}