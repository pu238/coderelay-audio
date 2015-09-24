using System;

class Generate
{
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