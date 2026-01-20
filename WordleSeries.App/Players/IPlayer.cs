using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordleSeries.App.Core;

namespace WordleSeries.App.Players;

public interface IPlayer
{
    string Nick { get; }
    HealthPoints HP { get; }
    Score TotalScore { get; }

    string GetGuess(int wordLength);
}

