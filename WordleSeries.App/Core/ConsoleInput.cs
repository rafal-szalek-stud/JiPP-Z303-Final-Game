using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text;

namespace WordleSeries.App.Core;

public static class ConsoleInput
{
    
    /// Czyta linię z konsoli w trybie nieblokującym (polling KeyAvailable),
    /// umożliwiając przerwanie przez CancellationToken (np. koniec czasu).
    /// Zwraca null, jeśli anulowano.
    
    public static async Task<string?> ReadLineAsync(CancellationToken ct)
    {
        var sb = new StringBuilder();

        while (!ct.IsCancellationRequested)
        {
            while (Console.KeyAvailable)
            {
                var key = Console.ReadKey(intercept: true);

                if (key.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine();
                    return sb.ToString();
                }

                if (key.Key == ConsoleKey.Backspace)
                {
                    if (sb.Length > 0)
                    {
                        sb.Length--;
                        // usuń znak z konsoli
                        Console.Write("\b \b");
                    }
                    continue;
                }

                // akceptujemy tylko znaki drukowalne
                if (!char.IsControl(key.KeyChar))
                {
                    sb.Append(key.KeyChar);
                    Console.Write(key.KeyChar);
                }
            }

            // małe opóźnienie, żeby nie mielić CPU
            try
            {
                await Task.Delay(25, ct);
            }
            catch (TaskCanceledException)
            {
                break;
            }
        }

        // anulowano (np. skończył się czas)
        return null;
    }
}

