using System.Text;

class BinaryTable
{
    static List<string> SplitDnf(string s)
    {
        char delimiter = '|';
        return s.Split(delimiter).ToList();
    }

    static List<string> SplitImplicants(string input)
    {
        var result = new List<string>();
        string temp = "";
        for (int i = 0; i < input.Length; i++)
        {
            if (input[i] == '!')
            {
                temp += input[i];
                continue;
            }
            if (input[i] == '|' || input[i] == '(' || input[i] == ')' || input[i] == ' ' || input[i] == '&')
            {
                if (!string.IsNullOrEmpty(temp))
                {
                    result.Add(temp);
                    temp = "";
                }
                continue;
            }
            temp += input[i];
        }
        if (!string.IsNullOrEmpty(temp))
        {
            result.Add(temp);
        }
        return result;
    }

    static bool AreLettersEqual(List<string> lit1, List<string> lit2)
    {
        var letters1 = lit1.Select(literal => literal.TrimStart('!')).ToList();
        var letters2 = lit2.Select(literal => literal.TrimStart('!')).ToList();
        letters1.Sort();
        letters2.Sort();
        return letters1.SequenceEqual(letters2);
    }

    static string MinimizeDnf(string dnf)
    {
        var clauses = SplitDnf(dnf);
        if (!clauses.Any() || clauses.Count == 1)
        {
            return clauses.Any() ? clauses[0] : "";
        }
        var result = new StringBuilder();
        var matrix = new int[clauses.Count, clauses[0].Length];
        bool changed = true;
        int stage = 1;
        while (changed)
        {
            bool merged = false;
            var newClauses = new List<string>();
            changed = false;
            Console.WriteLine($"Шаг {stage}:");
            for (int i = 0; i < clauses.Count; ++i)
            {
                merged = false;
                for (int j = 0; j < clauses.Count; ++j)
                {
                    if (i != j)
                    {
                        var literals1 = SplitImplicants(clauses[i]);
                        var literals2 = SplitImplicants(clauses[j]);

                        var matchingLiterals = literals1.Where(literal => literals2.Contains(literal)).ToList();

                        if (AreLettersEqual(literals1, literals2) && matchingLiterals.Count >= literals1.Count - 1)
                        {
                            string mergedClause = string.Join("&", matchingLiterals);
                            if (!newClauses.Contains(mergedClause))
                            {
                                newClauses.Add(mergedClause);
                                changed = true;
                                Console.WriteLine($"Слияние: {clauses[i]} с {clauses[j]} -> {mergedClause}");
                            }
                            merged = true;
                        }
                    }
                }
                if (!merged)
                {
                    newClauses.Add(clauses[i]);
                }
            }
            clauses = newClauses;
            ++stage;
        }

        for (int i = 0; i < clauses.Count; ++i)
        {
            result.Append($"({clauses[i]})");
            if (i < clauses.Count - 1)
            {
                result.Append(" | ");
            }
        }
        Console.WriteLine(result.ToString());
        return result.ToString();
    }

    static string PrintSDNF(int[] h)
    {
        var sdnf = new StringBuilder();
        for (int i = 0; i < 32; ++i)
        {
            if (h[i] == 1)
            {
                sdnf.Append($"({(i < 8 ? "q4'" : "!q4'")} & {((i / 4) % 2 == 1 ? "q3'" : "!q3'")} & {((i / 2) % 2 == 1 ? "q2'" : "!q2'")} & {(i % 2 == 1 ? "q1'" : "!q1'")}) | ");
            }
        }
        string sdnfStr = sdnf.ToString();
        sdnfStr = sdnfStr.Substring(0, sdnfStr.Length - 3);
        return sdnfStr;
    }

    static List<bool> AddOne(List<bool> binaryVector)
    {
        int carry = 1;
        for (int i = binaryVector.Count - 1; i >= 0; --i)
        {
            if (binaryVector[i] && carry == 1)
            {
                binaryVector[i] = false;
                carry = 1;
            }
            else if (!binaryVector[i] && carry == 1)
            {
                binaryVector[i] = true;
                carry = 0;
            }
            else
            {
                break;
            }
        }
        if (carry == 1)
        {
            return new List<bool>(new bool[binaryVector.Count]);
        }

        return binaryVector;
    }

    static void PrintBinaryTable()
    {
        int[] h4 = new int[32], h3 = new int[32], h2 = new int[32], h1 = new int[32];
        int[] q1 = { 0, 1, 1, 0, 0, 1, 1, 0, 0, 1, 1, 0, 0, 1, 1, 0, 0, 1, 1, 0, 0, 1, 1, 0, 0, 1, 1, 0, 0, 1, 1, 0 };
        int[] q2 = { 0, 1, 0, 0, 1, 0, 1, 1, 0, 1, 0, 0, 1, 0, 1, 1, 0, 1, 0, 0, 1, 0, 1, 1, 0, 1, 0, 0, 1, 0, 1, 1 };
        int[] q3 = { 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 1, 1, 1, 1, 1, 1, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 1, 1, 1, 1, 1, 1 };
        int[] q4 = { 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };

        for (int i = 0; i < 32; ++i)
        {
            int q1_prime = (i / 2) % 2;
            int q2_prime = (i / 4) % 2;
            int q3_prime = (i / 8) % 2;
            int q4_prime = (i < 16) ? 0 : 1;

            h1[i] = (q1[i] != q1_prime) ? 1 : 0;
            h2[i] = (q2[i] != q2_prime) ? 1 : 0;
            h3[i] = (q3[i] != q3_prime) ? 1 : 0;
            h4[i] = (q4[i] != q4_prime) ? 1 : 0;
        }
        Console.WriteLine();
        Console.WriteLine("q4'\tq3'\tq2'\tq1'\tV\tq4\tq3\tq2\tq1\th4\th3\th2\th1");

        for (int i = 0; i < 32; ++i)
        {
            Console.WriteLine($"{(i < 16 ? 0 : 1)}\t{((i / 8) % 2)}\t{((i / 4) % 2)}\t{((i / 2) % 2)}\t{(i % 2)}\t{q4[i]}\t{q3[i]}\t{q2[i]}\t{q1[i]}\t{h4[i]}\t{h3[i]}\t{h2[i]}\t{h1[i]}");
        }

        string sdnf_h1 = PrintSDNF(h1);
        string sdnf_h2 = PrintSDNF(h2);
        string sdnf_h3 = PrintSDNF(h3);
        string sdnf_h4 = PrintSDNF(h4);

        Console.WriteLine($"\nСДНФ for h1:\n{sdnf_h1}");
        string minimized_h1 = MinimizeDnf(sdnf_h1);
        Console.WriteLine($"\nСДНФ for h2:\n{sdnf_h2}");
        string minimized_h2 = MinimizeDnf(sdnf_h2);
        Console.WriteLine($"\nСДНФ for h3:\n{sdnf_h3}");
        string minimized_h3 = MinimizeDnf(sdnf_h3);
        Console.WriteLine($"\nСДНФ for h4:\n{sdnf_h4}");
        string minimized_h4 = MinimizeDnf(sdnf_h4);
    }

    static void Main()
    {
        PrintBinaryTable();
    }
}
