using System;
using System.IO;

class WavFile
{
    public static void WriteFile(string path, int sampleRate, double[] samplesL, double[] samplesR)
    {
        using (FileStream file = new FileStream(path, FileMode.Create))
        {
            WriteWav(file, sampleRate, samplesL, samplesR);
        }
    }

    public static void WriteWav(Stream stream, int sampleRate, double[] samplesL, double[] samplesR)
    {
        if (samplesL.Length != samplesR.Length)
            throw new ArgumentException("Length of samplesL not equal to length of samplesR");

        int numSamples = samplesL.Length;
        int dataSize = numSamples * 4 * 2;

        // Header
        // ckId
        byte[] riff = new byte[] { (byte)'R', (byte)'I', (byte)'F', (byte)'F' };
        stream.Write(riff, 0, 4);
        
        // WAVE Chunk
        // cksize // TODO: calc size, 4 + all other data
        stream.Write(LittleEndianInt32(36+(dataSize)), 0, 4);

        // waveid
        byte[] wave = new byte[] { (byte)'W', (byte)'A', (byte)'V', (byte)'E' };
        stream.Write(wave, 0, 4);

        // FORMAT Chunk
        //ckId
        byte[] fmt = new byte[] { (byte)'f', (byte)'m', (byte)'t', (byte)' ' };
        stream.Write(fmt, 0, 4);

        // cksize
        stream.Write(LittleEndianInt32(16), 0, 4);

        // format (0x0001 WAVE_FORMAT_PCM)
        stream.Write(LittleEndianInt16(1), 0, 2);

        // nChannels
        stream.Write(LittleEndianInt16(2), 0, 2);

        // nSamplesPerSec
        stream.Write(LittleEndianInt32(sampleRate), 0, 4);

        // nAvgBytesPerSec
        stream.Write(LittleEndianInt32(sampleRate*8), 0, 4);

        // nBlockAlign
        stream.Write(LittleEndianInt16(8), 0, 2);

        // wBitsPerSample
        stream.Write(LittleEndianInt16(32), 0, 2);

        // DATA Chunk
        // ckId
        byte[] data = new byte[] { (byte)'d', (byte)'a', (byte)'t', (byte)'a' };
        stream.Write(data, 0, 4);

        // ckSize
        stream.Write(LittleEndianInt32(dataSize), 0, 4);

        for (int i = 0; i < numSamples; ++i)
        {
            int l = (int)(samplesL[i] * Int32.MaxValue);
            stream.Write(LittleEndianInt32(l), 0, 4);

            int r = (int)(samplesR[i] * Int32.MaxValue);
            stream.Write(LittleEndianInt32(r), 0, 4);
        }
    }

    public static byte[] LittleEndianInt32(Int32 val)
    {
        byte[] bytes = BitConverter.GetBytes(val);

        if (BitConverter.IsLittleEndian == false)
            Array.Reverse(bytes);
            
        return bytes;
    }

    public static byte[] LittleEndianInt16(Int16 val)
    {
        byte[] bytes = BitConverter.GetBytes(val);

        if (BitConverter.IsLittleEndian == false)
            Array.Reverse(bytes);

        return bytes;
    }
}
