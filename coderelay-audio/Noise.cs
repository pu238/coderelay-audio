using System;

namespace coderelay_audio
{
    public class Noise
    {
        public static double[] GenerateWhiteNoise(int samples, int seed)
        {
            Random rand = new Random(seed);
            double[] noise = new double[samples];
            for (int i = 0; i < samples; ++i)
            {
                noise[i] = (rand.NextDouble() * 2.0 - 1.0) ; //range -1.0 - 1.0 
            }

            return noise;
        }

        public static double[] GenerateBrownNoise(int samples, int seed)
        {
            Random rand = new Random(seed);
            double[] noiseTable = new double[samples];
            double[] averageTable = new double[samples];
            for (int i = 0; i < samples; ++i)
            {
                noiseTable[i] = (rand.NextDouble() * 2.0f - 1.0f); //range -1.0 - 1.0 
            }

            for (int i = 3; i < samples; ++i)
            {
                averageTable[i] = (noiseTable[i - 3] + noiseTable[i - 2] + noiseTable[i - 1]) / 3;
            }

            return averageTable;
        }
    }
}