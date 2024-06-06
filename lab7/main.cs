int indexWord1, indexWord2, indexWordResult, indexAddress;
TableGenerator table = new TableGenerator(16);
table.PrintTable();

Console.Write("Введите индекс слова: ");
indexWord1 = int.Parse(Console.ReadLine());
Console.WriteLine("Слово: " + table.FindWord(indexWord1) + Environment.NewLine);

Console.Write("Введите индекс адреса слова: ");
indexAddress = int.Parse(Console.ReadLine());
Console.WriteLine("Адрес слова: " + table.FindAddressWord(indexAddress) + Environment.NewLine);

Console.Write("Введите индекс первого слова: ");
indexWord1 = int.Parse(Console.ReadLine());
Console.Write("Введите индекс второго слова: ");
indexWord2 = int.Parse(Console.ReadLine());
Console.Write("Введите индекс столбца для результата: ");
indexWordResult = int.Parse(Console.ReadLine());
Console.WriteLine("F15: ");
Console.WriteLine(table.FindWord(indexWord1));
Console.WriteLine(table.FindWord(indexWord2));
Console.WriteLine(table.ConstantOne(indexWordResult));
table.PrintTable();
Console.WriteLine();

Console.Write("Введите индекс первого слова: ");
indexWord1 = int.Parse(Console.ReadLine());
Console.Write("Введите индекс второго слова: ");
indexWord2 = int.Parse(Console.ReadLine());
Console.Write("Введите индекс столбца для результата: ");
indexWordResult = int.Parse(Console.ReadLine());
Console.WriteLine("F0: ");
Console.WriteLine(table.FindWord(indexWord1));
Console.WriteLine(table.FindWord(indexWord2));
Console.WriteLine(table.ConstantZero(indexWordResult));
table.PrintTable();
Console.WriteLine();

Console.Write("Введите индекс первого слова: ");
indexWord1 = int.Parse(Console.ReadLine());
Console.Write("Введите индекс второго слова: ");
indexWord2 = int.Parse(Console.ReadLine());
Console.Write("Введите индекс столбца для результата: ");
indexWordResult = int.Parse(Console.ReadLine());
Console.WriteLine("F5: ");
Console.WriteLine(table.FindWord(indexWord1));
Console.WriteLine(table.FindWord(indexWord2));
Console.WriteLine(table.ReturnSecondWord(indexWord2, indexWordResult));
table.PrintTable();
Console.WriteLine();

Console.Write("Введите индекс первого слова: ");
indexWord1 = int.Parse(Console.ReadLine());
Console.Write("Введите индекс второго слова: ");
indexWord2 = int.Parse(Console.ReadLine());
Console.Write("Введите индекс столбца для результата: ");
indexWordResult = int.Parse(Console.ReadLine());
Console.WriteLine("F10: ");
Console.WriteLine(table.FindWord(indexWord1));
Console.WriteLine(table.FindWord(indexWord2));
Console.WriteLine(table.ReverseSecondWord(indexWord2, indexWordResult));
table.PrintTable();
Console.WriteLine();

Console.Write("Введите ключ для поиска совпадений: ");
string key = Console.ReadLine();
List<string> listToCheck = table.TransformToStr(0, table.MatrixSize); // Получаем список всех слов
List<string> matchingWords = table.MatchFilter(key, listToCheck);

Console.WriteLine("Подходящие слова под ключ '" + key + "':");
foreach (string word in matchingWords)
{
    Console.WriteLine(word);
}

Console.WriteLine("Замена подходящих слов: ");
table.SummaOfFields();
table.PrintTable();
