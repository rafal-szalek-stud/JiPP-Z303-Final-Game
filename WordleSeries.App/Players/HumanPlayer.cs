using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordleSeries.App.Core;

namespace WordleSeries.App.Players;

public sealed class HumanPlayer : PlayerBase
{
    public HumanPlayer(string nick, HealthPoints initialHp) : base(nick, initialHp) { }

    public override string GetGuess(int wordLength)
    {
        Console.Write($"[{Nick}] Podaj slowo ({wordLength}): ");
        return (Console.ReadLine() ?? "").Trim().ToLowerInvariant();
    }
}

