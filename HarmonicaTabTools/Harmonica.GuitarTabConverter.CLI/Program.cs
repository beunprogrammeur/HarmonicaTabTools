using Harmonica.Core.Tuning;
using Harmonica.GuitarTabConverter.Core;
using Harmonica.GuitarTabConverter.Core.Guitar;
using System.Reflection;

internal class Program
{
    private static void Main(string[] args)
    {
        string inputFile = null;
        string outputFile = null;


        foreach(string arg in args)
        {
            if (arg.StartsWith("-in:"))
            {
                inputFile = arg.Substring(4);
            }
            else if (arg.StartsWith("-out:"))
            {
                outputFile = arg.Substring(5);
            }
        }

        if (string.IsNullOrEmpty(inputFile) || string.IsNullOrEmpty(outputFile) || !File.Exists(inputFile))
        {
            Console.WriteLine("arguments are incorrect or missing");
            return;
        }

        string configPath = "../../../../Harmonica.Core/Tunings/Richter_C.xml";
        var harmonicaTuningInC = TuningLoader.LoadTuningConfiguration(configPath);
        var harmonicaTuningInA = TuningLoader.Transpose(harmonicaTuningInC, -3);

        GuitarTuningConfiguration guitarTuning = new(StringConfiguration.Default);
        
        TabbingConfiguration configuration = new()
        {
            HarmonicaTuning = harmonicaTuningInA,
            GuitarTuning = guitarTuning
        };

        string inputFileContent = File.ReadAllText(inputFile);
        string outputFileContent = GuitarTabConverter.ConvertGuitarTab(configuration, inputFileContent);
        File.WriteAllText(outputFile, outputFileContent);
    }
}