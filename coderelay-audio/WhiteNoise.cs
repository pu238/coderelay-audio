using System;

namespace coderelay_audio
{
    public class WhiteNoise
    {
        public double[] Generate(int samples, int tableSamples, int tones, int sampleRate)
        {
            Random rand = new Random();
            double[] noiseTable = new double[samples];
            double[] noiseData = new double[tableSamples * tones * 128];
            WavetablePlayer noiseChannel = new WavetablePlayer(noiseTable, noiseData);
            for (int i = 0; i < samples; ++i)
            {
                noiseTable[i] = (rand.NextDouble() * 2.0 - 1.0) ; //range -1.0 - 1.0 
            }
    
            for (int i = 0; i < tones; i++)
            {
                noiseChannel.On();
                noiseChannel.Render(sampleRate);
            }

            return noiseData;
        }


    }
}