using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordleSeries.App.Core;

public sealed class KeyboardTracker
{
    private readonly HashSet<char> _absent = new();
    private readonly Dictionary<char, LetterState> _best = new();

    public void OnFeedbackComputed(object? sender, FeedbackComputedEventArgs e)
    {
        string guess = e.Guess.ToLowerInvariant();
        var states = e.Feedback.States;

        var hasNonAbsent = new HashSet<char>();
        for (int i = 0; i < guess.Length; i++)
        {
            if (states[i] == LetterState.Present || states[i] == LetterState.Correct)
                hasNonAbsent.Add(guess[i]);
        }

        for (int i = 0; i < guess.Length; i++)
        {
            char ch = guess[i];
            UpgradeBestState(ch, states[i]);

            if (states[i] == LetterState.Absent && !hasNonAbsent.Contains(ch))
            {
                if (!_best.TryGetValue(ch, out var best) || best == LetterState.Absent)
                    _absent.Add(ch);
            }
        }

        foreach (var kv in _best)
        {
            if (kv.Value == LetterState.Present || kv.Value == LetterState.Correct)
                _absent.Remove(kv.Key);
        }
    }

    private void UpgradeBestState(char ch, LetterState newState)
    {
        if (!_best.TryGetValue(ch, out var current))
        {
            _best[ch] = newState;
            return;
        }

        int Rank(LetterState s) => s switch
        {
            LetterState.Absent => 0,
            LetterState.Present => 1,
            LetterState.Correct => 2,
            _ => 0
        };

        if (Rank(newState) > Rank(current))
            _best[ch] = newState;
    }

    public void PrintAbsentLetters()
    {
        var letters = _absent.OrderBy(c => c).ToArray();
        Console.WriteLine(letters.Length == 0
            ? "Wykluczone litery: (brak)"
            : $"Wykluczone litery: {string.Join(" ", letters).ToUpperInvariant()}");
    }
}

