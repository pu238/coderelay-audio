using System;

class TableUtils
{
    // Interpolate between nearest two samples to sample position
    public static double Sample(double[] table, double offset)
    {
        long lowerIndex = (long)offset;
        long upperIndex = lowerIndex + 1;
        double lerp = offset - lowerIndex;
        
        if (upperIndex >= table.Length)
            return table[table.Length - 1];

        return MathUtils.Lerp(table[lowerIndex], table[upperIndex], lerp);
    }

    public static double[] Add(double[] tableA, double[] tableB)
    {
        if (tableA.Length != tableB.Length)
            throw new ArgumentException($"{nameof(tableA)} must have the same length as {nameof(tableB)}");

        var output = new double[tableA.Length];

        for (var i = 0; i < output.Length; i++)
        {
            output[i] = tableA[i] + tableB[i];
        }

        return output;
    }

    public static double[] Subtract(double[] tableA, double[] tableB)
    {
        if (tableA.Length != tableB.Length)
            throw new ArgumentException($"{nameof(tableA)} must have the same length as {nameof(tableB)}");

        var output = new double[tableA.Length];

        for (var i = 0; i < output.Length; i++)
        {
            output[i] = tableA[i] - tableB[i];
        }

        return output;
    }

    public static double[] Multiply(double[] tableA, double[] tableB)
    {
        if (tableA.Length != tableB.Length)
            throw new ArgumentException($"{nameof(tableA)} must have the same length as {nameof(tableB)}");

        var output = new double[tableA.Length];

        for (var i = 0; i < output.Length; i++)
        {
            output[i] = tableA[i] * tableB[i];
        }

        return output;
    }

    public static double[] Multiply(double[] table, double value)
    {
        var output = new double[table.Length];

        for (var i = 0; i < output.Length; i++)
        {
            output[i] = table[i] * value;
        }

        return output;
    }

    public static double[] Corrupt(double[] table, double amount, int seed = 1337)
    {
        var output = new double[table.Length];
        var rand = new Random(seed);

        for (var i = 0; i < output.Length; i++)
        {
            // the formula is rand * (max - min) + min
            // max = amount, min = -amount
            // below is the same as rand * (amount - -amount) + -amount
            output[i] = table[i] + (rand.NextDouble() * (amount * 2) - amount);
        }

        return output;
    }
}