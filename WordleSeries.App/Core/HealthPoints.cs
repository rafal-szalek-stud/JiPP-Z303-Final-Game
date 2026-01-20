using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordleSeries.App.Core;

public readonly struct HealthPoints
{
    public int Value { get; }

    public HealthPoints(int value) => Value = value < 0 ? 0 : value;

    public static HealthPoints operator +(HealthPoints hp, int add) => new(hp.Value + add);
    public static HealthPoints operator -(HealthPoints hp, int sub) => new(hp.Value - sub);

    // Spadek o 1 (czas/HP co sekundę)
    public static HealthPoints operator --(HealthPoints hp) => new(hp.Value - 1);

    public static bool operator <=(HealthPoints a, HealthPoints b) => a.Value <= b.Value;
    public static bool operator >=(HealthPoints a, HealthPoints b) => a.Value >= b.Value;
    public static bool operator <(HealthPoints a, HealthPoints b) => a.Value < b.Value;
    public static bool operator >(HealthPoints a, HealthPoints b) => a.Value > b.Value;

    public override string ToString() => Value.ToString();
}

