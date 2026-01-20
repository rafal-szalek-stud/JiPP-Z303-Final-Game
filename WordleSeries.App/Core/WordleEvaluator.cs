using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordleSeries.App.Core;

public static class WordleEvaluator
{
    public static Feedback Evaluate(string secret, string guess)
    {
        secret = secret.ToLowerInvariant();
        guess = guess.ToLowerInvariant();

        int n = secret.Length;
        var result = new LetterState[n];
        var remaining = new int[26];

        // 1) Correct
        for (int i = 0; i < n; i++)
        {
            if (guess[i] == secret[i])
            {
                result[i] = LetterState.Correct;
            }
            else
            {
                int idx = secret[i] - 'a';
                if (idx >= 0 && idx < 26) remaining[idx]++;
            }
        }

        // 2) Present / Absent
        for (int i = 0; i < n; i++)
        {
            if (result[i] == LetterState.Correct) continue;

            int g = guess[i] - 'a';
            if (g < 0 || g >= 26)
            {
                result[i] = LetterState.Absent;
                continue;
            }

            if (remaining[g] > 0)
            {
                result[i] = LetterState.Present;
                remaining[g]--;
            }
            else
            {
                result[i] = LetterState.Absent;
            }
        }

        return new Feedback(result);
    }
}

