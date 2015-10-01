using System;
using System.Collections.Generic;
using System.Diagnostics;

class Program
{
    const int SampleRate = 44100;

    static void Main(string[] args)
    {
        Arguments arguments = new Arguments(args);

        // Mix input samples
        double[] finalData = Mixer.Mix(SampleRate,
            CombinedNotes().OffsetBy(TimeSpan.FromSeconds(0)),
            RandomNotes().OffsetBy(TimeSpan.FromSeconds(1)),
            Aerodynamic().OffsetBy(TimeSpan.FromSeconds(5)),
            RandomGuitar().OffsetBy(TimeSpan.FromSeconds(12.5)));

        // Generate file
        WavFile.WriteFile(arguments.Output, SampleRate, finalData, finalData);

        // Execute post gen argument
        if (string.IsNullOrEmpty(arguments.ExecAfter) == false)
        {
            Process p = Process.Start(arguments.ExecAfter, arguments.ExecAfterArgs);
            p.WaitForExit();
        }
    }

    static MixerInput CombinedNotes()
    {
        var tones = 10;
        var toneLength = 4000;
        var trackLength = tones * toneLength;
        var data = new double[trackLength];
        var table = TableUtils.Multiply(TableUtils.Add(Generate.Saw(200), TableUtils.Multiply(Generate.Square(200), -1)), 0.5);
        var player = new WavetablePlayer(table, data);
        //var player = new WavetablePlayer(Generate.Saw(200), data);

        player.NoteOn();
        for (var i = 0; i < tones; i++)
        {
            player.SetNote(60);
            player.Render(toneLength);
        }
        player.NoteOff();

        return data;
    }

    static MixerInput RandomNotes()
    {
        Random randomNotes = new Random(111);

        var spansTones = 40;
        var spansToneLength = 4000;
        var spansTrackLength = spansTones * spansToneLength;
        var spansRandomData = new double[spansTrackLength];
        var spansPlayer = new WavetablePlayer(Generate.Sine(200), spansRandomData);

        spansPlayer.NoteOn();
        for (int i = 0; i < spansTones; i++)
        {
            spansPlayer.GlideToNote(randomNotes.Next(60, 71), spansToneLength);
            spansPlayer.Render(spansToneLength);
        }
        spansPlayer.NoteOff();

        return spansRandomData;
    }

    static MixerInput Aerodynamic()
    {
        const int tableSamples = 200;
        const int tones = 20;
        const int toneLength = 4000;
        const int trackLength = toneLength * tones * 4;

        double[] sawData = new double[trackLength];
        WavetablePlayer sawChannel = new WavetablePlayer(Generate.Saw(tableSamples), sawData);

        int[,] chords = {
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
                sawChannel.NoteOn();
                sawChannel.SetNote(chords[chord, 0]);
                sawChannel.Render(toneLength);
                sawChannel.SetNote(chords[chord, 1]);
                sawChannel.Render(toneLength);
                sawChannel.SetNote(chords[chord, 2]);
                sawChannel.Render(toneLength);
                sawChannel.SetNote(chords[chord, 3]);
                sawChannel.Render(toneLength);
                sawChannel.NoteOff();
            }
        }

        return sawData;
    }

    static MixerInput RandomGuitar()
    {
        var guitar = new List<double>();
        var rand = new Random(23);

        for (int i = 0; i < 30; i++)
        {
            var length = (1 + rand.NextDouble()) * SampleRate;
            var frequency = MathUtils.MidiNoteToFrequency(rand.Next(40, 70));
            guitar.AddRange(Instrument.String((int)length, (int)frequency, rand.Next(1, 10), i));
        }

        return guitar;
    }
}
