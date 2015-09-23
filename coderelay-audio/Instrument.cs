using System.Collections.Generic;

class Instrument
{
    public static double[] String(int samples, int frequency, int damping, int seed)
    {
        var cycle = new Queue<double>(Noise.GenerateWhiteNoise(samples / frequency, seed));
        var result = new List<double>();

        for (int f = 0; f < frequency; f++)
        {
            for (int d = 0; d < damping; d++)
            {
                for (int s = 0; s < samples / frequency; s++)
                {
                    var current = ((cycle.Dequeue() + cycle.Peek()) / 2) * 0.995;
                    cycle.Enqueue(current);
                }
            }

            result.AddRange(cycle);
        }

        return result.ToArray();
    }
}
