using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordleSeries.App.Core;

namespace WordleSeries.App.Players;

public abstract class PlayerBase : IPlayer
{
    public string Nick { get; protected set; }

    public HealthPoints HP { get; protected set; }
    public Score TotalScore { get; protected set; }

    public event HpChangedHandler? HpChanged;
    public event ScoreChangedHandler? ScoreChanged;

    protected PlayerBase(string nick, HealthPoints initialHp)
    {
        Nick = nick;
        HP = initialHp;
        TotalScore = new Score(0);
    }

    public abstract string GetGuess(int wordLength);

    public void DecreaseHpByOneSecond()
    {
        HP--;
        HpChanged?.Invoke(this, HP);
    }

    public void AddScore(Score add)
    {
        TotalScore = TotalScore + add;
        ScoreChanged?.Invoke(this, TotalScore);
    }
}

