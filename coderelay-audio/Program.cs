using System;
using System.Diagnostics;

class Program
{
    const int SampleRate = 44100;

    static void Main(string[] args)
    {
        Arguments arguments = new Arguments(args);

        const int tableSamples = 200;
        // Generate a single sine wave period
        double[] sineTable = new double[tableSamples];
        for (int i = 0; i < tableSamples; ++i)
        {
            sineTable[i] = Math.Sin(2.0 * Math.PI * (double)i / (double)tableSamples);
        }

        int tones = 10;
        double[] sineData = new double[tableSamples * tones * 128];
        WavetablePlayer sineChannel = new WavetablePlayer(sineTable, sineData);
        for (int i = 0; i < tones; ++i)
        {
            sineChannel.On();
            sineChannel.Render(tableSamples * 32);
            sineChannel.Off();
            sineChannel.Render(tableSamples * 96);
        }

        // Generate a single square wave period
        double[] squareTable = new double[tableSamples];
        for (int i = 0; i < tableSamples; ++i)
        {
            squareTable[i] = Math.Sin(2.0 * Math.PI * (double)i / (double)tableSamples) > 0 ? 1 : -1;
        }

        // Fill a buffer with square wave
        double[] squareData = new double[tableSamples * tones * 128];
        WavetablePlayer squareChanel = new WavetablePlayer(squareTable, squareData);
        for (int i = 0; i < tones; ++i)
        {
            squareChanel.Render(tableSamples * 64);
            squareChanel.On();
            squareChanel.Render(tableSamples * 32);
            squareChanel.Off();
            squareChanel.Render(tableSamples * 32);
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