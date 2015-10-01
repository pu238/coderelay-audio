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

    public static implicit operator MixerInput(List<double> samples)
    {
        return new MixerInput(TimeSpan.Zero, samples);
    }

    public MixerInput OffsetBy(TimeSpan offset)
    {
        return new MixerInput(Offset + offset, Samples);
    }
}

class Mixer
{
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
        
        var peak = 1.0;

        for (var i = 0; i < outputLength; i++)
        {
            var end = Math.Min(i + 128, outputLength);
            var nextPeak = 1.0;

            for (var j = i; j < end; j++)
            {
                var abs = Math.Abs(output[j]);
                if (abs > nextPeak)
                    nextPeak = abs;
            }

            if (nextPeak > peak)
                peak += (nextPeak - peak) * 0.25;

            if (nextPeak < peak)
                peak = Math.Max(peak * 0.995, 1);

            output[i] = MathUtils.Clamp(output[i] / peak, -1, 1);
        }

        return output;
    }
}
