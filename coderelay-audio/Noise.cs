using System;

namespace coderelay_audio
{
    public class Noise
    {
        public double[] GenerateWhiteNoise(int samples, int tableSamples, int tones, int sampleRate)
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

        public double[] GenerateBrownNoise(int samples, int tableSamples, int tones, int sampleRate)
        {
            Random rand = new Random();
            double[] noiseTable = new double[samples];
            double[] noiseData = new double[tableSamples * tones * 128];
            double[] averageTable = new double[samples];
            WavetablePlayer noiseChannel = new WavetablePlayer(averageTable, noiseData);
            for (int i = 0; i < samples; ++i)
            {
                noiseTable[i] = (rand.NextDouble() * 2.0f - 1.0f); //range -1.0 - 1.0 
            }

            for (int i = 3; i < samples; ++i)
            {
                averageTable[i] = (noiseTable[i - 3] + noiseTable[i - 2] + noiseTable[i - 1]) / 3;
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