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
        double[] sineTable = new double[sineSamples];
        for (int i = 0; i < sineSamples; ++i)
        {
            sineTable[i] = Math.Sin(2.0 * Math.PI * (double)i / (double)sineSamples);
        }

        // Fill a buffer with sine wave
        double[] sineData = new double[SampleRate * 2];
        for (int i = 0; i < sineData.Length; ++i)
        {
            sineData[i] = sineTable[i % sineSamples];
        }

        // Generate a single square wave period
        const int squareSamples = 200;
        double[] squareTable = new double[squareSamples];
        for (int i = 0; i < squareSamples; ++i)
        {
            squareTable[i] = Math.Sin(2.0 * Math.PI * (double)i / (double)sineSamples) > 0 ? 1 : 0;
        }

        // Fill a buffer with square wave
        double[] squareData = new double[SampleRate];
        for (int i = 0; i < squareData.Length; ++i)
        {
            squareData[i] = squareTable[i % squareSamples];
        }

        double[] finalData = Mixer.Mix(sineData, squareData);

        // Generate file
        WavFile.WriteFile(arguments.Output, SampleRate, finalData, finalData);

        // Execute post gen argument
        if (string.IsNullOrEmpty(arguments.ExecAfter) == false)
        {
            Process p = Process.Start(arguments.ExecAfter, arguments.ExecAfterArgs);
            p.WaitForExit();
        }
    }
}