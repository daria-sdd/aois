using System.Text;

public class TableGenerator
{
    private int tableSize_;
    private bool[,] table_;

    public int MatrixSize
    {
        get { return tableSize_; }
    }

    public TableGenerator()
    {
        tableSize_ = 0;
        table_ = new bool[0, 0];
    }

    public TableGenerator(int size)
    {
        tableSize_ = size;
        table_ = new bool[tableSize_, tableSize_];
        ValueTable();
    }

    private void ValueTable()
    {
        Random rand = new Random();
        for (int i = 0; i < tableSize_; i++)
        {
            for (int j = 0; j < tableSize_; j++)
            {
                table_[i, j] = rand.Next(2) == 0;
            }
        }
    }

    private List<bool> MatchFilter(string key, List<string> listToCheck, List<bool> Matches)
    {
        for (int i = 0; i < Matches.Count; i++)
        {
            bool flag = true;
            for (int j = 0; j < key.Length; j++)
            {
                if (key[j] != listToCheck[i][j])
                {
                    flag = false;
                    break;
                }
            }
            if (!flag && Matches[i])
                Matches[i] = flag;
        }
        return Matches;
    }

    public List<string> TransformToStr(int startIndex, int finishIndex)
    {
        List<string> wordList = new List<string>();
        for (int i = startIndex; i < finishIndex; i++)
        {
            wordList.Add(FindWord(i));
        }
        return wordList;
    }

    public void ChangeWord(string newWord, int resultIndex)
    {
        for (int i = resultIndex; i < tableSize_; i++)
        {
            table_[i, resultIndex] = newWord[i - resultIndex] == '0' ? false : true;
        }
        for (int i = 0; i < resultIndex; i++)
        {
            table_[i, resultIndex] = newWord[newWord.Length - resultIndex + i] == '0' ? false : true;
        }
    }

    public void PrintTable()
    {
        for (int i = 0; i < tableSize_; i++)
        {
            for (int j = 0; j < tableSize_; j++)
            {
                Console.Write(table_[i, j] ? "1 " : "0 ");
            }
            Console.WriteLine();
        }
    }

    public string FindWord(int index)
    {
        StringBuilder result = new StringBuilder();
        for (int i = index; i < tableSize_; i++)
        {
            result.Append(table_[i, index] ? "1" : "0");
        }
        for (int i = 0; i < index; i++)
        {
            result.Append(table_[i, index] ? "1" : "0");
        }
        return result.ToString();
    }

    public string FindAddressWord(int index)
    {
        StringBuilder result = new StringBuilder();
        for (int j = 0; j < tableSize_; j++)
        {
            result.Append(table_[index, j] ? "1" : "0");
            index = (index + 1) % tableSize_;
        }
        return result.ToString();
    }

    public string ConstantZero(int resultIndex)
    {
        string result = new string('0', 16);
        ChangeWord(result, resultIndex);
        return result;
    }

    public List<string> MatchFilter(string key, List<string> listToCheck)
    {
        List<string> matchingWords = new List<string>();

        foreach (string word in listToCheck)
        {
            bool isMatch = true;
            for (int j = 0; j < key.Length; j++)
            {
                if (key[j] != word[j])
                {
                    isMatch = false;
                    break;
                }
            }
            if (isMatch)
            {
                matchingWords.Add(word);
            }
        }

        Console.WriteLine("Matching words for key '" + key + "':");
        foreach (string word in matchingWords)
        {
            Console.WriteLine(word);
        }

        return matchingWords;
    }


    public string ConstantOne(int resultIndex)
    {
        string result = new string('1', 16);
        ChangeWord(result, resultIndex);
        return result;
    }

    public string ReturnSecondWord(int secondIndex, int resultIndex)
    {
        string result = FindWord(secondIndex);
        ChangeWord(result, resultIndex);
        return result;
    }


    public string ReverseSecondWord(int secondIndex, int resultIndex)
    {
        string secondWord = FindWord(secondIndex);
        char[] charArray = secondWord.ToCharArray();
        for (int i = 0; i < charArray.Length; i++)
        {
            if (charArray[i] == '0') charArray[i] = '1'; 
            else if (charArray[i] == '1') charArray[i] = '0'; 
        }
        string result = new string(charArray);
        ChangeWord(result, resultIndex);
        return result;
    }


    private string ToBinary(int number)
    {
        return Convert.ToString(number, 2).PadLeft(5, '0');
    }

    private int BinaryToDecimal(string binary)
    {
        return Convert.ToInt32(binary, 2);
    }

    public void SummaOfFields()
    {
        List<string> wordList = TransformToStr(0, tableSize_);
        List<bool> checkMatch = Enumerable.Repeat(true, wordList.Count).ToList();
        checkMatch = MatchFilter("111", wordList, checkMatch);
        for (int i = 0; i < tableSize_; i++)
        {
            if (checkMatch[i])
            {
                Console.Write("(" + i + ") " + FindWord(i) + " -> ");
                string currentWord = FindWord(i);
                int numA = BinaryToDecimal(currentWord.Substring(3, 4));
                int numB = BinaryToDecimal(currentWord.Substring(7, 4));
                int summa = numA + numB;
                currentWord = currentWord.Remove(11, 5);
                string addToCurrentWord = ToBinary(summa);
                if (addToCurrentWord.Length < 5)
                {
                    addToCurrentWord = addToCurrentWord.PadLeft(5, '0');
                }
                currentWord += addToCurrentWord;
                Console.WriteLine(currentWord);
                ChangeWord(currentWord, i);
            }
        }
    }
}

