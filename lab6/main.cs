Dictionary dictionary = new Dictionary();

while (true)
{
    Console.WriteLine("Выберите действие:");
    Console.WriteLine("1. Добавить термин");
    Console.WriteLine("2. Найти термин");
    Console.WriteLine("3. Удалить термин");
    Console.WriteLine("4. Показать все термины");
    Console.WriteLine("5. Выход");

    int choice;
    while (!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > 5)
    {
        Console.WriteLine("Некорректный ввод. Попробуйте снова.");
    }

    switch (choice)
    {
        case 1:
            Console.WriteLine("Введите термин:");
            string termToAdd = Console.ReadLine();
            Console.WriteLine("Введите определение:");
            string definitionToAdd = Console.ReadLine();
            dictionary.AddTerm(termToAdd, definitionToAdd);
            Console.WriteLine("Термин успешно добавлен.");
            break;
        case 2:
            Console.WriteLine("Введите термин для поиска:");
            string termToSearch = Console.ReadLine();
            Console.WriteLine(dictionary.SearchTerm(termToSearch));
            break;
        case 3:
            Console.WriteLine("Введите термин для удаления:");
            string termToDelete = Console.ReadLine();
            Console.WriteLine(dictionary.DeleteTerm(termToDelete));
            break;
        case 4:
            dictionary.DisplayAllTerms();
            break;
        case 5:
            Console.WriteLine("Выход из программы.");
            return;
    }
}
