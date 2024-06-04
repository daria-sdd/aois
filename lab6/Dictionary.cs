using System;
using System.Collections.Generic;

public class Dictionary
{
    private int capacity;
    private List<SortedDictionary<string, string>> table;
    private readonly Dictionary<char, int> alphabet;

    public Dictionary(int capacity = 20)
    {
        this.capacity = capacity;
        table = new List<SortedDictionary<string, string>>(capacity);
        for (int i = 0; i < capacity; i++)
        {
            table.Add(new SortedDictionary<string, string>());
        }
        alphabet = new Dictionary<char, int>();
        string letters = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";
        for (int i = 0; i < letters.Length; i++)
        {
            alphabet.Add(letters[i], i);
        }
    }

    private int HashFunction(string key)
    {
        if (key.Length < 2)
        {
            return 0;
        }

        int first = alphabet[char.ToUpper(key[0])];
        int second = alphabet[char.ToUpper(key[1])];
        return (first * 33 + second) % capacity;
    }

    public void AddTerm(string term, string definition)
    {
        int index = HashFunction(term);
        table[index][term] = definition;
    }

    public string SearchTerm(string term)
    {
        int index = HashFunction(term);
        if (table[index].TryGetValue(term, out string definition))
        {
            return $"Определение для '{term}': {definition}";
        }
        return "Термин не найден в словаре.";
    }

    public string DeleteTerm(string term)
    {
        int index = HashFunction(term);
        if (table[index].Remove(term))
        {
            return "Термин успешно удален.";
        }
        return "Термин не найден в словаре.";
    }

    public void DisplayAllTerms()
    {
        Console.WriteLine("Список всех терминов и их определений:");
        for (int i = 0; i < capacity; ++i)
        {
            int displayIndex = i + 1;
            if (table[i].Count == 0)
            {
                Console.WriteLine($"[{displayIndex}]");
            }
            else
            {
                foreach (var item in table[i])
                {
                    Console.WriteLine($"[{displayIndex}] {item.Key} - {item.Value}");
                }
            }
        }
    }
}





