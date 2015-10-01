using System;
using System.Linq;

class Generate
{
    public static double[] Combine(double[] wave1, double[] wave2)
    {
        if (wave1.Length != wave2.Length)
            throw new ArgumentException("Wave array lengths aren't equal");

        return wave1.Select((sample, index) => (sample + wave2[index]) / 2d).ToArray();
    }

    public static double[] Sine(long length)
    {
        // Generate a single sine wave period
        double[] table = new double[length];
        for (long i = 0; i < length; ++i)
        {
            table[i] = Math.Sin(2.0 * Math.PI * (double)i / (double)length);
        }

        return table;
    }

    public static double[] Square(long length)
    {
        // Generate a single square wave period
        double[] table = new double[length];
        for (long i = 0; i < length; ++i)
        {
            table[i] = Math.Sin(2.0 * Math.PI * (double)i / (double)length) > 0 ? 1 : -1;
        }

        return table;
    }

    public static double[] Saw(long length)
    {
        double[] table = new double[length];

        for (long i = 0; i < length; ++i)
        {
            table[i] = 2.0 * (double)i / (double)length - 1.0;
        }

        return table;
    }
}