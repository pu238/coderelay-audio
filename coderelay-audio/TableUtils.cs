
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
}