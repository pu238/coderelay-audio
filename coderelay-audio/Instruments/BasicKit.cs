using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instruments
{
    class BasicKit : Instrument
    {
        float bassDrumFundamental = 100.00f;
        float bassDrumFundamentalDecay = 0.992f;

        float snareDrumDecay = 0.9990f;

        public BasicKit(float tempo, int sampleRate) : base(tempo, sampleRate)
        {
        }


        public override MixerInput GenerateAudio(int length)
        {
            double[] sampleData = new double[sampleRate * length];

            // Basic beat, alternate bass drum and snare
            bool on = true;
            for(int i = 0; i < sampleData.Length - sampleRate; i += crotchetSamples)
            {
                if(on)
                {
                    BassDrum().CopyTo(sampleData, i);
                }
                else
                {
                    SnareDrum().CopyTo(sampleData, i);
                }
                on = !on;
            }
            
            // TODO: Some sort of sequencer, riff based?

            return sampleData;
        }

        // Bass drum is a low frequency with a decay
        private double[] BassDrum()
        {
            // Bass drum is a 1 second sample
            double[] sampleData = new double[sampleRate * 1];

            double bassDrumFundamentalAmplitude = 0.9 * short.MaxValue;

            for (int n = 0; n < sampleData.Length; n++)
            {
                sampleData[n] = (short)(bassDrumFundamentalAmplitude * Math.Sin((2 * Math.PI * n * bassDrumFundamental) / sampleRate));
                bassDrumFundamentalAmplitude *= bassDrumFundamentalDecay;
            }

            return sampleData;
        }

        // Snare drum is white noise with a decay
        private double[] SnareDrum()
        {
            double[] sampleData = new double[sampleRate * 1];

            double snareAmplitude = 1;
            Random rand = new Random();

            for (int n = 0; n < sampleData.Length; n++)
            {
                sampleData[n] = rand.NextDouble() * snareAmplitude;
                snareAmplitude *= snareDrumDecay;
            }

            return sampleData;
        }
        
    }
}
