using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordleSeries.App.Core;
using WordleSeries.App.Players;

namespace WordleSeries.App.Logging;

public sealed class Commentator
{
    private readonly List<string> _lines = new();

    public IReadOnlyList<string> Lines => _lines;

    public void AttachToRound(WordleRound round)
    {
        round.GuessSubmitted += (_, e) => Log($"[{e.PlayerNick}] Proba {e.AttemptNumber}: {e.Guess}");
        round.FeedbackComputed += (_, e) => Log($"[{e.PlayerNick}] Feedback: {e.Feedback}");
        round.SecondElapsed += (_, e) =>
        {
            if (e.TimeLeftSeconds % 5 == 0)
                Log($"Tick: czas pozostal {e.TimeLeftSeconds}s");
        };

        round.RoundEnded += (_, e) =>
        {
            Log($"Runda zakonczona: solved={e.Solved}, proby={e.AttemptsUsed}/{e.MaxAttempts}, czasLeft={e.TimeLeftSeconds}s, wynik={e.RoundScore}, secret={e.Secret}");
        };
    }

    public void AttachToPlayer(PlayerBase player)
    {
        player.HpChanged += (_, hp) => Log($"[{player.Nick}] HP={hp.Value}");
        player.ScoreChanged += (_, score) => Log($"[{player.Nick}] Score={score.Value}");
    }

    public void PrintLog(int lastN = 200)
    {
        foreach (var line in _lines.TakeLast(lastN))
            Console.WriteLine(line);
    }

    private void Log(string msg)
    {
        _lines.Add($"{DateTime.Now:HH:mm:ss} {msg}");
    }
}

