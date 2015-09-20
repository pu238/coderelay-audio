using System;
using System.Diagnostics;

class Program
{
    const int SampleRate = 44100;

    static void Main(string[] args)
    {
        Arguments arguments = new Arguments(args);

        // Generate a single sine wave period
        const int sineSamples = 200;
        double [] sineTable = new double[sineSamples];
        for (int i = 0; i < sineSamples; ++i)
        {
            sineTable[i] = Math.Sin(2.0 * Math.PI * (double)i / (double)sineSamples);
        }

        // Fill a buffer with sine wave
        double[] finaldata = new double[SampleRate * 2];
        for (int i = 0; i < finaldata.Length; ++i)
        {
            finaldata[i] = sineTable[i % sineSamples];
        }

        // Generate file
        WavFile.WriteFile(arguments.Output, SampleRate, finaldata, finaldata);

        // Execute post gen argument
        if (string.IsNullOrEmpty(arguments.ExecAfter) == false)
        {
            Process p = Process.Start(arguments.ExecAfter, arguments.ExecAfterArgs);
            p.WaitForExit();
        }
    }
}