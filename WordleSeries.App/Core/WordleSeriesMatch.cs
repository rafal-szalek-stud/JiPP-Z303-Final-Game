using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordleSeries.App.Dictionary;
using WordleSeries.App.Logging;
using WordleSeries.App.Players;

namespace WordleSeries.App.Core;

public sealed class WordleSeriesMatch
{
    private readonly IWordRepository _repo;
    private readonly AnswerProvider _answers;
    private readonly Commentator _commentator;
    private readonly int _maxAttempts;
    private readonly int _maxTimeSeconds;

    public WordleSeriesMatch(IWordRepository repo, AnswerProvider answers, Commentator commentator, int maxAttempts, int maxTimeSeconds)
    {
        _repo = repo;
        _answers = answers;
        _commentator = commentator;
        _maxAttempts = maxAttempts;
        _maxTimeSeconds = maxTimeSeconds;
    }

    public async Task<int> PlayThreeRoundsAsync(PlayerBase player)

    {
        _commentator.AttachToPlayer(player);

        int total = 0;

        var usedSecrets = new HashSet<string>();

        for (int roundIndex = 1; roundIndex <= 3; roundIndex++)
        {
            Console.WriteLine($"=== RUNDA {roundIndex}/3 ===");

            player.ResetForNewRound(new HealthPoints(_maxTimeSeconds));

            string secret;
            int guard = 0;
            do
            {
                secret = _answers.GetRandomAnswer(5);
                guard++;
                if (guard > 1000)
                    throw new InvalidOperationException("Nie udalo sie wylosowac 3 unikalnych hasel. Sprawdz answers_5.txt (za malo hasel).");
            }
            while (!usedSecrets.Add(secret));

            var tracker = new KeyboardTracker();
            var round = new WordleRound(_repo, secret, _maxAttempts, _maxTimeSeconds);

            _commentator.AttachToRound(round);

            if (player is BotPlayer bp)
            {
                bp.ResetCandidates(5);
                round.FeedbackComputed += bp.OnFeedbackComputed;
            }
            int roundScore = await round.PlayAsync(player, tracker);
            total += roundScore;
        }

        return total;
    }
}

