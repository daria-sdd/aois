public class TruthTableGenerator
{
    public bool And(bool a, bool b)
    {
        return a && b;
    }

    public bool Or(bool a, bool b)
    {
        return a || b;
    }

    public bool Not(bool a)
    {
        return !a;
    }

    public bool Implication(bool a, bool b)
    {
        return !(a && !b);
    }

    public bool Equivalention(bool a, bool b)
    {
        return a == b;
    }

    public int Precedence(char op)
    {
        if (op == '!') return 3;
        if (op == '&' || op == '|') return 2;
        if (op == '>' || op == '~') return 1;
        return 0;
    }

    public string Operator(Stack<char> st, string postfix, char c)
    {
        while (st.Count > 0 && Precedence(c) <= Precedence(st.Peek()))
        {
            postfix += st.Pop();
        }
        st.Push(c);
        return postfix;
    }

    public string ClosingParenthesis(Stack<char> st, string postfix)
    {
        while (st.Count > 0 && st.Peek() != '(')
        {
            postfix += st.Pop();
        }
        st.Pop();
        return postfix;
    }

    public string RemainingOperators(Stack<char> st, string postfix)
    {
        while (st.Count > 0)
        {
            postfix += st.Pop();
        }
        return postfix;
    }

    public string InfixToPostfix(string s)
    {
        Stack<char> st = new Stack<char>();
        string postfix = string.Empty;
        foreach (char c in s)
        {
            if (char.IsLetter(c))
                postfix += c;
            else if (c == '&' || c == '|' || c == '!' || c == '>' || c == '~')
            {
                postfix = Operator(st, postfix, c);
            }
            else if (c == '(')
            {
                st.Push(c);
            }
            else if (c == ')')
            {
                postfix = ClosingParenthesis(st, postfix);
            }
        }
        postfix = RemainingOperators(st, postfix);
        return postfix;
    }

    public bool UnaryOperator(char c, bool operand)
    {
        if (c == '!') return Not(operand);
        return false;
    }

    public bool BinaryOperator(char c, bool operand1, bool operand2)
    {
        if (c == '&') return And(operand1, operand2);
        if (c == '|') return Or(operand1, operand2);
        if (c == '>') return Implication(operand1, operand2);
        if (c == '~') return Equivalention(operand1, operand2);
        return false;
    }

    public List<bool> EvaluatePostfix(string postfixExpression, List<bool> values)
    {
        Stack<bool> st = new Stack<bool>();
        List<bool> results = new List<bool>();
        foreach (char c in postfixExpression)
        {
            if (char.IsLetter(c))
                st.Push(values[c - 'a']);
            else
            {
                bool result;
                bool operand2 = st.Pop();

                if (c == '!')
                    result = UnaryOperator(c, operand2);
                else
                {
                    bool operand1 = st.Pop();
                    result = BinaryOperator(c, operand1, operand2);
                }
                st.Push(result);
                results.Add(result);
            }
        }
        return results;
    }

    public int BinaryToDecimal(List<int> decimalResult)
    {
        int decimalValue = 0;
        int baseValue = 1;

        for (int i = decimalResult.Count - 1; i >= 0; --i)
        {
            if (decimalResult[i] == 1) decimalValue += baseValue;
            baseValue *= 2;
        }
        return decimalValue;
    }

    public string GetBinaryRepresentation(List<int> decimalResult)
    {
        return string.Join("", decimalResult);
    }

    public void PrintHeader(string expression, string postfixExpression)
    {
        foreach (char c in expression)
        {
            if (char.IsLetter(c))
                Console.Write(c + "\t");
        }

        foreach (char c in postfixExpression)
        {
            if (c == '&' || c == '|' || c == '!' || c == '>' || c == '~')
                Console.Write(c + "\t");
        }
        Console.WriteLine();

        for (int i = 0; i < expression.Count(char.IsLetter); i++)
        {
            Console.Write("----\t");
        }

        foreach (char c in postfixExpression)
        {
            if (c == '&' || c == '|' || c == '!' || c == '>' || c == '~')
                Console.Write("----\t");
        }
        Console.WriteLine();
    }


    public void PrintRow(List<bool> values, List<bool> results)
    {
        foreach (var value in values)
        {
            Console.Write((value ? 1 : 0) + "\t");
        }

        foreach (var result in results)
        {
            Console.Write((result ? 1 : 0) + "\t");
        }
        Console.WriteLine();
    }

    public string SKNF(List<bool> values, string sknf)
    {
        sknf += "(";
        for (int j = 0; j < values.Count; ++j)
        {
            sknf += (values[j] ? "!" : "") + ((char)('a' + j)).ToString() + (j < values.Count - 1 ? " | " : "");
        }
        sknf += ") & ";
        return sknf;
    }

    public string SDNF(List<bool> values, string sdnf)
    {
        sdnf += "(";
        for (int j = 0; j < values.Count; ++j)
        {
            sdnf += (values[j] ? "" : "!") + ((char)('a' + j)).ToString() + (j < values.Count - 1 ? " & " : "");
        }
        sdnf += ") | ";
        return sdnf;
    }

    public void PrintSKNF(string sknf, List<int> sknfIndices)
    {
        Console.WriteLine("СКНФ: " + sknf);
        Console.Write("Индексы СКНФ: ");
        foreach (int index in sknfIndices)
        {
            Console.Write(index + " ");
        }
        Console.WriteLine();
    }

    public void PrintSDNF(string sdnf, List<int> sdnfIndices)
    {
        Console.WriteLine("СДНФ: " + sdnf);
        Console.Write("Индексы СДНФ: ");
        foreach (int index in sdnfIndices)
        {
            Console.Write(index + " ");
        }
        Console.WriteLine();
    }

    public int CountVariables(string expression)
    {
        HashSet<char> variables = new HashSet<char>();
        foreach (char c in expression)
        {
            if (char.IsLetter(c))
            {
                variables.Add(c);
            }
        }
        return variables.Count;
    }

    public int PrintTruthTable(int n, string expression)
    {
        string postfixExpression = InfixToPostfix(expression);
        int totalRows = 1 << n;

        PrintHeader(expression, postfixExpression);

        string sknf = string.Empty;
        string sdnf = string.Empty;
        List<int> sknfIndices = new List<int>();
        List<int> sdnfIndices = new List<int>();
        List<int> decimalResult = new List<int>();
        int binaryResult = 0;

        for (int i = 0; i < totalRows; ++i)
        {
            List<bool> values = new List<bool>(new bool[n]);
            for (int j = 0; j < n; ++j)
            {
                values[n - j - 1] = (i & (1 << j)) != 0;
            }

            List<bool> results = EvaluatePostfix(postfixExpression, values);
            PrintRow(values, results);

            decimalResult.Add(results.Last() ? 1 : 0);
            binaryResult = BinaryToDecimal(decimalResult);

            if (!results.Last())
            {
                sknf = SKNF(values, sknf);
                sknfIndices.Add(i);
            }
            else
            {
                sdnf = SDNF(values, sdnf);
                sdnfIndices.Add(i);
            }
        }

        if (!string.IsNullOrEmpty(sknf)) sknf = sknf.Remove(sknf.Length - 3);
        if (!string.IsNullOrEmpty(sdnf)) sdnf = sdnf.Remove(sdnf.Length - 3);

        PrintSKNF(sknf, sknfIndices);
        PrintSDNF(sdnf, sdnfIndices);

        string binaryRepresentation = GetBinaryRepresentation(decimalResult);
        Console.WriteLine("Индексная форма: " + binaryRepresentation + " - " + binaryResult);
        return binaryResult;
    }
}
