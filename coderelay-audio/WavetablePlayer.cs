class WavetablePlayer
{
    double[] wavetable;
    double[] destbuffer;
    double referenceFrequency;

    long destOffset = 0;

    double currentSpeed = 1.0;
    double srcOffset = 0.0;

    double glideFromSpeed = 1.0;
    double glideToSpeed = 1.0;
    long glideDuration = 0;
    long glideElapsed = 0;

    // Wavetable should be a short looping sample that crosses 0 at the start and end, destbuffer is the render destination
    public WavetablePlayer(double [] wavetable, double [] destbuffer, double referenceFrequency)
    {
        this.wavetable = wavetable;
        this.destbuffer = destbuffer;
        this.referenceFrequency = referenceFrequency;
        this.Envelope = new ADSRWavetableEnvelope(0.0, 0.0, 1.0, 0.0);
    }

    // Assume that wavetable contains a single period, override it if you need it to contain something more
    public WavetablePlayer(double[] wavetable, double[] destbuffer) :
        this(wavetable, destbuffer, 44100.0 / wavetable.Length)
    {

    }

    public IWaveTableEnvelope Envelope
    {
        get; set;
    }

    public void NoteOn()
    {
        Envelope.NoteOn();
    }

    public void NoteOff()
    {
        Envelope.NoteOff();
    }

    // TODO: implement table reference pitch and pitch offset calc and replace this with SetTone?
    public void SetSpeed(double speed)
    {
        this.currentSpeed = speed;

        StopGlide();
    }

    public void SetNote(int note)
    {
        currentSpeed = GetSpeedForNote(note);

        StopGlide();
    }

    double GetSpeedForNote(int note)
    {
        double noteFrequency = MathUtils.MidiNoteToFrequency(note);
        return noteFrequency / referenceFrequency;
    }

    void StopGlide()
    {
        glideFromSpeed = currentSpeed;
        glideToSpeed = currentSpeed;
        glideDuration = 0;
        glideElapsed = 0;
    }

    public void GlideToNote(int note, long duration)
    {
        glideFromSpeed = currentSpeed;
        glideToSpeed = GetSpeedForNote(note);
        glideDuration = duration;
        glideElapsed = 0;
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
            if (srcOffset > 0.0 || Envelope.IsPlaying())
            {

                if (glideDuration > 0)
                {
                    glideElapsed++;
                    currentSpeed = MathUtils.Lerp(glideFromSpeed, glideToSpeed, (double)glideElapsed / (double)glideDuration);
                    {
                        if (glideElapsed >= glideDuration)
                            StopGlide();
                    }
                }

                destbuffer[destOffset] = TableUtils.Sample(wavetable, srcOffset) * Envelope.Sample();
                srcOffset = srcOffset + currentSpeed;

                // Loop if note still on
                if (Envelope.IsPlaying())
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
