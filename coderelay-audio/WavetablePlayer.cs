class WavetablePlayer
{
    double[] wavetable;
    double[] destbuffer;
    long offset = 0;
    bool on;

    public WavetablePlayer(double [] wavetable, double [] destbuffer)
    {
        this.wavetable = wavetable;
        this.destbuffer = destbuffer;
    }

    public void On()
    {
        on = true;
    }

    public void Off()
    {
        on = false;
    }

    public void Render(int samples)
    {
        long newOffset = offset + samples;

        if (newOffset > destbuffer.Length)
            newOffset = destbuffer.Length;

        for (; offset < newOffset; ++offset)
        {
            // This is gonna clip like crazy but I don't have enough lines this commit to make it nice
            if (on)
                destbuffer[offset] = wavetable[offset % wavetable.Length];
            else
                destbuffer[offset] = 0.0;
        }
    }
}
