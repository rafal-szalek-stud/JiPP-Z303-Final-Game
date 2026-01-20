using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace WordleSeries.App.Ranking;

public sealed class FileRankingRepository : IRankingRepository
{
    private readonly string _path;

    public FileRankingRepository(string path) => _path = path;

    public IReadOnlyList<RankingEntry> LoadTop(int count)
    {
        var all = LoadAll();
        return all
            .OrderByDescending(e => e.Score)
            .ThenByDescending(e => e.WhenUtc)
            .Take(count)
            .ToList();
    }

    public void SaveResult(string nick, int totalScore, DateTime whenUtc)
    {
        var all = LoadAll();
        all.Add(new RankingEntry(nick, totalScore, whenUtc));
        SaveAll(all);
    }

    private List<RankingEntry> LoadAll()
    {
        if (!File.Exists(_path)) return new List<RankingEntry>();

        var json = File.ReadAllText(_path);
        return JsonSerializer.Deserialize<List<RankingEntry>>(json) ?? new List<RankingEntry>();
    }

    private void SaveAll(List<RankingEntry> entries)
    {
        var json = JsonSerializer.Serialize(entries, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_path, json);
    }
}

