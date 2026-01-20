using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace WordleSeries.App.Players;

public static class NickValidator
{
    private static readonly Regex NickRx = new("^[A-Z]{3}$", RegexOptions.Compiled);

    public static string ReadNick3()
    {
        while (true)
        {
            Console.Write("Wpisz 3-literowy nick (A-Z): ");
            var input = (Console.ReadLine() ?? "").Trim().ToUpperInvariant();

            if (NickRx.IsMatch(input))
                return input;

            Console.WriteLine("Niepoprawny nick. Wymagane dokladnie 3 litery A-Z.\n");
        }
    }
}

