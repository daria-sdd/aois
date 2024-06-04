Number number1 = new Number(-5);
Console.WriteLine("Первое число");
number1.PrintCodes();

Number number2 = new Number(4);
Console.WriteLine("Второе число");
number2.PrintCodes();

Number sum = Number.Add(number1, number2);
Console.WriteLine("Сумма");
sum.PrintCodes();

Number difference = Number.Subtract(number1, number2);
Console.WriteLine("Разность");
difference.PrintCodes();

Number product = Number.Multiply(number1, number2);
Console.WriteLine("Умножение");
product.PrintCodes();

Number product2 = Number.Divide(number1, number2);
Console.WriteLine("Деление");
product2.PrintCodes();



Console.ReadLine();


FloatingPointNumber num1 = new FloatingPointNumber(10.5f);
FloatingPointNumber num2 = new FloatingPointNumber(2.3f);

Console.WriteLine("Число 1:");
num1.PrintBinary();
Console.WriteLine("Число 2:");
num2.PrintBinary();

FloatingPointNumber sumFloat = FloatingPointNumber.Add(num1, num2);

Console.WriteLine("Сумма:");
sumFloat.PrintBinary();

