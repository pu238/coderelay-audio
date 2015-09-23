using System;
using System.Collections.Generic;
using System.Linq;

class Mixer
{
    public static double[] Mix(params Func<IList<double>>[] sampleGenerators)
    {
        return Mix(sampleGenerators.Select(f => f()).ToArray());
    }

    public static double[] Mix(params IList<double>[] sampleBuffers)
    {
        var outputLength = sampleBuffers.Max(a => a.Count);
        var output = new double[outputLength];

        foreach (var samples in sampleBuffers)
        {
            for (var i = 0; i < samples.Count; i++)
            {
                output[i] += samples[i];
            }
        }

        var peak = output.Select(Math.Abs).Max();

        for (var i = 0; i < outputLength; i++)
        {
            output[i] /= peak;
        }

        return output;
    }
}
