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
        WavetablePlayer squareChanel = new WavetablePlayer(squareTable, squareData);
        for (int i = 0; i < tones; ++i)
        {
            squareChanel.SetSpeed(randomNotes.NextDouble() * 2.0 + 0.75);
            squareChanel.Render(toneLength * 2);
            squareChanel.NoteOn();
            squareChanel.Render(toneLength);
            squareChanel.NoteOff();
            squareChanel.Render(toneLength);
        }

        var guitar = new List<double>();

        guitar.AddRange(Instrument.GuitarNote(SampleRate * 1, 100, 1, 32424));
        guitar.AddRange(Instrument.GuitarNote(SampleRate * 1, 200, 1, 32424));
        guitar.AddRange(Instrument.GuitarNote(SampleRate * 2, 100, 1, 32424));
        guitar.AddRange(Instrument.GuitarNote(SampleRate * 1, 400, 1, 32424));
        guitar.AddRange(Instrument.GuitarNote(SampleRate * 4, 100, 1, 32424));
        guitar.AddRange(Instrument.GuitarNote(SampleRate * 1, 900, 5, 32424));
        guitar.AddRange(Instrument.GuitarNote(SampleRate * 2, 100, 1, 32424));
        guitar.AddRange(Instrument.GuitarNote(SampleRate * 1, 300, 1, 32424));

        double[] finalData = Mixer.Mix(sineData, squareData, guitar.ToArray());

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