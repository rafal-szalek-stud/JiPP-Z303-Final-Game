using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordleSeries.App.Dictionary;
using WordleSeries.App.Players;

namespace WordleSeries.App.Core;

public sealed class WordleRound
{
    public event SecondElapsedHandler? SecondElapsed;
    public event GuessSubmittedHandler? GuessSubmitted;
    public event FeedbackComputedHandler? FeedbackComputed;
    public event RoundEndedHandler? RoundEnded;

    private readonly IWordRepository _repo;
    private readonly string _secret;
    private readonly int _maxAttempts;
    private readonly int _maxTimeSeconds;

    public WordleRound(IWordRepository repo, string secret, int maxAttempts, int maxTimeSeconds)
    {
        _repo = repo;
        _secret = secret;
        _maxAttempts = maxAttempts;
        _maxTimeSeconds = maxTimeSeconds;
    }

    public async Task<int> PlayAsync(PlayerBase player, KeyboardTracker keyboardTracker)
    {
        int timeLeft = _maxTimeSeconds;
        var usedGuesses = new HashSet<string>();
        bool solved = false;
        int attemptsUsed = 0;

        FeedbackComputed += keyboardTracker.OnFeedbackComputed;

        using var roundCts = new CancellationTokenSource();
        var ct = roundCts.Token;

        // Task tickujący co 1s
        var tickTask = Task.Run(async () =>
        {
            try
            {
                while (!ct.IsCancellationRequested && timeLeft > 0 && player.HP.Value > 0)
                {
                    await Task.Delay(1000, ct);

                    if (ct.IsCancellationRequested) break;

                    timeLeft--;
                    player.DecreaseHpByOneSecond();
                    SecondElapsed?.Invoke(this, new SecondElapsedEventArgs(timeLeft));

                    if (timeLeft <= 0 || player.HP.Value <= 0)
                    {
                        roundCts.Cancel(); // zakończ rundę
                        break;
                    }
                }
            }
            catch (TaskCanceledException) { }
        }, ct);

        try
        {
            while (!ct.IsCancellationRequested && timeLeft > 0 && attemptsUsed < _maxAttempts && player.HP.Value > 0)
            {
                Console.WriteLine($"Runda: czas/HP: {timeLeft}s / {player.HP.Value}  proba: {attemptsUsed + 1}/{_maxAttempts}");
                keyboardTracker.PrintAbsentLetters();

                string? guess;

                if (player is Players.BotPlayer)
                {
                    // Bot nie potrzebuje async inputu — zgaduje natychmiast
                    guess = player.GetGuess(5);
                    Console.WriteLine();
                }
                else
                {
                    Console.Write($"[{player.Nick}] Podaj slowo (5) i Enter: ");
                    guess = await ConsoleInput.ReadLineAsync(ct);
                    if (guess is null)
                    {
                        Console.WriteLine("\nCzas minal.\n");
                        break;
                    }
                    guess = guess.Trim().ToLowerInvariant();
                }

                // Jeśli w trakcie wpisywania skończył się czas
                if (ct.IsCancellationRequested) break;

                // Walidacje
                if (guess.Length != 5 || guess.Any(ch => ch < 'a' || ch > 'z'))
                {
                    Console.WriteLine("Niepoprawny format (wymagane 5 liter a-z). Sprobuj ponownie.\n");
                    continue;
                }

                if (!_repo.IsValidWord(guess))
                {
                    Console.WriteLine("To slowo nie istnieje w slowniku. Sprobuj ponownie.\n");
                    continue;
                }

                if (!usedGuesses.Add(guess))
                {
                    Console.WriteLine("To slowo juz bylo uzyte. Sprobuj inne.\n");
                    continue;
                }

                attemptsUsed++;
                GuessSubmitted?.Invoke(this, new GuessSubmittedEventArgs(player.Nick, guess, attemptsUsed));

                var fb = WordleEvaluator.Evaluate(_secret, guess);
                Console.WriteLine($"Feedback: {fb}");
                FeedbackComputed?.Invoke(this, new FeedbackComputedEventArgs(player.Nick, guess, fb));

                // Stop po trafieniu
                if (fb.States.All(s => s == LetterState.Correct))
                {
                    solved = true;
                    roundCts.Cancel(); // zatrzymaj tick
                    break;
                }

                Console.WriteLine();
            }
        }
        finally
        {
            // zatrzymaj tick i poczekaj aż się domknie
            if (!roundCts.IsCancellationRequested) roundCts.Cancel();
            try { await tickTask; } catch { /* ignorujemy */ }

            FeedbackComputed -= keyboardTracker.OnFeedbackComputed;
        }

        int roundScore = CalculateRoundScore(solved, attemptsUsed, _maxAttempts, player.HP.Value);

        if (solved)
        {
            player.AddScore(new Score(roundScore));
            Console.WriteLine($"Odgadniete! Haslo: {_secret}. Wynik rundy: {roundScore}\n");
        }
        else
        {
            Console.WriteLine($"Koniec rundy. Haslo: {_secret}. Wynik rundy: 0\n");
        }

        RoundEnded?.Invoke(this, new RoundEndedEventArgs(player.Nick, _secret, solved, attemptsUsed, _maxAttempts, timeLeft, solved ? roundScore : 0));

        return solved ? roundScore : 0;
    }
    private static int CalculateRoundScore(bool solved, int attemptsUsed, int maxAttempts, int hpLeft)
    {
        if (!solved) return 0;

        int unused = Math.Max(0, maxAttempts - attemptsUsed);
        int baseScore = 100;
        int bonusUnused = 10 * unused;
        int bonusTime = Math.Max(0, hpLeft); // pozostałe HP jako premia czasowa
        return baseScore + bonusUnused + bonusTime;
    }
}

