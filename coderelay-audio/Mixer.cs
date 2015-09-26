using System;
using System.Collections.Generic;
using System.Linq;

struct MixerInput
{
    public readonly TimeSpan Offset;
    public readonly IList<double> Samples;

    public MixerInput(TimeSpan offset, IList<double> samples)
    {
        Offset = offset;
        Samples = samples;
    }

    public static implicit operator MixerInput(double[] samples)
    {
        return new MixerInput(TimeSpan.Zero, samples);
    }
}

class Mixer
{
    public static double[] Mix(int sampleRate, params Func<MixerInput>[] inputGenerators)
    {
        return Mix(sampleRate, inputGenerators.Select(f => f()).ToArray());
    }

    public static double[] Mix(int sampleRate, params MixerInput[] inputs)
    {
        var outputLength = inputs.Max(i => (int)(i.Offset.TotalSeconds * sampleRate) + i.Samples.Count);
        var output = new double[outputLength];

        foreach (var input in inputs)
        {
            var offset = (int)(input.Offset.TotalSeconds * sampleRate);

            for (var i = 0; i < input.Samples.Count; i++, offset++)
            {
                output[offset] += input.Samples[i];
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
