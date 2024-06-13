Console.WriteLine("Введите логическое выражение:");
string expression = Console.ReadLine(); 

var evaluator = new TruthTableGenerator();
int n = evaluator.CountVariables(expression);  
int result = evaluator.PrintTruthTable(n, expression);





