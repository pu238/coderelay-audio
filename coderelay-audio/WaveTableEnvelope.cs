interface IWaveTableEnvelope
{
    void NoteOn();
    void NoteOff();
    double Sample();
    bool IsPlaying();
}

public class ADSRWavetableEnvelope : IWaveTableEnvelope
{
    readonly double attackRate;
    readonly double decayRate;
    readonly double sustainLevel;
    readonly double releaseRate;

    bool on;
    bool inAttack;
    double level;

    public ADSRWavetableEnvelope(double attack, double decay, double sustainLevel, double release)
    {
        attackRate = 1.0 / (attack * 44100.0);
        decayRate = 1.0 / (decay * 44100.0);
        this.sustainLevel = sustainLevel;
        releaseRate = 1.0 / (release * 44100.0);
    }

    public void NoteOn()
    {
        on = true;
        inAttack = true;
    }

    public void NoteOff()
    {
        on = false;
    }

    public double Sample()
    {
        if (inAttack)
        {
            level += attackRate;

            if (level >= 1.0)
            {
                level = 1.0;
                inAttack = false;
            }
        }
        else if (on)
        {
            level -= decayRate;

            if (level < sustainLevel)
            {
                level = sustainLevel;
            }
        }
        else
        {
            level -= releaseRate;

            if (level < 0)
            {
                level = 0.0;
            }
        }

        return level;
    }

    public bool IsPlaying()
    {
        return on || level > 0.0;
    }
}