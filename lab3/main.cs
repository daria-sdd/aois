Console.WriteLine("Введите уравнение: ");
string expression = Console.ReadLine();

HashSet<char> variables = new HashSet<char>();
foreach (char c in expression)
{
    if (char.IsLetter(c))
    {
        variables.Add(c);
    }
}


TableGenerator program = new TableGenerator();
program.Menu(expression.Count(char.IsLetter), expression);

TableGenerator generator = new TableGenerator();

