using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordleSeries.App.Core;

public enum LetterState
{
    Absent,   // szare
    Present,  // żółte
    Correct   // zielone
}

public readonly struct Feedback
{
    public LetterState[] States { get; }

    public Feedback(LetterState[] states) => States = states;

    public bool Equals(Feedback other)
    {
        if (States.Length != other.States.Length) return false;
        for (int i = 0; i < States.Length; i++)
            if (States[i] != other.States[i]) return false;
        return true;
    }

    public override string ToString()
    {
        //  G=Correct, Y=Present, _=Absent
        return string.Concat(States.Select(s => s switch
        {
            LetterState.Correct => "G",
            LetterState.Present => "Y",
            _ => "_"
        }));
    }
}

