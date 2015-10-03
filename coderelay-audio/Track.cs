// TODO: split open start, single note, and close into separate classes
class TrackNode
{
    readonly int note;
    readonly bool open;
    readonly bool close;

    public TrackNode(int note, bool open = true, bool close = false)
    {
        this.note = note;
        this.open = open;
        this.close = close;
    }

    public void Begin(WavetablePlayer instrument)
    {
        if (open == false) return;
        instrument.SetNote(note-12);
        instrument.NoteOn();
    }

    public void End(WavetablePlayer instrument)
    {
        if (close == false) return;
        instrument.NoteOff();
    }
}

class Track
{
    public static void Play(TrackNode[] track, WavetablePlayer instrument, double nodesPerSecond)
    {
        int samplesPerNode = (int)(44100.0 / nodesPerSecond);

        for (int i = 0; i < track.Length; ++i)
        {
            TrackNode node = track[i];

            if (node == null)
            {
                instrument.Render(samplesPerNode);
            }
            else
            {
                node.Begin(instrument);
                instrument.Render(samplesPerNode);
                node.End(instrument);
            }
        }
    }
}
