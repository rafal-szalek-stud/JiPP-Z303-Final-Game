using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordleSeries.App.Core;

namespace WordleSeries.App.Players;

public delegate void HpChangedHandler(object sender, HealthPoints newHp);
public delegate void ScoreChangedHandler(object sender, Score newScore);

