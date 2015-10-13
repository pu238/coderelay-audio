using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum Note
{
    C, CSharp, D, DSharp, E, F, FSharp, G, GSharp, A, ASharp, B
}

public static class Midi
{
    public static int Note(Note note, int octave) => (int)note + (12 * octave);
}
