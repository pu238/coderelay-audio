using System;

class Arguments
{
    public string Output { get; private set; }
    public string ExecAfter { get; private set; }
    public string ExecAfterArgs { get; private set; }

    public Arguments(string [] args)
    {
        Output = "audio.wav";
        ExecAfter = null;
        ExecAfterArgs = Output;

        for (int i = 0; i < args.Length; ++i)
        {
            string arg = args[i];

            try
            {
                if (arg == "-output")
                {
                    Output = args[++i];
                }
                else if (arg == "-exec")
                {
                    ExecAfter = args[++i];
                }
                else if (arg == "-execargs")
                {
                    ExecAfterArgs = args[++i];
                }
                else
                {
                    throw new ArgumentOutOfRangeException(arg, "Unrecognised command line argument");
                }
            }
            catch (IndexOutOfRangeException)
            {
                throw new ArgumentException("Invalid command line arguments, parameter is missing", arg);
            }
        }
    }
}
