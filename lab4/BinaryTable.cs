using System;
using System.Collections.Generic;
using System.Linq;

public class BinaryTable
{
    private int[,] tableSum;
    private string[,] tableN;

    public BinaryTable()
    {
        tableSum = GenerateSumTable();
        tableN = GenerateShiftTable();
    }

    private int[,] GenerateSumTable()
    {
        int[,] table = new int[8, 5];

        for (int i = 0; i < 8; i++)
        {
            int a = (i >> 2) & 1;
            int b = (i >> 1) & 1;
            int c = i & 1;

            int sum = (a + b + c) % 2;
            int carry = (a + b + c) > 1 ? 1 : 0;

            table[i, 0] = a;
            table[i, 1] = b;
            table[i, 2] = c;
            table[i, 3] = sum;
            table[i, 4] = carry;
        }

        return table;
    }

    private string[,] GenerateShiftTable()
    {
        string[,] table = new string[16, 8];

        for (int i = 0; i < 16; i++)
        {
            int a = (i >> 3) & 1;
            int b = (i >> 2) & 1;
            int c = (i >> 1) & 1;
            int d = i & 1;

            int decimalValue = (a << 3) | (b << 2) | (c << 1) | d;
            int newValue = decimalValue + 9;
            if (newValue > 15)
            {
                table[i, 0] = a.ToString();
                table[i, 1] = b.ToString();
                table[i, 2] = c.ToString();
                table[i, 3] = d.ToString();
                table[i, 4] = "-";
                table[i, 5] = "-";
                table[i, 6] = "-";
                table[i, 7] = "-";
            }
            else
            {
                int e = (newValue >> 3) & 1;
                int f = (newValue >> 2) & 1;
                int g = (newValue >> 1) & 1;
                int h = newValue & 1;

                table[i, 0] = a.ToString();
                table[i, 1] = b.ToString();
                table[i, 2] = c.ToString();
                table[i, 3] = d.ToString();
                table[i, 4] = e.ToString();
                table[i, 5] = f.ToString();
                table[i, 6] = g.ToString();
                table[i, 7] = h.ToString();
            }
        }

        return table;
    }

    public void PrintSumTable()
    {
        Console.WriteLine("Table of sums with carry:");
        Console.WriteLine($"{"a",-2} {"b",-2} {"c",-2} {"sum",-3} {"carry",-5}");
        for (int i = 0; i < 8; i++)
        {
            Console.WriteLine($"{tableSum[i, 0],-2} {tableSum[i, 1],-2} {tableSum[i, 2],-2} {tableSum[i, 3],-3} {tableSum[i, 4],-5}");
        }
        Console.WriteLine();
    }

    public void PrintShiftTable()
    {
        Console.WriteLine("Table with shift by 9:");
        Console.WriteLine($"{"a",-2} {"b",-2} {"c",-2} {"d",-2} {"a+9",-4} {"b+9",-4} {"c+9",-4} {"d+9",-4}");
        for (int i = 0; i < 16; i++)
        {
            Console.WriteLine($"{tableN[i, 0],-2} {tableN[i, 1],-2} {tableN[i, 2],-2} {tableN[i, 3],-2} {tableN[i, 4],-4} {tableN[i, 5],-4} {tableN[i, 6],-4} {tableN[i, 7],-4}");
        }
        Console.WriteLine();
    }

    private string GetSDNF(int[,] table, int column, char[] variables)
    {
        int numRows = table.GetLength(0);
        int numCols = table.GetLength(1);

        string sdnf = "";

        for (int i = 0; i < numRows; i++)
        {
            if (table[i, column] == 1)
            {
                string term = "";
                for (int j = 0; j < numCols; j++)
                {
                    if (j >= variables.Length) continue;

                    if (table[i, j] == 1)
                        term += $"{variables[j]}";
                    else
                        term += $"!{variables[j]}";

                    term += " & ";
                }

                if (term.Length > 0)
                {
                    term = term.Remove(term.Length - 3); 
                }

                if (sdnf.Length > 0)
                {
                    sdnf += " | ";
                }

                sdnf += term;
            }
        }

        return sdnf;
    }

    private int[,] ConvertShiftTableToInt()
    {
        int[,] shiftTableInt = new int[16, 8];
        for (int i = 0; i < 16; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                shiftTableInt[i, j] = tableN[i, j] == "-" ? 0 : int.Parse(tableN[i, j]);
            }
        }
        return shiftTableInt;
    }

    public void PrintSDNFSumTable()
    {
        char[] sumVariables = { 'a', 'b', 'c' };
        string sdnfSum = GetSDNF(tableSum, 3, sumVariables);
        Console.WriteLine("СДНФ для столбца суммы в первой таблице:");
        Console.WriteLine(sdnfSum);
        Console.WriteLine();
        Console.WriteLine("Минимизированная СДНФ для столбца суммы в первой таблице:");
        Console.WriteLine(MinimizeSDNF(sdnfSum));
        Console.WriteLine();

        string sdnfCarry = GetSDNF(tableSum, 4, sumVariables);
        Console.WriteLine("СДНФ для столбца переноса в первой таблице:");
        Console.WriteLine(sdnfCarry);
        Console.WriteLine();
        Console.WriteLine("Минимизированная СДНФ для столбца переноса в первой таблице:");
        Console.WriteLine(MinimizeSDNF(sdnfCarry));
        Console.WriteLine();
    }

    public void PrintSDNFShiftTable()
    {
        int[,] shiftTableInt = ConvertShiftTableToInt();
        char[] shiftVariables = { 'a', 'b', 'c', 'd' };

        for (int i = 4; i <= 7; i++)
        {
            string sdnf = GetSDNF(shiftTableInt, i, shiftVariables);
            Console.WriteLine($"СДНФ для столбца {i} сдвига во второй таблице:");
            Console.WriteLine(sdnf);
            Console.WriteLine();
            Console.WriteLine($"Минимизированная СДНФ для столбца {i} сдвига во второй таблице:");
            Console.WriteLine(MinimizeSDNF(sdnf));
            Console.WriteLine();
        }
    }

    public string MinimizeSDNF(string sdnf)
    {
        var minterms = sdnf.Split(new[] { " | " }, StringSplitOptions.None).ToList();
        var primeImplicants = GetPrimeImplicants(minterms);
        var essentialPrimeImplicants = GetEssentialPrimeImplicants(primeImplicants, minterms);

        return string.Join(" | ", essentialPrimeImplicants);
    }

    private List<string> GetPrimeImplicants(List<string> minterms)
    {
        var primeImplicants = new List<string>(minterms);
        bool reduced;

        do
        {
            reduced = false;
            var newImplicants = new HashSet<string>();

            for (int i = 0; i < primeImplicants.Count; i++)
            {
                for (int j = i + 1; j < primeImplicants.Count; j++)
                {
                    var combined = CombineImplicants(primeImplicants[i], primeImplicants[j]);
                    if (combined != null)
                    {
                        newImplicants.Add(combined);
                        reduced = true;
                    }
                }
            }

            if (reduced)
            {
                primeImplicants = new List<string>(newImplicants);
            }

        } while (reduced);

        return primeImplicants;
    }

    private string CombineImplicants(string implicant1, string implicant2)
    {
        var literals1 = implicant1.Split(new[] { " & " }, StringSplitOptions.None);
        var literals2 = implicant2.Split(new[] { " & " }, StringSplitOptions.None);

        int diffCount = 0;
        string combined = "";

        for (int i = 0; i < literals1.Length; i++)
        {
            if (literals1[i] == literals2[i])
            {
                combined += literals1[i] + " & ";
            }
            else if (IsComplement(literals1[i], literals2[i]))
            {
                diffCount++;
            }
            else
            {
                return null;
            }
        }

        if (diffCount == 1)
        {
            return combined.TrimEnd(new[] { ' ', '&' });
        }

        return null;
    }

    private bool IsComplement(string literal1, string literal2)
    {
        return (literal1[0] == '!' && literal1.Substring(1) == literal2) ||
               (literal2[0] == '!' && literal2.Substring(1) == literal1);
    }

    private List<string> GetEssentialPrimeImplicants(List<string> primeImplicants, List<string> minterms)
    {
        var essentialPrimeImplicants = new List<string>();

        foreach (var minterm in minterms)
        {
            var coveringImplicants = primeImplicants.Where(p => Covers(p, minterm)).ToList();
            if (coveringImplicants.Count == 1)
            {
                essentialPrimeImplicants.Add(coveringImplicants[0]);
            }
        }

        return essentialPrimeImplicants.Distinct().ToList();
    }

    private bool Covers(string implicant, string minterm)
    {
        var implicantLiterals = implicant.Split(new[] { " & " }, StringSplitOptions.None);
        var mintermLiterals = minterm.Split(new[] { " & " }, StringSplitOptions.None);

        foreach (var literal in implicantLiterals)
        {
            if (!mintermLiterals.Contains(literal))
            {
                return false;
            }
        }

        return true;
    }


}


