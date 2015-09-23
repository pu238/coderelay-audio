class WavetablePlayer
{
    double[] wavetable;
    double[] destbuffer;
    long destOffset = 0;
    bool on;

    double speed = 1.0;
    double srcOffset = 0.0;
    double referenceFrequency;

    // Wavetable should be a short looping sample that crosses 0 at the start and end, destbuffer is the render destination
    public WavetablePlayer(double [] wavetable, double [] destbuffer, double referenceFrequency = 440.0)
    {
        this.wavetable = wavetable;
        this.destbuffer = destbuffer;
        this.referenceFrequency = referenceFrequency;
    }

    public void NoteOn()
    {
        on = true;
    }

    public void NoteOff()
    {
        on = false;
    }

    // TODO: implement table reference pitch and pitch offset calc and replace this with SetTone?
    public void SetSpeed(double speed)
    {
        this.speed = speed;
    }

    public void SetNote(int note)
    {
        double noteFrequency = MathUtils.MidiNoteToFrequency(note);

        speed = noteFrequency / referenceFrequency;
    }

    public void Render(int samples)
    {
        long newOffset = destOffset + samples;

        if (newOffset > destbuffer.Length)
            newOffset = destbuffer.Length;

        for (; destOffset < newOffset; ++destOffset)
        {
            // Only start playing the sample if 'on' is set, otherwise if we're mid sample,
            //  keep playing it to the end so we don't clip
            if (srcOffset > 0.0 || on)
            {
                destbuffer[destOffset] = TableUtils.Sample(wavetable, srcOffset);
                srcOffset = srcOffset + speed;

                // Loop if note still on
                if (on)
                {
                    srcOffset = srcOffset % wavetable.Length;
                }
                else
                {
                    if (srcOffset > wavetable.Length)
                        srcOffset = 0.0;
                }
            }
            else
            {
                destbuffer[destOffset] = 0.0;
            }
        }
    }
}
