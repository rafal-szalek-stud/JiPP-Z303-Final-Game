using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordleSeries.App.Core;
using WordleSeries.App.Dictionary;

namespace WordleSeries.App.Players;

public sealed class BotPlayer : PlayerBase
{
    private readonly IWordRepository _repo;
    private readonly Random _rng = new();
    private List<string> _candidates = new();
    private string? _lastGuess;

    public BotPlayer(string nick, HealthPoints initialHp, IWordRepository repo) : base(nick, initialHp)
    {
        _repo = repo;
        ResetCandidates(5);
    }

    public override string GetGuess(int wordLength)
    {
        if (_candidates.Count == 0)
            ResetCandidates(wordLength);

        var guess = _candidates[_rng.Next(_candidates.Count)];
        _candidates.Remove(guess);
        _lastGuess = guess;

        Console.WriteLine($"[{Nick}] (BOT) zgaduje: {guess}");
        return guess;
    }

    public void ResetCandidates(int wordLength)
    {
        _candidates = _repo.GetWords(wordLength).ToList();
    }

    // Hook pod zawężanie po feedbacku 
    public void NarrowCandidates(Func<string, string, bool> keepCandidatePredicate)
    {
        if (_lastGuess is null) return;
        string guess = _lastGuess;

        _candidates = _candidates
            .Where(candidate => keepCandidatePredicate(candidate, guess))
            .ToList();
    }
    public void OnFeedbackComputed(object? sender, FeedbackComputedEventArgs e)
    {
        if (e.PlayerNick != this.Nick) return;
        if (_lastGuess is null) return;

        string guess = _lastGuess;
        var fb = e.Feedback;

        _candidates = _candidates
            .Where(candidate => WordleEvaluator.Evaluate(candidate, guess).Equals(fb))
            .ToList();
    }
}

