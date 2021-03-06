﻿using System;

class MathUtils
{
    public static double Lerp(double a, double b, double f)
    {
        return a * (1.0 - f) + b * f;
    }

    public static double Clamp(double value, double min, double max)
    {
        if (value < min) return min;
        if (value > max) return max;
        return value;
    }

    public static double MidiNoteToFrequency(int note)
    {
        return Math.Pow(2.0, (note - 69.0) / 12.0) * 440.0;
    }
}
