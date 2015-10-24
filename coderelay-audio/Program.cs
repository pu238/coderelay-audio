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
            //CombinedNotes().OffsetBy(TimeSpan.FromSeconds(0)),
            //RandomNotes().OffsetBy(TimeSpan.FromSeconds(1)),
            SongUsingTrack().OffsetBy(TimeSpan.FromSeconds(0)),
            Aerodynamic().OffsetBy(TimeSpan.FromSeconds(14)),
            RandomGuitar().OffsetBy(TimeSpan.FromSeconds(21.5)));

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

    static MixerInput SongUsingTrack()
    {
        double[] data = new double[14*44100];
        WavetablePlayer sawInstrument = new WavetablePlayer(TableUtils.Corrupt(TableUtils.Multiply(Generate.Saw(200), 0.5), 0.5), data);
        sawInstrument.Envelope = new ADSRWavetableEnvelope(0.25, 0.25, 0.8, 0.5);
        // These notes are taken from the melody of some random tracker file I found floating around my hdd, ub-lgnd.xm
        // I have no idea who the original author is :(
        TrackNode[] trackData = new TrackNode[64];
        trackData[0] = new TrackNode(Midi.Note(Note.B, 6));               //B6
        trackData[2] = new TrackNode(Midi.Note(Note.A, 6));               //A6
        trackData[4] = new TrackNode(Midi.Note(Note.B, 6));               //B6
        trackData[6] = new TrackNode(Midi.Note(Note.A, 6));               //A6
        trackData[8] = new TrackNode(Midi.Note(Note.G, 6));               //G6
        trackData[10] = new TrackNode(0, false, true);  //-- 
        trackData[12] = new TrackNode(Midi.Note(Note.A, 6));              //A6
        trackData[14] = new TrackNode(0, false, true);  //--
        trackData[16] = new TrackNode(Midi.Note(Note.FSharp, 6));              //F#6
        trackData[18] = new TrackNode(Midi.Note(Note.E, 6));              //E6
        trackData[20] = new TrackNode(Midi.Note(Note.FSharp, 6));              //F#6
        trackData[22] = new TrackNode(Midi.Note(Note.B, 5));              //B5
        trackData[24] = new TrackNode(Midi.Note(Note.FSharp, 6));              //F#6
        trackData[26] = new TrackNode(Midi.Note(Note.G, 6));              //G6
        trackData[28] = new TrackNode(0, false, true);  //--
        trackData[32] = new TrackNode(Midi.Note(Note.E, 6));              //E6
        trackData[34] = new TrackNode(Midi.Note(Note.B, 5));              //B5
        trackData[36] = new TrackNode(Midi.Note(Note.FSharp, 6));              //F#6
        trackData[38] = new TrackNode(Midi.Note(Note.G, 6));              //G6
        trackData[40] = new TrackNode(Midi.Note(Note.E, 6));              //E6
        trackData[41] = new TrackNode(Midi.Note(Note.FSharp, 6));              //F#6
        trackData[46] = new TrackNode(Midi.Note(Note.D, 6));              //D6
        trackData[47] = new TrackNode(Midi.Note(Note.D, 6));              //D6
        trackData[48] = new TrackNode(Midi.Note(Note.E, 6));              //E6
        trackData[52] = new TrackNode(0, false, true);  //--
        trackData[56] = new TrackNode(Midi.Note(Note.G, 6));              //G6
        trackData[58] = new TrackNode(0, false, true);  //--
        trackData[60] = new TrackNode(Midi.Note(Note.C, 7));              //C7
        trackData[62] = new TrackNode(0, false, true);  // --

        for (int i = 0; i < 2; ++i)
            Track.Play(trackData, sawInstrument, 10.0);

        // Render some leadout time
        sawInstrument.Render(44100);

        return data;
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
