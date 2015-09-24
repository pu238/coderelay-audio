using System;
using System.Collections.Generic;
using System.Diagnostics;

class Program
{
    const int SampleRate = 44100;

    static void Main(string[] args)
    {
        Arguments arguments = new Arguments(args);
        
        const int tableSamples = 200;
        const double tableFrequency = 44100.0 / tableSamples;
        const int tones = 20;
        const int toneLength = 4000;
        const int trackLength = toneLength * tones*4;

        Random randomNotes = new Random(111);

        // Fill a buffer with square wave
        double[] squareData = new double[trackLength];
        WavetablePlayer squareChanel = new WavetablePlayer(Generate.Saw(tableSamples), squareData);

        int[,] chords = new int[,] {
            { 70, 62, 67, 62 },
            { 70, 64, 67, 64 },
            { 75, 67, 72, 67},
            { 72, 65, 69, 65}
        };

        for (int i = 0; i < tones; ++i)
        {
            int chord = i % chords.GetLength(0);

            for (int j = 0; j < 4; ++j)
            {
                squareChanel.NoteOn();
                squareChanel.SetNote(chords[chord, 0]);
                squareChanel.Render(toneLength);
                squareChanel.SetNote(chords[chord, 1]);
                squareChanel.Render(toneLength);
                squareChanel.SetNote(chords[chord, 2]);
                squareChanel.Render(toneLength);
                squareChanel.SetNote(chords[chord, 3]);
                squareChanel.Render(toneLength);
                squareChanel.NoteOff();
            }
        }

        var guitar = new List<double>();

        for (int i = 0; i < 30; i++)
        {
            var rand = new Random(i);
            guitar.AddRange(Instrument.String(SampleRate * rand.Next(1, 5), rand.Next(50, 700), rand.Next(1, 10), i));
        }

        double[] finalData = Mixer.Mix(squareData); //guitar.ToArray()

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