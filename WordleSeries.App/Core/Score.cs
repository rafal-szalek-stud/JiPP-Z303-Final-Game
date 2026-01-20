using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordleSeries.App.Core;

public readonly struct Score
{
    public int Value { get; }

    public Score(int value) => Value = value < 0 ? 0 : value;

    public static Score operator +(Score a, Score b) => new(a.Value + b.Value);
    public static Score operator +(Score a, int add) => new(a.Value + add);

    public override string ToString() => Value.ToString();
}

