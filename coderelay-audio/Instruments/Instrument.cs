using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instruments
{
    class Instrument
    {
        protected float tempo;
        protected int sampleRate;

        // Calculated note lengths in seconds
        protected float semiBreveDuration;
        protected float minimDuration;
        protected float crotchetDuration;
        protected float quaverDuration;
        protected float semiQuaverDuration;

        // Calculated note lengths in samples
        protected int semiBreveSamples;
        protected int minimSamples;
        protected int crotchetSamples;
        protected int quaverSamples;
        protected int semiQuaverSamples;

        public Instrument(float tempo, int sampleRate)
        {
            this.tempo = tempo;
            this.sampleRate = sampleRate;

            // Calculate the note durations based on the tempo
            float secondsPerBeat = 60.0f / tempo;
            semiBreveDuration = secondsPerBeat * 4.00f;
            minimDuration = secondsPerBeat * 2.00f;
            crotchetDuration = secondsPerBeat * 1.00f;
            quaverDuration = secondsPerBeat * 0.50f;
            semiQuaverDuration = secondsPerBeat * 0.25f;

            // Calcualte the note durations in samples based on the tempo
            float samplesPerBeat = sampleRate * 60.0f / tempo;
            semiBreveSamples = (int)(samplesPerBeat * 4.00f);
            minimSamples = (int)(samplesPerBeat * 2.00f);
            crotchetSamples = (int)(samplesPerBeat * 1.00f);
            quaverSamples = (int)(samplesPerBeat * 0.50f);
            semiQuaverSamples = (int)(samplesPerBeat * 0.25f);
        }


        public virtual MixerInput GenerateAudio(int length)
        {
            throw new NotImplementedException(); 
        }
    }
}
