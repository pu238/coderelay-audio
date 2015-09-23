using System.Collections.Generic;

class Instrument
{
    public static double[] GuitarNote(int samples, int frequency, int damping, int seed)
    {
        var cycle = new Queue<double>(Noise.GenerateWhiteNoise(frequency, seed));
        var result = new List<double>();

        for(int n = 0; n < samples / frequency; n++)
        {
            for (int d = 0; d < damping; d++)
            {
                for (int f = 0; f < frequency; f++)
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
