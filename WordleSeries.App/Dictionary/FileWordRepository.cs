using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace WordleSeries.App.Dictionary;

public sealed class FileWordRepository : IWordRepository
{
    private readonly Dictionary<int, List<string>> _guessesByLen = new();
    private readonly HashSet<string> _validGuesses = new();

    private static readonly Regex Allowed = new("^[a-z]+$", RegexOptions.Compiled);

    public FileWordRepository(string guessesPath)
    {
        LoadGuesses(guessesPath);
    }

    public IReadOnlyList<string> GetWords(int length)
    {
        return _guessesByLen.TryGetValue(length, out var list) ? list : Array.Empty<string>();
    }

    public bool IsValidWord(string word)
    {
        word = (word ?? "").Trim().ToLowerInvariant();
        return _validGuesses.Contains(word);
    }

    private void LoadGuesses(string path)
    {
        if (!File.Exists(path))
            throw new FileNotFoundException($"Nie znaleziono pliku guesses: {path}");

        foreach (var raw in File.ReadAllLines(path))
        {
            var w = raw.Trim().ToLowerInvariant();
            if (w.Length == 0) continue;
            if (!Allowed.IsMatch(w)) continue;      // tylko a-z
            if (w.Length < 2) continue;

            _validGuesses.Add(w);

            if (!_guessesByLen.TryGetValue(w.Length, out var list))
            {
                list = new List<string>();
                _guessesByLen[w.Length] = list;
            }
            list.Add(w);
        }
    }
}

