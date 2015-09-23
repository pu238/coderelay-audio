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
        const int tones = 20;
        const int toneLength = 8000;
        const int trackLength = toneLength * tones * 4;
        // Generate a single sine wave period
        double[] sineTable = new double[tableSamples];
        for (int i = 0; i < tableSamples; ++i)
        {
            sineTable[i] = Math.Sin(2.0 * Math.PI * (double)i / (double)tableSamples);
        }

        Random randomNotes = new Random(111);

        double[] sineData = new double[trackLength];
        WavetablePlayer sineChannel = new WavetablePlayer(sineTable, sineData);
        for (int i = 0; i < tones; ++i)
        {
            sineChannel.SetSpeed(randomNotes.NextDouble() * 2.0 + 0.75);
            sineChannel.NoteOn();
            sineChannel.Render(toneLength);
            sineChannel.NoteOff();
            sineChannel.Render(toneLength * 3);
        }

        // Generate a single square wave period
        double[] squareTable = new double[tableSamples];
        for (int i = 0; i < tableSamples; ++i)
        {
            squareTable[i] = Math.Sin(2.0 * Math.PI * (double)i / (double)tableSamples) > 0 ? 1 : -1;
        }

        // Fill a buffer with square wave
        double[] squareData = new double[trackLength];
        WavetablePlayer squareChanel = new WavetablePlayer(squareTable, squareData, 220.5);

        int[,] chords = new int[,] {
            { 7, 10, 14 },
            { 9, 12, 15 },
            { 9, 5, 2 },
            { 10, 7, 3 },
            { 5, 9, 12 }
        };

        for (int i = 0; i < tones; ++i)
        {
            int chord = i % 5;
            int octaveOffset = 5 * 12;

            for (int j = 0; j < 4; ++j)
            {
                squareChanel.NoteOn();
                squareChanel.SetNote(chords[chord, 0] + octaveOffset);
                squareChanel.Render(toneLength / 3);
                squareChanel.SetNote(chords[chord, 1] + octaveOffset);
                squareChanel.Render(toneLength / 3);
                squareChanel.SetNote(chords[chord, 2] + octaveOffset);
                squareChanel.Render(toneLength / 3);
                squareChanel.NoteOff();
            }
        }

        var guitar = new List<double>();

        for (int i = 0; i < 30; i++)
        {
            var rand = new Random(i);
            guitar.AddRange(Instrument.String(SampleRate * rand.Next(1, 5), rand.Next(50, 700), rand.Next(1, 10), i));
        }

        double[] finalData = Mixer.Mix(squareData, guitar.ToArray());

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