using System;

class MathUtils
{
    public static double Lerp(double a, double b, double f)
    {
        return a * f + b * (1.0 - f);
    }

    public static double MidiNoteToFrequency(int note)
    {
        return Math.Pow(2.0, (note - 69.0) / 12.0) * 440.0;
    }
}
