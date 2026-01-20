using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace WordleSeries.App.Dictionary;

public sealed class AnswerProvider
{
    private readonly List<string> _answers = new();
    private readonly Random _rng = new();

    private static readonly Regex Allowed = new("^[a-z]+$", RegexOptions.Compiled);

    public AnswerProvider(string answersPath, IWordRepository repo)
    {
        LoadAnswers(answersPath, repo);
    }

    public string GetRandomAnswer(int length)
    {
        var candidates = _answers.Where(a => a.Length == length).ToList();
        if (candidates.Count == 0)
            throw new InvalidOperationException($"Brak hasel o dlugosci {length}.");

        return candidates[_rng.Next(candidates.Count)];
    }

    private void LoadAnswers(string path, IWordRepository repo)
    {
        if (!File.Exists(path))
            throw new FileNotFoundException($"Nie znaleziono pliku answers: {path}");

        foreach (var raw in File.ReadAllLines(path))
        {
            var w = raw.Trim().ToLowerInvariant();
            if (w.Length == 0) continue;
            if (!Allowed.IsMatch(w)) continue;

            //answers muszą być podzbiorem guesses
            if (!repo.IsValidWord(w)) continue;

            _answers.Add(w);
        }
    }
}

