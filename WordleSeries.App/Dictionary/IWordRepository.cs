using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordleSeries.App.Dictionary;

public interface IWordRepository
{
    IReadOnlyList<string> GetWords(int length);
    bool IsValidWord(string word);
}

