using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordleSeries.App.Ranking;

public record RankingEntry(string Nick, int Score, DateTime WhenUtc);

public interface IRankingRepository
{
    IReadOnlyList<RankingEntry> LoadTop(int count);
    void SaveResult(string nick, int totalScore, DateTime whenUtc);
}
