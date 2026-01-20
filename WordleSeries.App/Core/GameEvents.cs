using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordleSeries.App.Core;

namespace WordleSeries.App.Core;

public sealed class FeedbackComputedEventArgs : EventArgs
{
    public string PlayerNick { get; }
    public string Guess { get; }
    public Feedback Feedback { get; }

    public FeedbackComputedEventArgs(string playerNick, string guess, Feedback feedback)
    {
        PlayerNick = playerNick;
        Guess = guess;
        Feedback = feedback;
    }
}

public delegate void FeedbackComputedHandler(object sender, FeedbackComputedEventArgs e);

