# JiPP-Z303-Final-Game

### Cel gry

Twoim zadaniem jest odgadnięcie 3 różnych 5-literowych haseł (seria 3 rund). W każdej rundzie masz ograniczoną liczbę prób oraz limit czasu.

### Wybór trybu

#### Po uruchomieniu gry wybierz z menu:

1) Nowy gracz – grasz samodzielnie

2) Demo (BOT) – gra bot (pokaz działania algorytmu)

3) Ranking – przegląd rankingów (osobno dla 60/120/300 s)

0) Wyjście

#### Następnie wybierasz tryb czasowy:

1) 60 sekund

2) 120 sekund

3) 300 sekund

#### Tryb czasowy wpływa na:

- limit czasu w każdej rundzie,

- początkowe “HP” (HP = czas w sekundach),

- osobny ranking dla każdego trybu.

### Sterowanie (Nowy gracz)

1) Wpisz swój 3-literowy nick (A–Z), np. ABC.

2) W każdej rundzie wpisuj propozycje słów (dokładnie 5 liter, tylko a–z, bez polskich znaków) i zatwierdzaj Enterem.

3) Możesz używać Backspace do poprawiania wpisu.

#### Gra odrzuci próbę, jeśli:

- słowo nie ma 5 liter,

- zawiera znaki inne niż a–z,

- nie istnieje w słowniku gry,

- powtórzysz to samo słowo w tej samej rundzie.

### Feedback po każdej próbie

#### Po wpisaniu słowa gra wyświetla “feedback” dla 5 liter:

- G – litera na właściwym miejscu (zielone)

- Y – litera występuje w haśle, ale w innym miejscu (żółte)

- _ – litery nie ma w haśle (szare)

#### Dodatkowo gra pokazuje listę:

- Wykluczone litery – litery, które na pewno nie występują w haśle (rosną po kolejnych próbach).

### Czas i HP (real-time)

- W każdej rundzie czas i HP spadają co 1 sekundę.

- HP = pozostały czas w sekundach.

- Jeśli czas/HP dojdzie do zera, runda kończy się przegraną.

#### Limit czasu dotyczy każdej rundy osobno (po zakończeniu rundy czas i HP resetują się na startową wartość trybu).

### Punktacja

Punkty są naliczane za każdą rundę, a potem sumowane dla całej serii 3 rund.

#### Jeśli odgadniesz hasło w rundzie:

- 100 pkt za rozwiązanie hasła,

- +10 pkt za każdą niewykorzystaną próbę,

- +pozostałe HP (premia za czas – im szybciej, tym więcej).

#### Jeśli nie odgadniesz hasła w rundzie:

- wynik rundy = 0.

Po 3 rundach zobaczysz podsumowanie i wynik łączny.

### Log przebiegu

Po zakończeniu serii możesz wybrać wyświetlenie logu (komentator), który pokazuje m.in.:

- zgłoszone próby,

- feedback,

- przebieg czasu,

- wynik rundy.

### Zapis wyniku i ranking

#### Po serii 3 rund:

- wynik gracza jest zapisywany do rankingu tylko dla wybranego trybu (60/120/300),

- następnie wyświetlany jest ranking (np. Top 20).

### Tryb BOT (Demo)

#### W trybie demo bot:

- zgaduje słowa ze słownika,

- po każdej próbie zawęża listę kandydatów na podstawie feedbacku (jak solver Wordle),

- pokazuje swoje próby i przechodzi 3 rundy.
