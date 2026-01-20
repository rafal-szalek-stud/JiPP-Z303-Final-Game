using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordleSeries.App.Core;

public sealed class SecondElapsedEventArgs : EventArgs
{
    public int TimeLeftSeconds { get; }
    public SecondElapsedEventArgs(int timeLeftSeconds) => TimeLeftSeconds = timeLeftSeconds;
}

public sealed class GuessSubmittedEventArgs : EventArgs
{
    public string PlayerNick { get; }
    public string Guess { get; }
    public int AttemptNumber { get; }

    public GuessSubmittedEventArgs(string playerNick, string guess, int attemptNumber)
    {
        PlayerNick = playerNick;
        Guess = guess;
        AttemptNumber = attemptNumber;
    }
}

public sealed class RoundEndedEventArgs : EventArgs
{
    public string PlayerNick { get; }
    public string Secret { get; }
    public bool Solved { get; }
    public int AttemptsUsed { get; }
    public int MaxAttempts { get; }
    public int TimeLeftSeconds { get; }
    public int RoundScore { get; }

    public RoundEndedEventArgs(string playerNick, string secret, bool solved, int attemptsUsed, int maxAttempts, int timeLeftSeconds, int roundScore)
    {
        PlayerNick = playerNick;
        Secret = secret;
        Solved = solved;
        AttemptsUsed = attemptsUsed;
        MaxAttempts = maxAttempts;
        TimeLeftSeconds = timeLeftSeconds;
        RoundScore = roundScore;
    }
}

public delegate void SecondElapsedHandler(object sender, SecondElapsedEventArgs e);
public delegate void GuessSubmittedHandler(object sender, GuessSubmittedEventArgs e);
public delegate void RoundEndedHandler(object sender, RoundEndedEventArgs e);

