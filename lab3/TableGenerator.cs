using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class TableGenerator
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
        return (a && b) || (!a && !b);
    }

    public int Precedence(char op)
    {
        switch (op)
        {
            case '!': return 3;
            case '&':
            case '|': return 2;
            case '>':
            case '~': return 1;
            default: return 0;
        }
    }

    public string ProcessOperator(Stack<char> st, string postfix, char c)
    {
        while (st.Count > 0 && Precedence(c) <= Precedence(st.Peek()))
        {
            postfix += st.Pop();
        }
        st.Push(c);
        return postfix;
    }

    public string ProcessClosingParenthesis(Stack<char> st, string postfix)
    {
        while (st.Count > 0 && st.Peek() != '(')
        {
            postfix += st.Pop();
        }
        st.Pop();
        return postfix;
    }

    public string ProcessRemainingOperators(Stack<char> st, string postfix)
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
        StringBuilder postfix = new StringBuilder();
        foreach (char c in s)
        {
            if (char.IsLetter(c))
                postfix.Append(c);
            else if (c == '&' || c == '|' || c == '!' || c == '>' || c == '~')
                postfix.Append(ProcessOperator(st, postfix.ToString(), c));
            else if (c == '(')
                st.Push(c);
            else if (c == ')')
                postfix.Append(ProcessClosingParenthesis(st, postfix.ToString()));
        }
        postfix.Append(ProcessRemainingOperators(st, postfix.ToString()));
        return postfix.ToString();
    }

    public bool ProcessUnaryOperator(char c, bool operand)
    {
        if (c == '!')
            return Not(operand);
        return false;
    }

    public bool ProcessBinaryOperator(char c, bool operand1, bool operand2)
    {
        switch (c)
        {
            case '&': return And(operand1, operand2);
            case '|': return Or(operand1, operand2);
            case '>': return Implication(operand1, operand2);
            case '~': return Equivalention(operand1, operand2);
            default: return false;
        }
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
                    result = ProcessUnaryOperator(c, operand2);
                else
                {
                    bool operand1 = st.Pop();
                    result = ProcessBinaryOperator(c, operand1, operand2);
                }
                st.Push(result);
                results.Add(result);
            }
        }
        return results;
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
            if ("&|!>~".Contains(c))
                Console.Write(c + "\t");
        }
        Console.WriteLine();
    }

    public void PrintRow(List<bool> values, List<bool> results)
    {
        foreach (bool value in values)
        {
            Console.Write((value ? 1 : 0) + "\t");
        }

        foreach (bool result in results)
        {
            Console.Write((result ? 1 : 0) + "\t");
        }
        Console.WriteLine();
    }



    public string UpdateSKNF(List<bool> values, string sknf)
    {
        sknf += "(";
        for (int j = 0; j < values.Count; ++j)
        {
            sknf += (values[j] ? "!" : "") + ((char)('a' + j)).ToString() + (j < values.Count - 1 ? "|" : "");
        }
        sknf += ")&";
        return sknf;
    }

    public string UpdateSDNF(List<bool> values, string sdnf)
    {
        sdnf += "(";
        for (int j = 0; j < values.Count; ++j)
        {
            sdnf += (values[j] ? "" : "!") + ((char)('a' + j)).ToString() + (j < values.Count - 1 ? "&" : "");
        }
        sdnf += ")|";
        return sdnf;
    }

    public void PrintSKNF(string sknf, List<int> sknfIndices)
    {
        Console.WriteLine("СКНФ: " + sknf);
        Console.Write("СКНФ индексы: ");
        foreach (int index in sknfIndices)
        {
            Console.Write(index + " ");
        }
        Console.WriteLine();
    }

    public void PrintSDNF(string sdnf, List<int> sdnfIndices)
    {
        Console.WriteLine("СДНФ: " + sdnf);
        Console.Write("СДНФ индексы: ");
        foreach (int index in sdnfIndices)
        {
            Console.Write(index + " ");
        }
        Console.WriteLine();
    }

    public List<Tuple<int, int>> FindClauses(string sknf)
    {
        List<Tuple<int, int>> positions = new List<Tuple<int, int>>();
        int openBracket = 0;
        int shift = 0;

        for (int i = 0; i < sknf.Length; ++i)
        {
            if (sknf[i] == '(')
            {
                if (shift == 0)
                {
                    openBracket = i;
                }
                shift++;
            }
            else if (sknf[i] == ')')
            {
                shift--;
                if (shift == 0)
                {
                    positions.Add(Tuple.Create(openBracket, i));
                }
            }
        }

        return positions;
    }

    public string ExtractClause(string sknf, int start, int end)
    {
        return sknf.Substring(start, end - start + 1);
    }

    public List<string> GetClauses(string sknf)
    {
        List<string> clauses = new List<string>();
        List<Tuple<int, int>> positions = FindClauses(sknf);

        foreach (var pos in positions)
        {
            clauses.Add(ExtractClause(sknf, pos.Item1, pos.Item2));
        }

        return clauses;
    }

    public bool IsLiteralStart(char c)
    {
        return char.IsLetter(c) || c == '!';
    }

    public string ExtractLiteral(string clause, ref int pos)
    {
        if (clause[pos] == '!')
        {
            if (pos + 1 < clause.Length && char.IsLetter(clause[pos + 1]))
            {
                return "!" + clause[++pos];
            }
        }
        else if (char.IsLetter(clause[pos]))
        {
            return clause[pos].ToString();
        }
        return string.Empty;
    }

    public List<string> GetLits(string clause)
    {
        List<string> lits = new List<string>();
        for (int i = 0; i < clause.Length; ++i)
        {
            if (IsLiteralStart(clause[i]))
            {
                string lit = ExtractLiteral(clause, ref i);
                if (!string.IsNullOrEmpty(lit))
                {
                    lits.Add(lit);
                }
            }
        }
        return lits;
    }

    public string RemoveNegation(string lit)
    {
        return lit.Substring(lit.IndexOf('!') == 0 ? 1 : 0);
    }

    public List<string> NormalizeLiterals(List<string> lits)
    {
        List<string> normalized = new List<string>();
        foreach (string lit in lits)
        {
            normalized.Add(RemoveNegation(lit));
        }
        normalized.Sort();
        return normalized;
    }

    public bool EqualLetters(List<string> lits1, List<string> lits2)
    {
        List<string> normalizedLits1 = NormalizeLiterals(lits1);
        List<string> normalizedLits2 = NormalizeLiterals(lits2);
        return normalizedLits1.SequenceEqual(normalizedLits2);
    }

    public bool IsDelimiter(char c)
    {
        return c == '|' || c == '(' || c == ')' || c == ' ' || c == '&';
    }

    public void AddToResult(List<string> result, ref string temp)
    {
        if (!string.IsNullOrEmpty(temp))
        {
            result.Add(temp);
            temp = string.Empty;
        }
    }

    public List<string> Splitting(string input)
    {
        List<string> result = new List<string>();
        string temp = "";
        foreach (char c in input)
        {
            if (c == '!')
            {
                temp += c;
                continue;
            }
            if (IsDelimiter(c))
            {
                AddToResult(result, ref temp);
                continue;
            }
            temp += c;
        }
        AddToResult(result, ref temp);
        return result;
    }

    public bool OriginalSubstring(string subConstituent, string originalConstituent)
    {
        List<string> sublits = Splitting(subConstituent);
        List<string> originallits = Splitting(originalConstituent);

        foreach (string lit in sublits)
        {
            if (!originallits.Contains(lit))
            {
                return false;
            }
        }
        return true;
    }

    public void PrintMergedClausesTable(List<string> clauses, List<string> originalClauses)
    {
        Console.WriteLine("Расчетно-табличный метод:");
        Console.Write("{0,15}", " ");
        foreach (var clause in originalClauses)
        {
            Console.Write("{0,15}", clause);
        }
        Console.WriteLine();

        foreach (var clause in clauses)
        {
            Console.Write("{0,15}", clause);
            foreach (var originalClause in originalClauses)
            {
                if (originalClause.Contains(clause))
                {
                    bool containsSubstring = OriginalSubstring(clause, originalClause);
                    Console.Write("{0,15}", containsSubstring ? "X" : " ");
                }
                else
                {
                    Console.Write("{0,15}", " ");
                }
            }
            Console.WriteLine();
        }
    }

    public string MinimizeSDNFTable(string sdnf)
    {
        List<string> clauses = GetClauses(sdnf);
        if (clauses.Count == 0 || clauses.Count == 1)
        {
            return clauses.Count == 0 ? "" : clauses[0];
        }

        string result = "";
        bool changed = true;
        int stage = 1;
        while (changed)
        {
            bool merged = false;
            List<string> newClauses = new List<string>();
            changed = false;

            Console.WriteLine("Шаг " + stage + ":");

            for (int i = 0; i < clauses.Count; ++i)
            {
                merged = false;
                for (int j = 0; j < clauses.Count; ++j)
                {
                    if (i != j)
                    {
                        List<string> lits1 = GetLits(clauses[i]);
                        List<string> lits2 = GetLits(clauses[j]);

                        List<string> matchingLits = new List<string>();
                        if (EqualLetters(lits1, lits2))
                        {
                            foreach (string lit in lits1)
                            {
                                if (lits2.Contains(lit))
                                {
                                    matchingLits.Add(lit);
                                }
                            }

                            if (matchingLits.Count >= lits1.Count - 1)
                            {
                                string mergedClause = matchingLits.Aggregate((a, b) => a + "&" + b);
                                if (!newClauses.Contains(mergedClause))
                                {
                                    newClauses.Add(mergedClause);
                                    changed = true;
                                    Console.WriteLine("Объединение: " + clauses[i] + " с " + clauses[j] + " -> " + mergedClause);
                                }
                                merged = true;
                            }
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
            result += "(" + clauses[i] + ")";
            if (i < clauses.Count - 1)
            {
                result += " | ";
            }
        }

        PrintMergedClausesTable(clauses, GetClauses(sdnf));
        return result;
    }

    public string MinimizeSKNFTable(string sknf)
    {
        List<string> clauses = GetClauses(sknf);
        if (clauses.Count == 0 || clauses.Count == 1)
        {
            return clauses.Count == 0 ? "" : clauses[0];
        }

        string result = "";
        bool changed = true;
        int stage = 1;
        while (changed)
        {
            bool merged = false;
            List<string> newClauses = new List<string>();
            changed = false;

            Console.WriteLine("Шаг " + stage + ":");

            for (int i = 0; i < clauses.Count; ++i)
            {
                merged = false;
                for (int j = 0; j < clauses.Count; ++j)
                {
                    if (i != j)
                    {
                        List<string> lits1 = GetLits(clauses[i]);
                        List<string> lits2 = GetLits(clauses[j]);

                        List<string> matchingLits = new List<string>();
                        if (EqualLetters(lits1, lits2))
                        {
                            foreach (string lit in lits1)
                            {
                                if (lits2.Contains(lit))
                                {
                                    matchingLits.Add(lit);
                                }
                            }

                            if (matchingLits.Count >= lits1.Count - 1)
                            {
                                string mergedClause = matchingLits.Skip(1).Aggregate(matchingLits[0], (a, b) => a + "|" + b);
                                if (!newClauses.Contains(mergedClause))
                                {
                                    newClauses.Add(mergedClause);
                                    changed = true;
                                    Console.WriteLine("Объединение: " + clauses[i] + " с " + clauses[j] + " -> " + mergedClause);
                                }
                                merged = true;
                            }
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
            result += "(" + clauses[i] + ")";
            if (i < clauses.Count - 1)
            {
                result += " & ";
            }
        }

        PrintMergedClausesTable(clauses, GetClauses(sknf));
        return result;
    }

    public int ToGray(int number)
    {
        return number ^ (number >> 1);
    }

    public List<string> ExtractVariablesSKNF(string sknf)
    {
        HashSet<string> variables = new HashSet<string>();
        StringBuilder term = new StringBuilder();
        bool insideTerm = false;

        foreach (char ch in sknf)
        {
            if (ch == '(')
            {
                insideTerm = true;
                term.Clear();
            }
            else if (ch == ')')
            {
                insideTerm = false;
                if (term.Length > 0)
                {
                    string[] vars = term.ToString().Split('|');
                    foreach (string var in vars)
                    {
                        string cleanedVar = var.Trim().Replace("!", "");
                        if (!string.IsNullOrEmpty(cleanedVar))
                        {
                            variables.Add(cleanedVar);
                        }
                    }
                }
            }
            else if (insideTerm)
            {
                term.Append(ch);
            }
        }

        return variables.ToList();
    }

    public List<string> ExtractVariablesSDNF(string sdnf)
    {
        HashSet<string> variables = new HashSet<string>();
        StringBuilder term = new StringBuilder();
        bool insideTerm = false;

        foreach (char ch in sdnf)
        {
            if (ch == '(')
            {
                insideTerm = true;
                term.Clear();
            }
            else if (ch == ')')
            {
                insideTerm = false;
                if (term.Length > 0)
                {
                    string[] vars = term.ToString().Split('&');
                    foreach (string var in vars)
                    {
                        string cleanedVar = var.Trim().Replace("!", "");
                        if (!string.IsNullOrEmpty(cleanedVar))
                        {
                            variables.Add(cleanedVar);
                        }
                    }
                }
            }
            else if (insideTerm)
            {
                term.Append(ch);
            }
        }

        return new List<string>(variables);
    }

    public void MinimizeSKNFByKarnaugh(string sknf, List<int> index)
    {
        List<string> variables = ExtractVariablesSKNF(sknf);
        int n = variables.Count;
        int rows, cols;

        if (n % 2 == 0)
        {
            rows = cols = 1 << (n / 2);
        }
        else
        {
            rows = 1 << (n / 2);
            cols = 1 << ((n + 1) / 2);
        }

        int[][] karnaughMap = new int[rows][];
        for (int i = 0; i < rows; i++)
        {
            karnaughMap[i] = new int[cols];
        }

        for (int i = 0; i < index.Count; i++)
        {
            int row = ToGray(i / cols);
            int col = ToGray(i % cols);
            karnaughMap[row][col] = index[i];
        }

        Console.WriteLine("Карта Карно:");
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                Console.Write(karnaughMap[i][j] + "   ");
            }
            Console.WriteLine();
            Console.WriteLine();
        }
    }

    public void MinimizeSDNFByKarnaugh(string sdnf, List<int> index)
    {
        List<string> variables = ExtractVariablesSDNF(sdnf);
        int n = variables.Count;
        int rows, cols;

        if (n % 2 == 0)
        {
            rows = cols = 1 << (n / 2);
        }
        else
        {
            rows = 1 << (n / 2);
            cols = 1 << ((n + 1) / 2);
        }

        int[][] karnaughMap = new int[rows][];
        for (int i = 0; i < rows; i++)
        {
            karnaughMap[i] = new int[cols];
        }

        for (int i = 0; i < index.Count; i++)
        {
            int row = ToGray(i / cols);
            int col = ToGray(i % cols);
            karnaughMap[row][col] = index[i];
        }

        Console.WriteLine("Карта Карно для СДНФ:");
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                Console.Write(karnaughMap[i][j] + "   ");
            }
            Console.WriteLine();
            Console.WriteLine();
        }
    }

    public void Menu(int n, string expression)
    {
        string postfixExpression = InfixToPostfix(expression);
        int totalRows = 1 << n;

        PrintHeader(expression, postfixExpression);

        string sknf = "";
        string sdnf = "";
        List<int> sknfIndices = new List<int>();
        List<int> sdnfIndices = new List<int>();
        List<int> newResult = new List<int>();
        List<bool> results = new List<bool>();

        for (int i = 0; i < totalRows; ++i)
        {
            List<bool> values = new List<bool>(new bool[n]);
            for (int j = 0; j < n; ++j)
            {
                values[n - j - 1] = (i & (1 << j)) != 0;
            }

            results = EvaluatePostfix(postfixExpression, values);
            PrintRow(values, results);

            if (!results.Last())
            {
                sknf = UpdateSKNF(values, sknf);
                sknfIndices.Add(i);
                newResult.Add(0);
            }
            else
            {
                sdnf = UpdateSDNF(values, sdnf);
                sdnfIndices.Add(i);
                newResult.Add(1);
            }
        }

        if (!string.IsNullOrEmpty(sknf))
            sknf = sknf.Remove(sknf.Length - 1);
        if (!string.IsNullOrEmpty(sdnf))
            sdnf = sdnf.Remove(sdnf.Length - 1);

        PrintSKNF(sknf, sknfIndices);
        PrintSDNF(sdnf, sdnfIndices);

        string minimizedSDNF = MinimizeSDNFTable(sdnf);
        Console.WriteLine("Минимизация СДНФ: " + minimizedSDNF);

        string minimizedSKNF = MinimizeSKNFTable(sknf);
        Console.WriteLine("Минимизация СКНФ: " + minimizedSKNF);

        MinimizeSKNFByKarnaugh(sknf, newResult);
        Console.WriteLine("Минимизация СКНФ картой Карно:");
        Console.WriteLine(minimizedSKNF);

        // MinimizeSDNFByKarnaugh(sdnf, newResult);
        Console.WriteLine("Минимизация СДНФ картой Карно:");
        Console.WriteLine(minimizedSDNF);

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
        List<int> decimalResults = new List<int>();

        for (int i = 0; i < totalRows; ++i)
        {
            List<bool> values = new List<bool>(new bool[n]);
            for (int j = 0; j < n; ++j)
            {
                values[n - j - 1] = (i & (1 << j)) != 0;
            }

            List<bool> results = EvaluatePostfix(postfixExpression, values);
            PrintRow(values, results);

            decimalResults.Add(results.Last() ? 1 : 0);

            if (!results.Last())
            {
                sknf = UpdateSKNF(values, sknf);
                sknfIndices.Add(i);
            }
            else
            {
                sdnf = UpdateSDNF(values, sdnf);
                sdnfIndices.Add(i);
            }
        }

        if (!string.IsNullOrEmpty(sknf)) sknf = sknf.Remove(sknf.Length - 2);
        if (!string.IsNullOrEmpty(sdnf)) sdnf = sdnf.Remove(sdnf.Length - 2);

        PrintSKNF(sknf, sknfIndices);
        PrintSDNF(sdnf, sdnfIndices);

        string binaryRepresentation = string.Join("", decimalResults);
        int binaryResult = Convert.ToInt32(binaryRepresentation, 2);

        Console.WriteLine("Индексная форма: " + binaryRepresentation + " - " + binaryResult);
        return binaryResult;
    }



}
