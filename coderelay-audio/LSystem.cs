using System.Collections.Generic;

class Token
{
    public readonly string id;
    public readonly string[] parameters;

    public Token(string id, string[] parameters)
    {
        this.id = id;
        this.parameters = parameters;
    }
}

class LSystemRules
{
    public List<Token> Apply(List<Token> input)
    {
        List<Token> output = new List<Token>();

        for (int i = 0; i < input.Count; ++i)
        {
            Token t = input[i];
            output.Add(new Token(t.id, t.parameters));
            output.Add(new Token(t.id, t.parameters));
        }

        return output;
    }
}

class LSystemExecution
{
    public List<TrackNode> TrackData = new List<TrackNode>();

    public void Execute(List<Token> tokens)
    {
        for (int i = 0; i < tokens.Count; ++i)
        {
            TrackData.Add(new TrackNode(Midi.Note((Note)System.Enum.Parse(typeof(Note),tokens[i].parameters[0]), 6)));
            TrackData.Add(null);
            TrackData.Add(new TrackNode(0, false, true));
            TrackData.Add(null);
        }
    }
}

class LSystem
{
    readonly LSystemRules rules;
    readonly LSystemExecution execution;

    const char ParameterListOpenBrace = '(';
    const char ParameterListCloseBrace = ')';
    const char ParameterListSeparator = ',';

    public LSystem(LSystemRules rules, LSystemExecution execution)
    {
        this.rules = rules;
        this.execution = execution;
    }

    public List<Token> Parse(string input)
    {
        List<Token> tokens = new List<Token>();
        
        for (int i = 0; i < input.Length; )
        {
            string token_id = input[i].ToString();
            string[] token_parameters = null;

            i++;
            if (i < input.Length && input[i] == ParameterListOpenBrace) // If open brace, find matching brace then split parameter list
            {
                int parameterstart = i + 1;
                int parameterend = input.IndexOf(ParameterListCloseBrace, parameterstart); 
                string rawparams = input.Substring(parameterstart, parameterend - parameterstart);
                token_parameters = rawparams.Split(new char[] {ParameterListSeparator});
                i = parameterend + 1;
            }

            tokens.Add(new Token(token_id, token_parameters));
        }

        return tokens;
    }

    public void PerformIterationsAndExecute(string input, int n)
    {
        List<Token> tokens = Parse(input);

        for (int i = 0; i < n; ++i)
        {
            tokens = rules.Apply(tokens);
        }

        execution.Execute(tokens);
    }
}
