using WordleSeries.App.Core;
using WordleSeries.App.Dictionary;
using WordleSeries.App.Logging;
using WordleSeries.App.Players;
using WordleSeries.App.Ranking;

static int ReadTimeMode()
{
    while (true)
    {
        Console.WriteLine("Wybierz tryb czasowy:");
        Console.WriteLine("1) 60 sekund");
        Console.WriteLine("2) 120 sekund");
        Console.WriteLine("3) 300 sekund");
        Console.Write("Wybor: ");

        var choice = (Console.ReadLine() ?? "").Trim();
        if (choice == "1") return 60;
        if (choice == "2") return 120;
        if (choice == "3") return 300;

        Console.WriteLine("Niepoprawny wybor.\n");
    }
}


static void PrintRanking(IRankingRepository ranking, int top = 10)
{
    Console.WriteLine("\n=== RANKING ===");
    var list = ranking.LoadTop(top);
    if (list.Count == 0)
    {
        Console.WriteLine("(brak wynikow)");
        return;
    }

    int i = 1;
    foreach (var e in list)
    {
        Console.WriteLine($"{i,2}. {e.Nick}  {e.Score,6}  ({e.WhenUtc:u})");
        i++;
    }
}

var baseDir = AppContext.BaseDirectory;
var guessesPath = Path.Combine(baseDir, "Resources", "guesses_5.txt");
var answersPath = Path.Combine(baseDir, "Resources", "answers_5.txt");


var repo = new FileWordRepository(guessesPath);
var answers = new AnswerProvider(answersPath, repo);

while (true)
{
    Console.Clear();
    Console.WriteLine("WORDLE SERIES (3 rundy)");
    Console.WriteLine("1) Nowy gracz");
    Console.WriteLine("2) Demo (BOT)");
    Console.WriteLine("3) Ranking");
    Console.WriteLine("0) Wyjscie");
    Console.Write("Wybor: ");
    var choice = (Console.ReadLine() ?? "").Trim();
    if (choice == "3")
    {
        Console.Clear();

        int mode = ReadTimeMode(); // wybór 60/120/300
        var selectedRankingPath = Path.Combine(baseDir, "Resources", $"ranking_{mode}.json");
        var selectedRankingRepo = new FileRankingRepository(selectedRankingPath);

        Console.WriteLine($"RANKING — TRYB {mode}s\n");
        PrintRanking(selectedRankingRepo, top: 20);


        Console.WriteLine("\nNacisnij Enter, aby wrocic do menu...");
        Console.ReadLine();
        continue;
    }

    if (choice == "0") break;

    int maxTime = ReadTimeMode();
    var rankingPath = Path.Combine(baseDir, "Resources", $"ranking_{maxTime}.json");
    var rankingRepo = new FileRankingRepository(rankingPath);
    var initialHp = new HealthPoints(maxTime);

    PlayerBase player;
    if (choice == "2")
    {
        player = new BotPlayer("BOT", initialHp, repo);
    }
    else
    {
        Console.WriteLine();
        var nick = NickValidator.ReadNick3();
        player = new HumanPlayer(nick, initialHp);
    }

    var commentator = new Commentator();
    var match = new WordleSeriesMatch(repo, answers, commentator, maxAttempts: 6, maxTimeSeconds: maxTime);

    Console.Clear();
    Console.WriteLine($"Start serii 3 rund. Limit czasu/HP: {maxTime}\n");

    int totalScore = await match.PlayThreeRoundsAsync(player);


    Console.WriteLine("=== PODSUMOWANIE ===");
    Console.WriteLine($"Wynik laczny: {totalScore}\n");

    Console.WriteLine("Czy wyswietlic log przebiegu? (t/n)");
    var showLog = (Console.ReadLine() ?? "").Trim().ToLowerInvariant();
    if (showLog == "t" || showLog == "tak")
    {
        Console.WriteLine("\n=== LOG ===");
        commentator.PrintLog();
        Console.WriteLine();
    }

    // Zapis do rankingu tylko dla człowieka 
    if (player is HumanPlayer)
    {
        // nick już jest w player.Nick
        rankingRepo.SaveResult(player.Nick, totalScore, DateTime.UtcNow);
        Console.WriteLine("Wynik zapisany do rankingu.\n");
    }

    PrintRanking(rankingRepo);

    Console.WriteLine("\nNacisnij Enter, aby przejsc dalej (nowy gracz)...");
    Console.ReadLine();
}
